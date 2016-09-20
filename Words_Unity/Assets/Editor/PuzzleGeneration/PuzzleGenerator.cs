using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*
 * TODO - Add a progress bar to the generation
 */

public class PuzzleGenerator : EditorWindow
{
	private const string kProgressBarTitle = "Puzzle Generation";
	private const int kWordListProgressStep = 1000;

	[Range(5, 32)]
	public int Width = 7;
	[Range(5, 32)]
	public int Height = 7;
	[Range(1, 10)]
	public int WordListPasses = 1;
	[Range(1, 1024)]
	public int WordLimit = 100;
	[Range(1, 17)]
	public int MaxTileUsage = 5;

	private char INVALID_CHAR = ' ';
	private GridEntry[,] mGrid;

	private Words WordList;

	private List<EWordDirection> mWordDirections;
	private List<GridPosition> mGridPositions;

	private List<string> mWords;
	private List<ScoredPlacement> mWordPlacements;

	private List<ScoredPlacement> mScoredPlacements;
	private bool mHasPlacedInitialWord;

	private string[] mAllWords;
	private int mAllWordsCount;

	private int mPlacedWords;
	private int mMaxDimension;

	private PuzzleContents mNewPuzzleContents;

	[MenuItem("Words/Puzzle/Open Generator")]
	static public void ShowWindow()
	{
		PuzzleGenerator window = GetWindow(typeof(PuzzleGenerator)) as PuzzleGenerator;
		window.Initialise();
	}

	void OnGUI()
	{
		GUILayout.Label("Settings", EditorStyles.boldLabel);
		GUILayout.Space(8);

		Width = EditorGUILayout.IntSlider("Width", Width, 4, 32);
		Height = EditorGUILayout.IntSlider("Height", Height, 4, 32);
		WordListPasses = EditorGUILayout.IntSlider("Word List Passes", WordListPasses, 1, 5);
		WordLimit = EditorGUILayout.IntSlider("WordLimit", WordLimit, 1, 1024);
		MaxTileUsage = EditorGUILayout.IntSlider("MaxTileUsage", MaxTileUsage, 1, 17);

		GUILayout.Space(8);
		if (GUILayout.Button("Generate"))
		{
			Generate();
		}
	}

	private void Initialise()
	{
		GameObject wordListsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Words.prefab");
		if (wordListsPrefab)
		{
			WordList = wordListsPrefab.GetComponent<Words>();
		}

		mWords = new List<string>(WordLimit);
		mWordPlacements = new List<ScoredPlacement>(WordLimit);
	}

	public void Generate()
	{
		float startTime = Time.realtimeSinceStartup;

		SetupPositionalLists();

		bool wasGenerationSuccessful = GenerateInternal();
		if (!wasGenerationSuccessful)
		{
			Debug.LogWarning("Generation unsuccessful!");

			string contentsPath = AssetDatabase.GetAssetPath(mNewPuzzleContents);
			AssetDatabase.DeleteAsset(contentsPath);
		}
		mNewPuzzleContents = null;

		float endTime = Time.realtimeSinceStartup;
		float timeTaken = endTime - startTime;
		Debug.Log(string.Format("Time taken: {0:n2} seconds", timeTaken));
	}

