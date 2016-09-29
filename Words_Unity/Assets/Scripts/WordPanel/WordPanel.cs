using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public enum EWordValidityResult
{
	NoChange = 0,

	InvalidWord,

	CompleteMatch,
	WrongInstance,
	NoMatch,

	WasRemoved,
	WasAlreadyFound,
}

public class WordPanel : UIMonoBehaviour
{
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
		for (int wordLength = 3; wordsPlaced < mWordsRemaining; ++wordLength)
		{
			WordPair[] groupWords = Array.FindAll(wordPairs, word => word.Length == wordLength);

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

	public EWordValidityResult CheckWordValidity(string word, List<CharacterTile> highlightedTiles)
	{
		Debug.Log(string.Format("Checking {0} validity", word));

		if (word.Length <= 2)
		{
			return EWordValidityResult.InvalidWord;
		}

		string reversedWord = WordHelper.ReverseWord(word);
		CharacterTile startTile = highlightedTiles.FirstItem();
		CharacterTile endTile = highlightedTiles.LastItem();

		EWordValidityResult result = EWordValidityResult.NoChange;

		int wordsMarkedAsFound = 0;
		foreach (WordPanelEntry entry in mPanelEntries)
		{
			result = entry.DoesMatchSelection(word, reversedWord, startTile, endTile);
			if (result == EWordValidityResult.CompleteMatch)
			{
				if (!entry.HasBeenFound)
				{
					++wordsMarkedAsFound;
					entry.MarkWordAsFound();
					UpdateTitle(entry);
					result = EWordValidityResult.WasRemoved;
				}
				else
				{
					result = EWordValidityResult.WasAlreadyFound;
					break;
				}
			}
			else if (result == EWordValidityResult.WrongInstance)
			{
				break;
			}
		}

		if (wordsMarkedAsFound > 0)
		{
			foreach (CharacterTile tile in highlightedTiles)
			{
				tile.DecreaseUsage(wordsMarkedAsFound);
			}
			result = EWordValidityResult.WasRemoved;
		}

		return result;
	}

	public void UpdateTitle(WordPanelEntry word)
	{
		--mWordsRemaining;
		Title.SetTitle(mWordsRemaining);
	}
}