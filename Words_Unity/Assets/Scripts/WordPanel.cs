using UnityEngine;
using System;
using System.Collections.Generic;

public class WordPanel : UIMonoBehaviour
{
	public WordPanelGroup WordPanelGroupPrefab;

	public WordPanelTitle Title;

	private List<WordPanelEntry> mPanelEntries;
	private int mWordsRemaining;

	public void Initialise(WordPair[] wordPairs)
	{
		mWordsRemaining = wordPairs.Length;
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

			wordsPlaced += groupWords.Length;
		}

		Title.SetTitle(mWordsRemaining);

		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Abs(groupStartPosition.y - previousGroupSize));
	}

	public bool RemoveWordIfExists(string word)
	{
		Debug.Log(string.Format("Checking {0} validity", word));

		if (word.Length <= 2)
		{
			return false;
		}

		string reversedWord = WordHelper.ReverseWord(word);

		bool foundMatch = false;

		foreach (WordPanelEntry sourceWord in mPanelEntries)
		{
			if (sourceWord.HasBeenFound)
			{
				continue;
			}

			string sourceText = sourceWord.Word;
			if (sourceText == word || sourceText == reversedWord)
			{
				sourceWord.MarkWordAsFound();
				UpdateTitle(sourceWord);

				foundMatch = true;
			}
		}

		return foundMatch;
	}

	public void UpdateTitle(WordPanelEntry word)
	{
		--mWordsRemaining;
		Title.SetTitle(mWordsRemaining);
	}
}