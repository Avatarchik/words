using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;

public class PuzzleGenerator : EditorWindow
{
	private const string kProgressBarTitle = "Puzzle Generation";
	private const int kWordListProgressBarStep = 1000;

	private int mSize = 7;
	private int mWordLimit = 100;
	private int mMaxTileUsage = 5;

	private int mTestLevelsPerSize = 5;

	private int mPotDsToGenerate = 30;
	[Range(1, 31)]
	public int mPotDStartDateDay = 1;
	[Range(1, 12)]
	public int mPotDStartDateMonth = 1;
	[Range(2016, 2020)]
	public int mPotDStartDateYear = 2016;

	private char INVALID_CHAR = ' ';
	private CharacterUsage[,] mGrid;

	private Words WordList;

	private List<PotentialWordPlacement> mPotentialPlacements;

	private List<string> mWords;
	private List<ScoredWordPlacement> mWordPlacements;

	private List<ScoredWordPlacement> mScoredPlacements;
	private bool mHasPlacedInitialWord;

	private string[] mAllWords;
	private int mAllWordsCount;

	private PuzzleContents mNewPuzzleContents;
	private string mContentsPath;

	private WordDefinitions mDefinitions;

	[MenuItem("Words/Puzzle/Open Generator")]
	static public void ShowWindow()
	{
		PuzzleGenerator window = GetWindow(typeof(PuzzleGenerator)) as PuzzleGenerator;
		window.Initialise();
	}

	void OnGUI()
	{
		// Normal generation
		GUILayout.Label("Settings", EditorStyles.boldLabel);

		GUILayout.Space(8);
		mSize = EditorGUILayout.IntSlider("Size", mSize, GlobalSettings.Instance.PuzzleSizeMin, GlobalSettings.Instance.PuzzleSizeMax);
		mWordLimit = EditorGUILayout.IntSlider("Word Limit", mWordLimit, 1, 1024);
		mMaxTileUsage = EditorGUILayout.IntSlider("Max Tile Usage", mMaxTileUsage, 1, GlobalSettings.Instance.PuzzleSizeMaxTileUsage);

		GUILayout.Space(8);
		if (GUILayout.Button("Generate"))
		{
			Generate("Assets/Resources/Puzzles");
		}

		// Test level generation
		GUILayout.Space(8);
		GUILayout.Label("Test Levels", EditorStyles.boldLabel);

		GUILayout.Space(8);
		mTestLevelsPerSize = EditorGUILayout.IntSlider("Levels Per Size", mTestLevelsPerSize, 1, 8);

		GUILayout.Space(8);
		if (GUILayout.Button("Generate Test Puzzles"))
		{
			GenerateTestLevels();
		}

		// Puzzle of the Day generation
		GUILayout.Space(8);
		GUILayout.Label("Puzzle of the Day Levels", EditorStyles.boldLabel);

		GUILayout.Space(8);
		mPotDsToGenerate = EditorGUILayout.IntSlider("Puzzles", mPotDsToGenerate, 1, 365);
		mPotDStartDateDay = EditorGUILayout.IntSlider("Start Date Day", mPotDStartDateDay, 1, 31);
		mPotDStartDateMonth = EditorGUILayout.IntSlider("Start Date Month", mPotDStartDateMonth, 1, 12);
		mPotDStartDateYear = EditorGUILayout.IntSlider("Start Date Year", mPotDStartDateYear, 2016, 2020);

		GUILayout.Space(8);
		if (GUILayout.Button("Generate PotD Puzzles"))
		{
			GeneratePuzzleOfTheDays();
		}
	}

	private void Initialise()
	{
		GameObject wordListsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Words.prefab");
		if (wordListsPrefab)
		{
			WordList = wordListsPrefab.GetComponent<Words>();
		}

		mWords = new List<string>(mWordLimit);
		mWordPlacements = new List<ScoredWordPlacement>(mWordLimit);

		mScoredPlacements = new List<ScoredWordPlacement>(mWordLimit);
		mHasPlacedInitialWord = false;

		mNewPuzzleContents = null;

		GameObject wordDefinitionsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WordDefinitions.prefab");
		if (wordDefinitionsPrefab)
		{
			mDefinitions = wordDefinitionsPrefab.GetComponent<WordDefinitions>();
		}
	}

