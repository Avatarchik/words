using UnityEngine;
using System.Collections.Generic;

// http://uigradients.com/

/*
 * TODO - Make the generator run in the editor
 * TODO - Add a progress bar to the generation
 */

public class Generator : MonoBehaviour
{
	static public Generator Instance;

	private char INVALID_CHAR = ' ';
	public GridEntry[,] mGrid;

	public GameObject CharacterPrefab;
	public Words WordList;

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

	private List<EWordDirection> mWordDirections;
	private List<GridPosition> mGridPositions;

	private ColourScheme _Scheme;
	public ColourScheme Scheme
	{
		get
		{
			return _Scheme;
		}
		set
		{
			_Scheme = value;

			for (int x = 0; x < Width; ++x)
			{
				for (int y = 0; y < Height; ++y)
				{
					GridEntry entry = mGrid[x, y];
					entry.SetBackgroundColour(_Scheme.High, _Scheme.Low, MaxCharacterUsage);
				}
			}
		}
	}

	[HideInInspector]
	public int MaxCharacterUsage;

	public WordPanel WordPanelRef;
	private List<string> mWords = new List<string>();
	private List<ScoredPlacement> mWordPlacements = new List<ScoredPlacement>();

	[HideInInspector]
	public bool IsRunning;

	private RectTransform mRectTrans;

	public PuzzleContents contents;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		mRectTrans = GetComponent<RectTransform>();

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

	void Start()
	{
		Generate();
	}

	void OnEnable()
	{
		ColourSwitcher.OnColourSwitched += OnColourSwitched;
	}

	void OnDisable()
	{
		ColourSwitcher.OnColourSwitched -= OnColourSwitched;
	}

	public void Generate()
	{
		IsRunning = true;
		float startTime = Time.realtimeSinceStartup;

		bool wasGenerationSuccessful = GenerateInternal();

		if (!wasGenerationSuccessful)
		{
			Debug.LogWarning("Generation unsuccessful!");
			Cleanup();
		}
		else
		{
			MaxCharacterUsage = 0;
			for (int x = 0; x < Width; ++x)
			{
				for (int y = 0; y < Height; ++y)
				{
					GridEntry entry = mGrid[x, y];
					if (entry.Character != INVALID_CHAR && entry.CharacterCount > 1)
					{
						MaxCharacterUsage = Mathf.Max(MaxCharacterUsage, entry.CharacterCount);
					}
				}
			}
			Debug.Log("Max character usage: " + MaxCharacterUsage);

			for (int x = 0; x < Width; ++x)
			{
				for (int y = 0; y < Height; ++y)
				{
					GridEntry entry = mGrid[x, y];
					entry.SetPosition(new GridPosition(x, y));
					entry.SetBackgroundColour(Scheme.High, Scheme.Low, MaxCharacterUsage);
				}
			}

			// Scale accordingly
			mRectTrans.localScale = new Vector3(16f / Width, 16f / Height, 1);
		}

		float endTime = Time.realtimeSinceStartup;
		float timeTaken = endTime - startTime;
		Debug.Log(string.Format("Time taken: {0:n2} seconds", timeTaken));

		IsRunning = false;
	}

	private bool GenerateInternal()
	{
		contents.Initialise(Width, Height);

		// Cleanup
		Cleanup();

		Vector3 gridSize = new Vector3(
			0,
			(Height * 24) + ((Height - 1) * 8),
			0);
		Vector3 halfGridSize = gridSize * 0.5f;
		halfGridSize.x -= 12;
		halfGridSize.y -= 12;

		// Creation
		mGrid = new GridEntry[Width, Height];
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				GridEntry entry = new GridEntry();

				entry.Position = new GridPosition(x, y);
				entry.Character = INVALID_CHAR;

				entry.PrefabInstance = Instantiate(CharacterPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
				entry.PrefabInstance.transform.localPosition = new Vector3(x * 32, y * 32, 0) - halfGridSize;

				mGrid[x, y] = entry;
			}
		}

		List<ScoredPlacement> scoredPlacements = new List<ScoredPlacement>(Width * Height);
		bool hasPlacedInitialWord = false;

		string[] allWords = WordList.GetAllWords();

		int placedWords = 0;

		int maxDimension = Mathf.Max(Width, Height);

