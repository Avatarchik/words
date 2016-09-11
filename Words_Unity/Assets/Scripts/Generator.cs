using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

// http://uigradients.com/

/*
 * TODO - Make the generator run in the editor
 * TODO - Add a progress bar to the generation
 */

[Serializable]
public class GridPosition
{
	public int X;
	public int Y;

	public GridPosition(int x, int y)
	{
		X = x;
		Y = y;
	}

	public GridPosition(GridPosition position)
	{
		X = position.X;
		Y = position.Y;
	}

	static public bool operator ==(GridPosition lhs, GridPosition rhs)
	{
		bool areEqual = lhs.X == rhs.X && lhs.Y == rhs.Y;
		return areEqual;
	}

	static public bool operator !=(GridPosition lhs, GridPosition rhs)
	{
		return !(lhs == rhs);
	}
}

[Serializable]
public class GridEntry
{
	public GridPosition Position;

	private int _CharacterCount;
	public int CharacterCount
	{
		get
		{
			return _CharacterCount;
		}
		set
		{
			_CharacterCount = value;

			if (_CharacterCount > 0)
			{
				if (!Generator.Instance.IsRunning && BackgroundComp)
				{
					SetBackgroundColour(Generator.Instance.Scheme.High, Generator.Instance.Scheme.Low, Generator.Instance.CurrentMaxCharacterUsage);
				}
			}
			else
			{
				GameObject.Destroy(_PrefabInstance);
				Generator.Instance.RemoveEntry(this);
			}
		}
	}

	private char _Character;
	public char Character
	{
		get
		{
			return _Character;
		}
		set
		{
			CharacterCount = (_Character == value) ? (CharacterCount + 1) : 1;

			_Character = value;
			if (PrefabInstance)
			{
				PrefabInstance.name = string.Format("[{0}, {1}] = {2}", Position.X, Position.Y, _Character);

				if (TextComp)
				{
					TextComp.text = _Character.ToString();
				}
			}
		}
	}

	private GameObject _PrefabInstance;
	public GameObject PrefabInstance
	{
		get
		{
			return _PrefabInstance;
		}
		set
		{
			_PrefabInstance = value;
			TextComp = _PrefabInstance ? _PrefabInstance.GetComponentInChildren<Text>() : null;
			ImageComp = _PrefabInstance ? _PrefabInstance.GetComponentInChildren<Image>() : null;
			PositionReferenceComp = _PrefabInstance ? _PrefabInstance.GetComponent<GridPositionReference>() : null;
			BackgroundComp = _PrefabInstance ? _PrefabInstance.GetComponentInChildren<CharacterBackground>() : null;
		}
	}

	public void SetPosition(GridPosition position)
	{
		if (PositionReferenceComp)
		{
			PositionReferenceComp.Position = position;
		}
	}

	public void SetBackgroundColour(Color fromColour, Color toColour, int maxCharacterCount)
	{
		if (ImageComp)
		{
			float t = (float)CharacterCount / maxCharacterCount;
			t = MathfHelper.Clamp0(t);
			ImageComp.color = ColorHelper.Blend(fromColour, toColour, t);
		}
	}

	public void AddTint(Color highlightColour)
	{
		if (BackgroundComp)
		{
			BackgroundComp.AddTint(highlightColour);
		}
	}

	public void RemoveTint()
	{
		if (BackgroundComp)
		{
			BackgroundComp.RemoveTint();
		}
	}

	private Text TextComp;
	private Image ImageComp;
	private GridPositionReference PositionReferenceComp;
	private CharacterBackground BackgroundComp;
}

public class ScoredPlacement
{
	public int Score;
	public GridPosition Position;
	public Generator.EWordDirection WordDirection;

	public ScoredPlacement(int score, GridPosition position, Generator.EWordDirection wordDirection)
	{
		Score = score;
		Position = position;
		WordDirection = wordDirection;
	}
}

public class Generator : MonoBehaviour
{
	public enum EWordDirection : byte
	{
		North = 0,
		NorthEast,
		East,
		SouthEast,
		South,
		SouthWest,
		West,
		NorthWest,