	private void SetupPositionalLists()
	{
		mWordDirections = new List<EWordDirection>((int)EWordDirection.Count);
		for (int directionIndex = 0, count = (int)EWordDirection.Count; directionIndex < count; ++directionIndex)
		{
			mWordDirections.Add((EWordDirection)directionIndex);
		}

		mGridPositions = new List<GridPosition>(Width * Height);
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				GridPosition position = new GridPosition(x, y);
				mGridPositions.Add(position);
			}
		}
	}

	private bool GenerateInternal()
	{
		bool userCancelled = false;

		// Step 1 - initialise
		mNewPuzzleContents = InitialiseGeneration();

		// Step 2 - Run over the word lists and attempt to places words
		for (int passIndex = 0; passIndex < WordListPasses; ++passIndex)
		{
			bool wasSuccessful = RunWordListPass(passIndex, out userCancelled);
			Debug.Log(string.Format("Pass #{0} placed words: {1}", passIndex + 1, mPlacedWords));

			if (userCancelled)
			{
				return false;
			}

			if (!wasSuccessful)
			{
				break;
			}
		}

		// Step 3 - Check for any gaps to assess the validity
		bool foundGaps = CheckForGaps();
		if (foundGaps)
		{
			return false;
		}

		// Step 4 - Check for partial words
		CheckForPartialWords(out userCancelled);
		if (userCancelled)
		{
			return false;
		}

		// Step 5 - Place any partial words that were found
		PlacePartialWords();

		// Step 6 - Finish
		Debug.Log("Word count: " + mWords.Count);
		mNewPuzzleContents.Finalise(mGrid);

		return true;
	}

	private PuzzleContents InitialiseGeneration()
	{
		ProgressBarHelper.Begin(false, kProgressBarTitle, "Step 1/5: Initialising...");

		string newPath = string.Format("Assets/Resources/Puzzles/New Puzzle_{0}.asset", System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
		PuzzleContents contents = CreateScriptableObjects.CreateNewPuzzleContents(newPath);
		contents.Initialise(Width, Height);

		mGrid = new GridEntry[Width, Height];
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				mGrid[x, y] = new GridEntry(INVALID_CHAR);
			}
		}

		mScoredPlacements = new List<ScoredPlacement>(Width * Height);
		mHasPlacedInitialWord = false;

		mAllWords = WordList.GetAllWords();
		mAllWordsCount = mAllWords.Length;

		mPlacedWords = 0;
		mMaxDimension = Mathf.Max(Width, Height);

		ProgressBarHelper.End();

		return contents;
	}

	private bool RunWordListPass(int passIndex, out bool userCancelled)
	{
		userCancelled = false;

		if (mPlacedWords >= WordLimit)
		{
			return false;
		}

		string progressBarMessageFormat = "Step 2/5: Pass #{0}/{1}. Placed {2:N0}/{3:N0}. Checked {4:N0}/{5:N0}";
		string progressBarMessage = string.Format(progressBarMessageFormat, (passIndex + 1), WordListPasses, 0, 0, 0, 0);
		ProgressBarHelper.Begin(true, kProgressBarTitle, progressBarMessage, 1f / mAllWordsCount);

		int wordsPlacedThisPass = 0;

		mAllWords.Shuffle();
		for (int wordIndex = 0; wordIndex < mAllWordsCount; ++wordIndex)
		{
			if ((wordIndex % kWordListProgressStep) == 0)
			{
				progressBarMessage = string.Format(progressBarMessageFormat,
					(passIndex + 1), WordListPasses,
					mPlacedWords, WordLimit,
					wordIndex, mAllWordsCount);
				bool isStillRunning = ProgressBarHelper.Update(kWordListProgressStep, progressBarMessage);
				if (!isStillRunning)
				{
					userCancelled = true;
					return false;
				}
			}

			string word = mAllWords[wordIndex];
			if (mWords.Contains(word) || word.Length > mMaxDimension)
			{
				continue;
			}

			mWordDirections.Shuffle();
			mGridPositions.Shuffle();

			mScoredPlacements.Clear();

			bool hasFoundPlacement = false;
			foreach (GridPosition position in mGridPositions)
			{
				foreach (EWordDirection wordDirection in mWordDirections)
				{
					int score;
					if (IsWordPlacementValid(word, position, wordDirection, out score))
					{
						mScoredPlacements.Add(new ScoredPlacement(score, position, wordDirection));
					}
				}
			}

			mScoredPlacements.Sort((a, b) => a.Score.CompareTo(b.Score));
			hasFoundPlacement = mScoredPlacements.Count > 0;

			if (!hasFoundPlacement)
			{
				continue;
			}

			if (!mHasPlacedInitialWord)
			{
				mHasPlacedInitialWord = true;

				mScoredPlacements.Clear();

				foreach (GridPosition position in mGridPositions)
				{
					foreach (EWordDirection wordDirection in mWordDirections)
					{
						int score;
						if (IsWordPlacementValid(word, position, wordDirection, out score))
						{
							mScoredPlacements.Add(new ScoredPlacement(score, position, wordDirection));
						}
					}
				}

				mScoredPlacements.Shuffle();
				hasFoundPlacement = true;
			}

			if (hasFoundPlacement)
			{
				ScoredPlacement sp = mScoredPlacements.LastItem();
				GridPosition toPosition;
				PlaceWord(word, sp.Position, sp.WordDirection, out toPosition);
				mWords.Add(word);

				mNewPuzzleContents.RegisterWord(word, sp.Position, toPosition);

				mWordPlacements.Add(sp);

				++mPlacedWords;
				++wordsPlacedThisPass;
				if (mPlacedWords >= WordLimit)
				{
					break;
				}
			}
		}

		ProgressBarHelper.End();

		if (wordsPlacedThisPass == 0)
		{
			return false;
		}

		return true;
	}

	private bool CheckForGaps()
	{
		ProgressBarHelper.Begin(false, kProgressBarTitle, "Step 3/5: Assessing puzzle validity");
		bool foundGap = false;

		// Did any tiles get missed?
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				if (mGrid[x, y].Character == INVALID_CHAR)
				{
					foundGap = true;
				}
			}
		}

		ProgressBarHelper.End();

		return foundGap;
	}

	private void CheckForPartialWords(out bool userCancelled)
	{
		userCancelled = false;

		string progressBarMessageFormat = "Step 3/5: Checking for partial words {0:N0}/{1:N0}";
		string progressBarMessage = string.Format(progressBarMessageFormat, 0, 0);
		ProgressBarHelper.Begin(true, kProgressBarTitle, progressBarMessage, 1f / mAllWordsCount);

		int extras = 0;
		List<int> originalWordIndices = new List<int>();
		List<string> partialWords = new List<string>();
		for (int wordIndex = 0; wordIndex < mAllWordsCount; ++wordIndex)
		{
			if ((wordIndex % kWordListProgressStep) == 0)
			{
				bool isStillRunning = ProgressBarHelper.Update(kWordListProgressStep, string.Format(progressBarMessageFormat, wordIndex, mAllWordsCount));
				if (!isStillRunning)
				{
					userCancelled = true;
					return;
				}
			}

			string word = mAllWords[wordIndex];
			if (mWords.Contains(word))
			{
				continue;
			}

			for (int usedWordIndex = 0; usedWordIndex < mWords.Count; ++usedWordIndex)
			{
				if (mWords[usedWordIndex].Contains(word))
				{
					originalWordIndices.Add(usedWordIndex);
					partialWords.Add(word);
					++extras;
					break;
				}
			}
		}

		ProgressBarHelper.End();
	}

	private void PlacePartialWords()
	{
		// TODO - fix and add a progress bar
		/*for (int i = 0; i < originalWordIndices.Count; ++i)
		{
			int originalIndex = originalWordIndices[i];
			string str = mWords[originalIndex];
			string partialWord = partialWords[i];

			ScoredPlacement wordPlacement = mWordPlacements[originalIndex];

			int score = 0;
			if (IsWordPlacementValid(partialWord, wordPlacement.Position, wordPlacement.WordDirection, out score))
			{
				mWords.Add(partialWord);

				GridPosition fromPosition;
				GridPosition toPosition;
				PlacePartialWord(str, partialWord, wordPlacement.Position, wordPlacement.WordDirection, out fromPosition, out toPosition);

				contents.RegisterWord(partialWord, wordPlacement.Position, toPosition);
			}
		}*/
	}

	private bool IsWordPlacementValid(string word, GridPosition position, EWordDirection wordDirection, out int score)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		bool isPlacementValid = true;
		score = 0;

		GridPosition pos = new GridPosition(position.X, position.Y);
		int wordLength = word.Length;
		char character;
		char gridCharacter;
		for (int characterIndex = 0; characterIndex < wordLength; ++characterIndex)
		{
			character = word[characterIndex];
			gridCharacter = mGrid[pos.X, pos.Y].Character;
			if ((gridCharacter != INVALID_CHAR && character != gridCharacter) || mGrid[pos.X, pos.Y].NumberOfUses >= MaxTileUsage)
			{
				isPlacementValid = false;
				break;
			}

			pos.X += xModifier;
			pos.Y += yModifier;

			if (!GridHelper.IsGridPositionValid(pos, Width, Height))
			{
				isPlacementValid = false;
				break;
			}

			if (character == gridCharacter)
			{
				++score;
			}
		}

		return isPlacementValid;
	}

	private void PlaceWord(string word, GridPosition position, EWordDirection wordDirection, out GridPosition toPosition)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		toPosition = new GridPosition(position.X, position.Y);
		GridEntry entry;
		int wordLength = word.Length;
		for (int characterIndex = 0; characterIndex < wordLength; ++characterIndex)
		{
			entry = mGrid[toPosition.X, toPosition.Y];
			entry.Character = word[characterIndex];
			++entry.NumberOfUses;

			toPosition.X += xModifier;
			toPosition.Y += yModifier;
		}
	}

	private void PlacePartialWord(string word, string partialWord, GridPosition position, EWordDirection wordDirection, out GridPosition fromPosition, out GridPosition toPosition)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		fromPosition = new GridPosition(position.X, position.Y);

		int startIndex = word.IndexOf(partialWord, 0);
		for (int i = 0; i < startIndex; ++i)
		{
			fromPosition.X += xModifier;
			fromPosition.Y += yModifier;
		}

		GridPosition pos = fromPosition;
		toPosition = fromPosition;

		int partialWordLength = partialWord.Length;
		for (int characterIndex = 0; characterIndex < partialWordLength; ++characterIndex)
		{
			mGrid[pos.X, pos.Y].Character = partialWord[characterIndex];
			toPosition.X += xModifier;
			toPosition.Y += yModifier;
		}
	}
}
 
 