		// Puzzling
		for (int i = 0; i < WordListPasses; ++i)
		{
			if (placedWords >= WordLimit)
			{
				break;
			}

			allWords.Shuffle();
			foreach (string word in allWords)
			{
				if (mWords.Contains(word) || word.Length > maxDimension)
				{
					continue;
				}

				mWordDirections.Shuffle();
				mGridPositions.Shuffle();

				scoredPlacements.Clear();

				bool hasFoundPlacement = false;
				foreach (GridPosition position in mGridPositions)
				{
					foreach (EWordDirection wordDirection in mWordDirections)
					{
						int score;
						if (IsWordPlacementValid(word, position, wordDirection, out score))
						{
							scoredPlacements.Add(new ScoredPlacement(score, position, wordDirection));
						}
					}
				}

				scoredPlacements.Sort((a, b) => a.Score.CompareTo(b.Score));
				hasFoundPlacement = scoredPlacements.Count > 0;

				if (!hasFoundPlacement)
				{
					continue;
				}

				if (!hasPlacedInitialWord)
				{
					hasPlacedInitialWord = true;

					scoredPlacements.Clear();

					foreach (GridPosition position in mGridPositions)
					{
						foreach (EWordDirection wordDirection in mWordDirections)
						{
							int score;
							if (IsWordPlacementValid(word, position, wordDirection, out score))
							{
								scoredPlacements.Add(new ScoredPlacement(score, position, wordDirection));
							}
						}
					}

					scoredPlacements.Shuffle();
					hasFoundPlacement = true;
				}

				if (hasFoundPlacement)
				{
					ScoredPlacement sp = scoredPlacements.LastItem();
					GridPosition toPosition;
					PlaceWord(word, sp.Position, sp.WordDirection, out toPosition);
					mWords.Add(word);

					contents.RegisterWord(word, sp.Position, toPosition);

					mWordPlacements.Add(sp);

					++placedWords;
					if (placedWords >= WordLimit)
					{
						break;
					}
				}
			}

			Debug.Log(string.Format("Pass #{0} placed words: {1}", i + 1, placedWords));
		}

		// Did any tiles get missed?
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				if (mGrid[x, y].Character == INVALID_CHAR)
				{
					Debug.LogWarning("found a gap");
					return false;
				}
			}
		}

		// What about partial words?
		int extras = 0;
		List<int> originalWordIndices = new List<int>();
		List<string> partialWords = new List<string>();
		foreach (string word in allWords)
		{
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

		for (int i = 0; i < originalWordIndices.Count; ++i)
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
		}
		Debug.Log("Extras: " + extras);
		Debug.Log("New total: " + mWords.Count);

		mWords.Sort();
		WordPanelRef.Initialise(mWords);

		contents.Finalise(mGrid);

		return true;
	}

	private void Cleanup()
	{
		mRectTrans.localScale = Vector3.one;

		mWords.Clear();
		mWordPlacements.Clear();
	}

	public void RemoveEntry(GridEntry entry)
	{
		mGrid[entry.Position.X, entry.Position.Y] = null;
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
			if ((gridCharacter != INVALID_CHAR && character != gridCharacter) || mGrid[pos.X, pos.Y].CharacterCount >= MaxTileUsage)
			{
				isPlacementValid = false;
				break;
			}

			pos.X += xModifier;
			pos.Y += yModifier;

			if (!IsGridPositionValid(pos))
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
		int wordLength = word.Length;
		for (int characterIndex = 0; characterIndex < wordLength; ++characterIndex)
		{
			mGrid[toPosition.X, toPosition.Y].Character = word[characterIndex];
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

	private bool IsGridPositionValid(GridPosition position)
	{
		bool isValid = position.X >= 0 && position.Y >= 0 && position.X < Width && position.Y < Height;
		return isValid;
	}

	public string GetWord(GridPositionReference fromPosition, GridPositionReference toPosition, ref List<GridEntry> tiles)
	{
		string word = string.Empty;
		tiles.Clear();

		int xDelta = toPosition.Position.X - fromPosition.Position.X;
		int yDelta = toPosition.Position.Y - fromPosition.Position.Y;
		int xModifier = MathfHelper.ClampM11(xDelta);
		int yModifier = MathfHelper.ClampM11(yDelta);

		bool isCardinal = false;
		isCardinal |= xModifier != 0 && yModifier == 0;
		isCardinal |= xModifier == 0 && yModifier != 0;
		isCardinal |= Mathf.Abs(xDelta) == Mathf.Abs(yDelta);

		if (!isCardinal)
		{
			GridEntry entry = mGrid[fromPosition.Position.X, fromPosition.Position.Y];
			word += entry.Character;
			tiles.Add(entry);
			return word;
		}

		int maxDimension = Mathf.Max(Width, Height);

		GridPosition pos = new GridPosition(fromPosition.Position);
		GridPosition endPos = new GridPosition(toPosition.Position);
		endPos.X += xModifier;
		endPos.Y += yModifier;

		int checkedEntries = 0;
		do
		{
			GridEntry entry = mGrid[pos.X, pos.Y];
			if (entry == null)
			{
				break;
			}

			word += entry.Character;
			tiles.Add(entry);

			pos.X += xModifier;
			pos.Y += yModifier;

			++checkedEntries;
		}
		while (IsGridPositionValid(pos) && pos != endPos && checkedEntries < maxDimension);

		return word;
	}

	public void DecrementCharacterCount(List<GridEntry> tiles)
	{
		foreach (GridEntry entry in tiles)
		{
			int count = entry.CharacterCount;
			entry.CharacterCount = count - 1;
		}
	}

	private void OnColourSwitched(ColourScheme newScheme)
	{
		Scheme = newScheme;
	}
}