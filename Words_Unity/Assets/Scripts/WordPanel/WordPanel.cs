using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EWordValidityResult
{
	NoChange = 0,

	Match,
	WrongInstance,
	NoMatch,

	WasRemoved,
	WasAlreadyFound,
}

public class WordValidityResult
{
	public int WordsFound;
	public int WordsAlreadyFound;
	public bool IsWrongInstance;
	public int TileDecrements;

	public List<string> Words = new List<string>();

	public WordValidityResult()
	{
		WordsFound = 0;
		WordsAlreadyFound = 0;
		IsWrongInstance = false;
		TileDecrements = 0;
	}
}

public class WordPanel : UIMonoBehaviour
{
	public PuzzleLoader PuzzleLoaderRef;
	public WordPanelGroup WordPanelGroupPrefab;

	public WordPanelTitle Title;

	private int mWordCount;
	private int mWordsRemaining;

	private List<WordPanelGroup> mPanelGroups;
	private List<WordPanelEntry> mPanelEntries;

	private bool mIsPuzzleComplete;

	private float mPanelSize;

	public void Initialise(WordPair[] wordPairs)
	{
		CleanUp();

		mWordCount = wordPairs.Length;
		mWordsRemaining = mWordCount;
		mPanelGroups = new List<WordPanelGroup>(32);
		mPanelEntries = new List<WordPanelEntry>(mWordCount);

		Vector3 position = new Vector3(0, 38, 0); // TODO - fix the literals
		Vector3 groupStartPosition = Vector3.zero;
		int wordsPlaced = 0;
		int previousGroupSize = 0;
		for (int wordLength = 32; wordLength >= 3; --wordLength)
		{
			WordPair[] groupWords = Array.FindAll(wordPairs, word => word.Length == wordLength);
			if (groupWords.Length <= 0)
			{
				continue;
			}

			WordPanelGroup newGroup = Instantiate(WordPanelGroupPrefab, position, Quaternion.identity, transform) as WordPanelGroup;
			newGroup.transform.SetParent(transform);
			newGroup.rectTransform.localPosition = position;

			groupStartPosition.y -= previousGroupSize;
			newGroup.Initialise(this, wordLength, groupWords, ref mPanelEntries, groupStartPosition);
			previousGroupSize = newGroup.Height;

			mPanelGroups.Add(newGroup);

			wordsPlaced += groupWords.Length;
		}

		mPanelSize = Mathf.Abs(groupStartPosition.y - previousGroupSize);
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, mPanelSize);

		SaveGameManager.Instance.ActivePuzzleState.EnsureInitialisation(PuzzleLoaderRef.GetCurrentPuzzleSize(), mWordCount);

		bool[] wordsFoundSavedState = SaveGameManager.Instance.ActivePuzzleState.WordsFoundStates;
		for (int foundStateIndex = 0; foundStateIndex < mWordCount; ++foundStateIndex)
		{
			if (wordsFoundSavedState[foundStateIndex])
			{
				mPanelEntries[foundStateIndex].MarkWordAsFound();
				--mWordsRemaining;
			}
		}

		Title.SetTitle(mWordsRemaining);
	}

	void Update()
	{
		if (!mIsPuzzleComplete && mWordsRemaining <= 0)
		{
			mIsPuzzleComplete = true;
			StartCoroutine(SwitchToPuzzleCompleteLevel());
		}
	}

	private IEnumerator SwitchToPuzzleCompleteLevel()
	{
		InGameMenu inGameMenu = MenuManager.Instance.CurrentMenu as InGameMenu;
		if (inGameMenu)
		{
			inGameMenu.HidePauseButton();
		}

		yield return new WaitForSeconds(2);
		MenuManager.Instance.SwitchMenu(EMenuType.PuzzleCompleteMenu);
	}

	private void CleanUp()
	{
		mWordCount = 0;
		mWordsRemaining = 0;

		if (mPanelGroups != null)
		{
			for (int groupIndex = (mPanelGroups.Count - 1); groupIndex >= 0; --groupIndex)
			{
				if (mPanelGroups[groupIndex] != null)
				{
					Destroy(mPanelGroups[groupIndex].gameObject);
				}
			}
			mPanelGroups = null;
		}

		mPanelEntries = null;

		mIsPuzzleComplete = false;
	}

	public WordValidityResult CheckWordValidity(string word, List<CharacterTile> highlightedTiles)
	{
		//ODebug.Log(string.Format("Checking {0} validity", word));

		WordValidityResult result = new WordValidityResult();

		if (word.Length <= 2)
		{
			return result;
		}

		CharacterTile startTile = highlightedTiles.FirstItem();
		CharacterTile endTile = highlightedTiles.LastItem();

		bool isCompleteMatch = false;
		foreach (WordPanelEntry entry in mPanelEntries)
		{
			entry.DoesMatchSelection(word, startTile, endTile, out isCompleteMatch);
			if (isCompleteMatch || result.IsWrongInstance)
			{
				break;
			}
		}

		if (!isCompleteMatch)
		{
			return result;
		}

		bool updateTitle = false;

		foreach (WordPanelEntry entry in mPanelEntries)
		{
			EWordValidityResult matchResult = entry.DoesMatchSelection(word, startTile, endTile, out isCompleteMatch);
			/*if (matchResult != EWordValidityResult.NoMatch)
			{
				ODebug.LogWarning(string.Format("entry: {0}, result: {1}", entry.mWord, matchResult));
			}*/

			if (matchResult == EWordValidityResult.Match)
			{
				if (!entry.HasBeenFound)
				{
					--mWordsRemaining;

					entry.MarkWordAsFound();

					++result.WordsFound;
					result.Words.Add(entry.GetWord());

					SaveGameManager.Instance.ActivePuzzleState.MarkWordAsFound(entry.WordIndex);

					updateTitle = true;

					List<CharacterTile> tiles = new List<CharacterTile>();
					PuzzleLoaderRef.GetTilesBetween(entry.FromPosition, entry.ToPosition, ref tiles);
					foreach (CharacterTile tile in tiles)
					{
						tile.DecreaseUsage(1);
					}

					result.TileDecrements += tiles.Count;
				}
				else
				{
					++result.WordsAlreadyFound;
				}
			}
		}

		if (updateTitle)
		{
			Title.WordsRemoved(result, mWordsRemaining);
		}

		return result;
	}

	public void ToggleGroup(WordPanelGroup group)
	{
		group.ToggleCollapsedState();

		float deltaModifier = group.IsCollapsed ? group.HeightMinusTitle : -group.HeightMinusTitle;

		bool modifyDelta = false;
		for (int groupIndex = 0; groupIndex < mPanelGroups.Count; ++groupIndex)
		{
			WordPanelGroup otherGroup = mPanelGroups[groupIndex];
			if (otherGroup == group)
			{
				modifyDelta = true;
				continue;
			}

			if (modifyDelta)
			{
				otherGroup.ModifyYDelta(deltaModifier);
			}
		}

		mPanelSize -= deltaModifier;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, mPanelSize);
	}

	public float GetCompletePercentage()
	{
		float percentage = (float)(mWordCount - mWordsRemaining) / mWordCount;
		percentage *= 100;
		return percentage;
	}
}