		Count,
	}

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
	[Range(1, 100)]
	public int MaxCharacterUsage = 10;
	[Range(1, 400)]
	public int WordLimit = 100;

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
					entry.SetBackgroundColour(_Scheme.High, _Scheme.Low, CurrentMaxCharacterUsage);
				}
			}
		}
	}

	[HideInInspector]
	public int CurrentMaxCharacterUsage;

	public WordPanel WordPanelRef;
	private List<string> mWords = new List<string>();

	[HideInInspector]
	public bool IsRunning;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

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

		mWords.Clear();
		GenerateInternal();

		CurrentMaxCharacterUsage = 0;
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				GridEntry entry = mGrid[x, y];
				if (entry.Character != INVALID_CHAR && entry.CharacterCount > 1)
				{
					CurrentMaxCharacterUsage = Mathf.Max(CurrentMaxCharacterUsage, entry.CharacterCount);
				}
			}
		}

		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				GridEntry entry = mGrid[x, y];
				entry.SetPosition(new GridPosition(x, y));
				entry.SetBackgroundColour(Scheme.High, Scheme.Low, CurrentMaxCharacterUsage);
			}
		}

		IsRunning = false;
	}

	private bool GenerateInternal()
	{
		float startTime = Time.realtimeSinceStartup;

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
					PlaceWord(word, sp.Position, sp.WordDirection);
					mWords.Add(word);

					++placedWords;
					if (placedWords >= WordLimit)
					{
						break;
					}
				}
			}

			Debug.Log(string.Format("#{0} Placed words: {1}", i + 1, placedWords));
		}

		// Plug the gaps
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				if (mGrid[x, y].Character == INVALID_CHAR)
				{
					mGrid[x, y].Character = GetRandomLetter();
				}
			}
		}

		float endTime = Time.realtimeSinceStartup;
		float timeTaken = endTime - startTime;
		Debug.Log(string.Format("Time taken: {0:n2} seconds", timeTaken));

		mWords.Sort();
		WordPanelRef.Initialise(mWords);

		return true;
	}

	private char GetRandomLetter()
	{
		const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		return letters[UnityEngine.Random.Range(0, letters.Length)];
	}

	private void Cleanup()
	{
		foreach (Transform child in transform)
		{
			Destroy(child.gameObject);
		}
	}

	public void RemoveEntry(GridEntry entry)
	{
		mGrid[entry.Position.X, entry.Position.Y] = null;
	}

	private bool IsWordPlacementValid(string word, GridPosition position, EWordDirection wordDirection, out int score)
	{
		int xModifier;
		int yModifier;
		GetDirectionModifier(wordDirection, out xModifier, out yModifier);

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
			if (gridCharacter != INVALID_CHAR && character != gridCharacter && mGrid[pos.X, pos.Y].CharacterCount < MaxCharacterUsage)
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

	private void PlaceWord(string word, GridPosition position, EWordDirection wordDirection)
	{
		int xModifier;
		int yModifier;
		GetDirectionModifier(wordDirection, out xModifier, out yModifier);

		GridPosition pos = new GridPosition(position.X, position.Y);
		int wordLength = word.Length;
		for (int characterIndex = 0; characterIndex < wordLength; ++characterIndex)
		{
			mGrid[pos.X, pos.Y].Character = word[characterIndex];
			pos.X += xModifier;
			pos.Y += yModifier;
		}
	}

	private bool IsGridPositionValid(GridPosition position)
	{
		bool isValid = position.X >= 0 && position.Y >= 0 && position.X < Width && position.Y < Height;
		return isValid;
	}

	private void GetDirectionModifier(EWordDirection wordDirection, out int xModifier, out int yModifier)
	{
		switch (wordDirection)
		{
			case EWordDirection.North:
				xModifier = 0;
				yModifier = 1;
				break;

			case EWordDirection.NorthEast:
				xModifier = 1;
				yModifier = 1;
				break;

			case EWordDirection.East:
				xModifier = 1;
				yModifier = 0;
				break;

			case EWordDirection.SouthEast:
				xModifier = 1;
				yModifier = -1;
				break;

			case EWordDirection.South:
				xModifier = 0;
				yModifier = -1;
				break;

			case EWordDirection.SouthWest:
				xModifier = -1;
				yModifier = -1;
				break;

			case EWordDirection.West:
				xModifier = -1;
				yModifier = 0;
				break;

			case EWordDirection.NorthWest:
				xModifier = -1;
				yModifier = 1;
				break;

			default:
				xModifier = 0;
				yModifier = 0;
				Debug.LogError("Invalid word direction");
				break;
		}
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