	private bool Generate(string pathRoot, bool isPotD = false, string potdDate = "")
	{
		float startTime = Time.realtimeSinceStartup;

		Initialise();
		SetupPositionalLists();

		bool wasGenerationSuccessful = GenerateInternal(pathRoot, isPotD, potdDate);
		if (!wasGenerationSuccessful)
		{
			ODebug.LogWarning("Generation unsuccessful!");

			string contentsPath = AssetDatabase.GetAssetPath(mNewPuzzleContents);
			AssetDatabase.DeleteAsset(contentsPath);
		}
		mNewPuzzleContents = null;

		float endTime = Time.realtimeSinceStartup;
		float timeTaken = endTime - startTime;
		ODebug.Log(string.Format("Time taken: {0:n2} seconds", timeTaken));

		mDefinitions = null;

		return wasGenerationSuccessful;
	}

	private void GenerateTestLevels()
	{
		int originalSize = mSize;

		for (int i = GlobalSettings.Instance.PuzzleSizeMin; i < (GlobalSettings.Instance.PuzzleSizeMax + 1); ++i)
		{
			for (int j = 0; j < mTestLevelsPerSize; ++j)
			{
				mSize = i;

				bool wasSuccessful = Generate("Assets/Resources/Puzzles");
				if (!wasSuccessful)
				{
					--j;
				}
			}
		}

		mSize = originalSize;
	}

	private void GeneratePuzzleOfTheDays()
	{
		string pathRoot = "Assets/PuzzleOfTheDay/";
		mWordLimit = 1024;
		mMaxTileUsage = GlobalSettings.Instance.PuzzleSizeMaxTileUsage;

		DateTime startDate = new DateTime(mPotDStartDateYear, mPotDStartDateMonth, mPotDStartDateDay);

		for (int puzzleCount = 0; puzzleCount < mPotDsToGenerate; ++puzzleCount)
		{
			mSize = UnityEngine.Random.Range(GlobalSettings.Instance.PuzzleSizeMin, GlobalSettings.Instance.PuzzleSizeMax);

			DateTime date = startDate;
			date = date.AddDays(puzzleCount);

			bool wasSuccessful = Generate(pathRoot, true, date.ToString("yyyy_MM_dd"));
			if (!wasSuccessful)
			{
				puzzleCount = MathfHelper.Clamp0(puzzleCount - 1);
			}
		}

		ODebug.Log("Puzzle of the Days generated");
	}

