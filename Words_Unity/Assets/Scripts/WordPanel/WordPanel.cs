using UnityEngine;
using System;
using System.Collections.Generic;

public class WordPanel : UIMonoBehaviour
{
	public WordPanelGroup WordPanelGroupPrefab;

	public WordPanelTitle Title;

	private int mWordsRemaining;
	private List<WordPanelGroup> mPanelGroups;
	private List<WordPanelEntry> mPanelEntries;

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
	}

	public void CheckWordValidity(string word, List<CharacterTile> highlightedTiles, out bool wasWordRemoved, out bool wasWordAlreadyFound)
	{
		Debug.Log(string.Format("Checking {0} validity", word));

		wasWordRemoved = false;
		wasWordAlreadyFound = false;

		if (word.Length <= 2)
		{
			return;
		}

		string reversedWord = WordHelper.ReverseWord(word);

		int wordsMatched = 0;
		foreach (WordPanelEntry sourceWord in mPanelEntries)
		{
			string sourceText = sourceWord.Word;
			if (sourceText == word || sourceText == reversedWord)
			{
				++wordsMatched;

				wasWordRemoved = !sourceWord.HasBeenFound;
				wasWordAlreadyFound = sourceWord.HasBeenFound;

				sourceWord.MarkWordAsFound();
				UpdateTitle(sourceWord);
			}
		}

		if (wasWordRemoved)
		{
			foreach (CharacterTile tile in highlightedTiles)
			{
				tile.DecreaseUsage(wordsMatched);
			}
		}
	}

	public void UpdateTitle(WordPanelEntry word)
	{
		--mWordsRemaining;
		Title.SetTitle(mWordsRemaining);
	}
}