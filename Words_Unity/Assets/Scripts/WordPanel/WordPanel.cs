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

	private int mWordsRemaining;
	private List<WordPanelGroup> mPanelGroups;
	private List<WordPanelEntry> mPanelEntries;

	private bool mIsPuzzleComplete;

	public void Initialise(WordPair[] wordPairs)
	{
		CleanUp();

		mWordsRemaining = wordPairs.Length;
		mPanelGroups = new List<WordPanelGroup>(32);
		mPanelEntries = new List<WordPanelEntry>(mWordsRemaining);

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
			newGroup.Initialise(wordLength, groupWords, ref mPanelEntries, groupStartPosition, out previousGroupSize);

			mPanelGroups.Add(newGroup);

			wordsPlaced += groupWords.Length;
		}

		Title.SetTitle(mWordsRemaining);

		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Abs(groupStartPosition.y - previousGroupSize));
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

		string reversedWord = WordHelper.ReverseWord(word);
		CharacterTile startTile = highlightedTiles.FirstItem();
		CharacterTile endTile = highlightedTiles.LastItem();

		bool isCompleteMatch = false;
		foreach (WordPanelEntry entry in mPanelEntries)
		{
			entry.DoesMatchSelection(word, reversedWord, startTile, endTile, out isCompleteMatch);
			if (isCompleteMatch)
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
			EWordValidityResult matchResult = entry.DoesMatchSelection(word, reversedWord, startTile, endTile, out isCompleteMatch);
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
			else if (matchResult == EWordValidityResult.WrongInstance)
			{
				result.IsWrongInstance = true;
			}
		}

		if (updateTitle)
		{
			Title.WordsRemoved(result.WordsFound, mWordsRemaining);
		}

		return result;
	}
}