	private void SetupPositionalLists()
	{
		mPotentialPlacements = new List<PotentialWordPlacement>(mSize * mSize * (int)EWordDirection.Count);
		for (int x = 0; x < mSize; ++x)
		{
			for (int y = 0; y < mSize; ++y)
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

	private bool GenerateInternal(string pathRoot, bool isPotD, string potdDate)
	{
		bool userCancelled = false;

		// Step 1 - initialise
		mNewPuzzleContents = InitialiseGeneration(pathRoot);

		// Step 2 - Run over the word list and attempt to places words
		RunWordListPass(out userCancelled);

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
		ODebug.Log("Word count: " + mWords.Count);
		bool wasFinalised = FinaliseGeneration(isPotD, potdDate);

		return wasFinalised;
	}

	private PuzzleContents InitialiseGeneration(string pathRoot)
	{
		ProgressBarHelper.Begin(false, kProgressBarTitle, "Step 1/5: Initialising...");

		mContentsPath = string.Format("{0}{1}.asset", pathRoot, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
		PuzzleContents contents = CreateScriptableObjects.CreateNewPuzzleContents(mContentsPath);
		contents.Initialise(mSize);

		mGrid = new CharacterUsage[mSize, mSize];
		for (int x = 0; x < mSize; ++x)
		{
			for (int y = 0; y < mSize; ++y)
			{
				mGrid[x, y] = new CharacterUsage(INVALID_CHAR);
			}
		}

		mScoredPlacements = new List<ScoredWordPlacement>(mSize * mSize);
		mHasPlacedInitialWord = false;

		mAllWords = WordList.GetAllWords(mSize);
		mAllWordsCount = mAllWords.Length;

		ProgressBarHelper.End();

		return contents;
	}

	private bool FinaliseGeneration(bool isPotD, string potdDate)
	{
		bool wasFinalised = mNewPuzzleContents.Finalise(mGrid);

		string searchDir = PathHelper.Combine(Application.dataPath, string.Format("Resources/Puzzles/Size {0}", mSize));
		string[] puzzlePaths = null;
		if (Directory.Exists(searchDir))
		{
			puzzlePaths = Directory.GetFiles(searchDir, "*.asset");
		}

		int nextID = 0;
		if (puzzlePaths != null && puzzlePaths.Length > 0)
		{
			string lastPuzzle = puzzlePaths[puzzlePaths.Length - 1];
			string[] pathSplit = Path.GetFileName(lastPuzzle).Split('_');
			nextID = int.Parse(pathSplit[1]);
			++nextID;
		}

		string newContentsFileName;
		if (isPotD)
		{
			newContentsFileName = potdDate;
			mNewPuzzleContents.Guid = GlobalSettings.Instance.PotDFixedGuid;
		}
		else
		{
			newContentsFileName = string.Format("{0:D2}_{1:D4}_{2:D4}", mSize, nextID, mWords.Count);
		}

		if (!wasFinalised)
		{
			newContentsFileName += "_Failed";
		}
		AssetDatabase.RenameAsset(mContentsPath, newContentsFileName);

		string currentPath = AssetDatabase.GetAssetPath(mNewPuzzleContents);
		string newPath = currentPath.Replace("/Puzzles/", string.Format("/Puzzles/Size {0}/", mSize));
		string newPathParent = Path.GetDirectoryName(newPath);
		if (!Directory.Exists(newPathParent))
		{
			Directory.CreateDirectory(newPathParent);
			AssetDatabase.Refresh();
		}
		AssetDatabase.MoveAsset(currentPath, newPath);
		AssetDatabase.Refresh();

		if (isPotD)
		{
			AssetImporter assetImporter = AssetImporter.GetAtPath(newPath);
			assetImporter.assetBundleName = potdDate;
		}

		return wasFinalised;
	}

	private void RunWordListPass(out bool userCancelled)
	{
		userCancelled = false;

		string progressBarMessageFormat = "Step 2/5: Placed {0:N0}/{1:N0} ({2:N1}%). Words {3:N0}/{4:N0}";
		string progressBarMessage = string.Format(progressBarMessageFormat, 0, 0, 0, 0, 0);
		ProgressBarHelper.Begin(true, kProgressBarTitle, progressBarMessage, 1f / mAllWordsCount);

		mAllWords.Shuffle();
		mPotentialPlacements.Shuffle();

		int placedWords = 0;

		for (int wordIndex = 0; wordIndex < mAllWordsCount; ++wordIndex)
		{
			if ((wordIndex % kWordListProgressBarStep) == 0)
			{
				progressBarMessage = string.Format(progressBarMessageFormat,
					placedWords, mWordLimit, ((float)placedWords / mWordLimit) * 100,
					wordIndex, mAllWordsCount);
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

				string definition = null;
				mDefinitions.GetDefinitionFor(word, ref definition);
				mNewPuzzleContents.RegisterWord(word, definition, sp.Position, toPosition);

				mWords.Add(word);
				mWordPlacements.Add(sp);

				mAllWords[wordIndex] = null;

				++placedWords;
				if (placedWords >= mWordLimit)
				{
					break;
				}
			}
		}

		ProgressBarHelper.End();

		ODebug.Log(string.Format("Placed words: {0}", placedWords));
	}

	private bool CheckForGaps()
	{
		ProgressBarHelper.Begin(false, kProgressBarTitle, "Step 3/5: Assessing puzzle validity");
		bool foundGap = false;

		// Did any tiles get missed?
		for (int x = 0; x < mSize; ++x)
		{
			for (int y = 0; y < mSize; ++y)
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

			if (mAllWords[wordIndex] == null)
			{
				continue;
			}
			string word = mAllWords[wordIndex];

			string alreadyPlacedWord;
			for (int usedWordIndex = 0; usedWordIndex < mWords.Count; ++usedWordIndex)
			{
				alreadyPlacedWord = mWords[usedWordIndex];
				string wordReversed = WordHelper.ReverseWord(word);

				if ((alreadyPlacedWord == word) || (alreadyPlacedWord == wordReversed))
				{
					continue;
				}

				bool containsForwards = alreadyPlacedWord.Contains(word);
				bool containsBackwards = alreadyPlacedWord.Contains(wordReversed);
				if (containsForwards || containsBackwards)
				{
					if (containsBackwards)
					{
						word = wordReversed;
					}

					if (!HasWordBeenPlaced(word))
					{
						ScoredWordPlacement alreadyPlacedPlacement = mWordPlacements[usedWordIndex];

						GridPosition fromPosition;
						GridPosition toPosition;
						bool wasWordPlaced = PlacePartialWord(alreadyPlacedWord, word,
							alreadyPlacedPlacement.Position.X, alreadyPlacedPlacement.Position.Y,
							alreadyPlacedPlacement.WordDirection, out fromPosition, out toPosition);

						if (wasWordPlaced)
						{
							//ODebug.Log(string.Format("Partial word of {0}: {1}", alreadyPlacedWord, word));

							string definition = null;
							mDefinitions.GetDefinitionFor(word, ref definition);
							mNewPuzzleContents.RegisterWord(word, definition, fromPosition, toPosition);

							mWords.Add(word);
							mWordPlacements.Add(alreadyPlacedPlacement);

							mAllWords[wordIndex] = null;

							++partialWordsPlaced;

							break;
						}
					}
				}
			}
		}

		ODebug.Log("Partial words found: " + partialWordsPlaced);

		ProgressBarHelper.End();
	}

	private void CheckForNaturallyPlacedWords(out bool userCancelled)
	{
		userCancelled = false;

		string progressBarMessageFormat = "Step 5/5: Natural words {0:N0}. Words {1:N0}/{2:N0}";
		string progressBarMessage = string.Format(progressBarMessageFormat, 0, 0, 0);
		ProgressBarHelper.Begin(true, kProgressBarTitle, progressBarMessage, 1f / mAllWordsCount);

		string word;
		string wordReversed;
		GridPosition foundPositionEnd = null;

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

			if (mAllWords[wordIndex] == null)
			{
				continue;
			}
			word = mAllWords[wordIndex];
			wordReversed = WordHelper.ReverseWord(word);

			bool hasPlacedWord = false;
			foreach (PotentialWordPlacement potentialPlacement in mPotentialPlacements)
			{
				if (DoesPlacementContainWord(word, potentialPlacement.Position.X, potentialPlacement.Position.Y, potentialPlacement.WordDirection, out foundPositionEnd))
				{
					hasPlacedWord = PlaceNaturalWord(word, word, potentialPlacement.Position, potentialPlacement.WordDirection);
					break;
				}

				if (DoesPlacementContainWord(wordReversed, potentialPlacement.Position.X, potentialPlacement.Position.Y, potentialPlacement.WordDirection, out foundPositionEnd))
				{
					hasPlacedWord = PlaceNaturalWord(wordReversed, word, potentialPlacement.Position, potentialPlacement.WordDirection);
					break;
				}
			}

			if (hasPlacedWord)
			{
				mAllWords[wordIndex] = null;

				++naturallyPlacedWordsFound;

				if (mWords.Count >= mWordLimit)
				{
					break;
				}
			}
		}

		ODebug.Log("Naturally placed words: " + naturallyPlacedWordsFound);

		ProgressBarHelper.End();
	}

	private bool PlaceNaturalWord(string word, string forwardsWord, GridPosition startPosition, EWordDirection direction)
	{
		foreach (string usedWord in mWords)
		{
			if (usedWord == forwardsWord)
			{
				return false;
			}
		}

		if (IsWordPlacementValid(word, startPosition.X, startPosition.Y, direction))
		{
			GridPosition endPosition;
			PlaceWord(word, startPosition.X, startPosition.Y, direction, out endPosition);

			string definition = null;
			mDefinitions.GetDefinitionFor(forwardsWord, ref definition);
			mNewPuzzleContents.RegisterWord(forwardsWord, definition, startPosition, endPosition);

			mWords.Add(forwardsWord);

			return true;
		}

		return false;
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
				if ((gridCharacter != INVALID_CHAR && character != gridCharacter) || mGrid[x, y].NumberOfUses >= mMaxTileUsage)
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
		return GridHelper.IsGridPositionValid(x, y, mSize);
	}

	private void PlaceWord(string word, int x, int y, EWordDirection wordDirection, out GridPosition toPosition)
	{
		int xModifier;
		int yModifier;
		WordDirection.GetModifiers(wordDirection, out xModifier, out yModifier);

		CharacterUsage entry;
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

		CharacterUsage entry;
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

	private bool HasWordBeenPlaced(string word)
	{
		bool result = false;

		for (int wordIndex = 0; wordIndex < mWords.Count; ++wordIndex)
		{
			if (word == mWords[wordIndex])
			{
				result = true;
				break;
			}
		}

		return result;
	}
}