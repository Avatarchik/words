using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PuzzleGenerator : EditorWindow
{
	private const string kProgressBarTitle = "Puzzle Generation";
	private const int kWordListProgressBarStep = 1000;

	private int kWidthMin = 4;
	private int kWidthMax = 16;
	private int kHeightMin = 4;
	private int kHeightMax = 16;
	private int kWordListPassesMin = 1;
	private int kWordListPassesMax = 3;
	private int kWordLimitMin = 1;
	private int kWordLimitMax = 1024;
	private int kMaxTileUsageMin = 1;
	private int kMaxTileUsageMax = 17;

	[Range(4, 16)]
	public int Width = 7;
	[Range(4, 16)]
	public int Height = 7;
	[Range(1, 3)]
	public int WordListPasses = 1;
	[Range(1, 1024)]
	public int WordLimit = 100;
	[Range(1, 17)]
	public int MaxTileUsage = 5;

	private char INVALID_CHAR = ' ';
	private GridEntry[,] mGrid;

	private Words WordList;

	private List<PotentialWordPlacement> mPotentialPlacements;

	private List<string> mWords;
	private List<ScoredWordPlacement> mWordPlacements;

	private List<ScoredWordPlacement> mScoredPlacements;
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

		Width = EditorGUILayout.IntSlider("Width", Width, kWidthMin, kWidthMax);
		Height = EditorGUILayout.IntSlider("Height", Height, kHeightMin, kHeightMax);
		WordListPasses = EditorGUILayout.IntSlider("Word List Passes", WordListPasses, kWordListPassesMin, kWordListPassesMax);
		WordLimit = EditorGUILayout.IntSlider("WordLimit", WordLimit, kWordLimitMin, kWordLimitMax);
		MaxTileUsage = EditorGUILayout.IntSlider("MaxTileUsage", MaxTileUsage, kMaxTileUsageMin, kMaxTileUsageMax);

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
		mWordPlacements = new List<ScoredWordPlacement>(WordLimit);

		mScoredPlacements = new List<ScoredWordPlacement>(WordLimit);
		mHasPlacedInitialWord = false;

		mPlacedWords = 0;

		mNewPuzzleContents = null;
	}

	public void Generate()
	{
		float startTime = Time.realtimeSinceStartup;

		Initialise();
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
		mPotentialPlacements = new List<PotentialWordPlacement>(Width * Height * (int)EWordDirection.Count);
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				for (int directionIndex = 0, count = (int)EWordDirection.Count; directionIndex < count; ++directionIndex)
				{
					int xModifier;
					int yModifier;
					WordDirection.GetModifiers((EWordDirection)directionIndex, out xModifier, out yModifier);

					if (IsPlacementWithinBounds(x, y, xModifier, yModifier, 3))
					{
						mPotentialPlacements.Add(new PotentialWordPlacement(new GridPosition(x, y), (EWordDirection)directionIndex));
					}
				}
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

		// Step 5 - Check for naturally placed words
		CheckForNaturallyPlacedWords(out userCancelled);
		if (userCancelled)
		{
			return false;
		}

		// Finished
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

		mScoredPlacements = new List<ScoredWordPlacement>(Width * Height);
		mHasPlacedInitialWord = false;

		mPlacedWords = 0;
		mMaxDimension = Mathf.Max(Width, Height);

		mAllWords = WordList.GetAllWords(mMaxDimension);
		mAllWordsCount = mAllWords.Length;

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

		string progressBarMessageFormat = "Step 2/5: Pass #{0}/{1}. Placed {2:N0}/{3:N0} ({4:N1}%). Words {5:N0}/{6:N0}";
		string progressBarMessage = string.Format(progressBarMessageFormat, (passIndex + 1), WordListPasses, 0, 0, 0, 0, 0);
		ProgressBarHelper.Begin(true, kProgressBarTitle, progressBarMessage, 1f / mAllWordsCount);

		int wordsPlacedThisPass = 0;

		mAllWords.Shuffle();
		mPotentialPlacements.Shuffle();

		for (int wordIndex = 0; wordIndex < mAllWordsCount; ++wordIndex)
		{
			if ((wordIndex % kWordListProgressBarStep) == 0)
			{
				progressBarMessage = string.Format(progressBarMessageFormat,
					(passIndex + 1), WordListPasses,
					mPlacedWords, WordLimit, ((float)mPlacedWords / WordLimit) * 100,
					wordIndex, mAllWordsCount);
				bool isStillRunning = ProgressBarHelper.Update(kWordListProgressBarStep, progressBarMessage);
				if (!isStillRunning)
				{
					userCancelled = true;
					return false;
				}
			}

			string word = mAllWords[wordIndex];
			if (word == null)
			{
				continue;
			}

			mScoredPlacements.Clear();

			int score;
			foreach (PotentialWordPlacement potentialPlacement in mPotentialPlacements)
			{
				if (IsWordPlacementValid(word, potentialPlacement.Position.X, potentialPlacement.Position.Y, potentialPlacement.WordDirection, out score))
				{
					mScoredPlacements.Add(new ScoredWordPlacement(score, potentialPlacement.Position, potentialPlacement.WordDirection));
				}
			}

			mScoredPlacements.Sort((a, b) => a.Score.CompareTo(b.Score));
			bool hasFoundPlacement = mScoredPlacements.Count > 0;
			if (!hasFoundPlacement)
			{
				continue;
			}

			if (!mHasPlacedInitialWord)
			{
				mHasPlacedInitialWord = true;
				mScoredPlacements.Shuffle();
				hasFoundPlacement = true;
			}

			if (hasFoundPlacement)
			{
				ScoredWordPlacement sp = mScoredPlacements.LastItem();
				GridPosition toPosition;
				PlaceWord(word, sp.Position.X, sp.Position.Y, sp.WordDirection, out toPosition);

				mNewPuzzleContents.RegisterWord(word, sp.Position, toPosition);

				mWords.Add(word);
				mWordPlacements.Add(sp);

				mAllWords[wordIndex] = null;

				++mPlacedWords;
				++wordsPlacedThisPass;
				if (mPlacedWords >= WordLimit)
				{
					break;
				}
			}
		}

		ProgressBarHelper.End();

		bool wasPassSuccessful = wordsPlacedThisPass > 0;
		return wasPassSuccessful;
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

		string progressBarMessageFormat = "Step 4/5: Partial words {0:N0}. Words {1:N0}/{2:N0}";
		string progressBarMessage = string.Format(progressBarMessageFormat, 0, 0, 0);
		ProgressBarHelper.Begin(true, kProgressBarTitle, progressBarMessage, 1f / mAllWordsCount);

		int partialWordsPlaced = 0;
		for (int wordIndex = 0; wordIndex < mAllWordsCount; ++wordIndex)
		{
			if ((wordIndex % kWordListProgressBarStep) == 0)
			{
				progressBarMessage = string.Format(progressBarMessageFormat, partialWordsPlaced, wordIndex, mAllWordsCount);
				bool isStillRunning = ProgressBarHelper.Update(kWordListProgressBarStep, progressBarMessage);
				if (!isStillRunning)
				{
					userCancelled = true;
					return;
				}
			}

			string word = mAllWords[wordIndex];
			if (word == null)
			{
				continue;
			}

			string alreadyPlacedWord;
			for (int usedWordIndex = 0; usedWordIndex < mWords.Count; ++usedWordIndex)
			{
				alreadyPlacedWord = mWords[usedWordIndex];

				if (alreadyPlacedWord.Contains(word))
				{
					ScoredWordPlacement alreadyPlacedPlacement = mWordPlacements[usedWordIndex];

					GridPosition fromPosition;
					GridPosition toPosition;
					bool wasWordPlaced = PlacePartialWord(alreadyPlacedWord, word,
						alreadyPlacedPlacement.Position.X, alreadyPlacedPlacement.Position.Y,
						alreadyPlacedPlacement.WordDirection, out fromPosition, out toPosition);

					if (wasWordPlaced)
					{
						mNewPuzzleContents.RegisterWord(word, fromPosition, toPosition);

						mWords.Add(word);
						mWordPlacements.Add(alreadyPlacedPlacement);

						mAllWords[wordIndex] = null;

						++partialWordsPlaced;

						break;
					}
				}
			}
		}

		Debug.Log("Partial words found: " + partialWordsPlaced);

		ProgressBarHelper.End();
	}

	private void CheckForNaturallyPlacedWords(out bool userCancelled)
	{
		userCancelled = false;

		string progressBarMessageFormat = "Step 5/5: Natural words {0:N0}. Words {1:N0}/{2:N0}";
		string progressBarMessage = string.Format(progressBarMessageFormat, 0, 0, 0);
		ProgressBarHelper.Begin(true, kProgressBarTitle, progressBarMessage, 1f / mAllWordsCount);

		string word;
		bool foundOccurrence = false;
		GridPosition foundPosition = null;
		GridPosition foundPositionEnd = null;
		EWordDirection foundWordDirection = EWordDirection.Unknown;

		int naturallyPlacedWordsFound = 0;
		for (int wordIndex = 0; wordIndex < mAllWordsCount; ++wordIndex)
		{
			if ((wordIndex % kWordListProgressBarStep) == 0)
			{
				progressBarMessage = string.Format(progressBarMessageFormat, naturallyPlacedWordsFound, wordIndex, mAllWordsCount);
				bool isStillRunning = ProgressBarHelper.Update(kWordListProgressBarStep, progressBarMessage);
				if (!isStillRunning)
				{
					userCancelled = true;
					return;
				}
			}

			word = mAllWords[wordIndex];
			if (word == null)
			{
				continue;
			}

			foundOccurrence = false;

			foreach (PotentialWordPlacement potentialPlacement in mPotentialPlacements)
			{
				if (DoesPlacementContainWord(word, potentialPlacement.Position.X, potentialPlacement.Position.Y, potentialPlacement.WordDirection, out foundPositionEnd))
				{
					foundPosition = potentialPlacement.Position;
					foundWordDirection = potentialPlacement.WordDirection;

					foundOccurrence = true;
					break;
				}
			}

			if (foundOccurrence)
			{
				if (IsWordPlacementValid(word, foundPosition.X, foundPosition.Y, foundWordDirection))
				{
					mNewPuzzleContents.RegisterWord(word, foundPosition, foundPositionEnd);
					mWords.Add(word);
					mWordPlacements.Add(new ScoredWordPlacement(0, foundPosition, foundWordDirection));

					mAllWords[wordIndex] = null;

					++naturallyPlacedWordsFound;

					if (mWords.Count >= WordLimit)
					{
						break;
					}
				}
			}
		}

		Debug.Log("Naturally placed words: " + naturallyPlacedWordsFound);

		ProgressBarHelper.End();
	}

	private bool DoesPlacementContainWord(string word, int x, int y, EWordDirection wordDirection, out GridPosition foundPositionEnd)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		bool doesContainWord = true;
		int wordLength = word.Length;

		if (IsPlacementWithinBounds(x, y, xModifier, yModifier, wordLength))
		{
			char character;
			char gridCharacter;

			for (int characterIndex = 0; characterIndex < wordLength; ++characterIndex)
			{
				character = word[characterIndex];
				gridCharacter = mGrid[x, y].Character;

				if (character != gridCharacter)
				{
					doesContainWord = false;
					break;
				}

				x += xModifier;
				y += yModifier;
			}
		}
		else
		{
			doesContainWord = false;
		}

		foundPositionEnd = new GridPosition(x, y);

		return doesContainWord;
	}

	private bool IsWordPlacementValid(string word, int x, int y, EWordDirection wordDirection)
	{
		int score;
		return IsWordPlacementValid(word, x, y, wordDirection, out score);
	}

	private bool IsWordPlacementValid(string word, int x, int y, EWordDirection wordDirection, out int score)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		bool isPlacementValid = true;
		int wordLength = word.Length;
		score = 0;

		if (IsPlacementWithinBounds(x, y, xModifier, yModifier, wordLength))
		{
			char character;
			char gridCharacter;
			for (int characterIndex = 0; characterIndex < wordLength; ++characterIndex)
			{
				character = word[characterIndex];
				gridCharacter = mGrid[x, y].Character;
				if ((gridCharacter != INVALID_CHAR && character != gridCharacter) || mGrid[x, y].NumberOfUses >= MaxTileUsage)
				{
					isPlacementValid = false;
					break;
				}

				x += xModifier;
				y += yModifier;

				if (character == gridCharacter)
				{
					++score;
				}
			}
		}
		else
		{
			isPlacementValid = false;
		}

		return isPlacementValid;
	}

	private bool IsPlacementWithinBounds(int x, int y, int xModifier, int yModifier, int wordLength)
	{
		x = x + (xModifier * wordLength);
		y = y + (yModifier * wordLength);
		return GridHelper.IsGridPositionValid(x, y, Width, Height);
	}

	private void PlaceWord(string word, int x, int y, EWordDirection wordDirection, out GridPosition toPosition)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		GridEntry entry;
		int wordLength = word.Length;
		for (int characterIndex = 0; characterIndex < wordLength; ++characterIndex)
		{
			entry = mGrid[x, y];
			entry.Character = word[characterIndex];
			++entry.NumberOfUses;

			x += xModifier;
			y += yModifier;
		}

		toPosition = new GridPosition(x - xModifier, y - yModifier);
	}

	private bool PlacePartialWord(string word, string partialWord, int x, int y, EWordDirection wordDirection, out GridPosition fromPosition, out GridPosition toPosition)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		int startIndex = word.IndexOf(partialWord, 0);
		x += xModifier * startIndex;
		y += yModifier * startIndex;
		fromPosition = new GridPosition(x, y);

		if (!IsWordPlacementValid(partialWord, x, y, wordDirection))
		{
			toPosition = fromPosition;
			return false;
		}

		GridEntry entry;
		int partialWordLength = partialWord.Length;
		for (int characterIndex = 0; characterIndex < partialWordLength; ++characterIndex)
		{
			entry = mGrid[x, y];
			entry.Character = partialWord[characterIndex];
			++entry.NumberOfUses;

			x += xModifier;
			y += yModifier;
		}

		toPosition = new GridPosition(x - xModifier, y - yModifier);

		return true;
	}
}