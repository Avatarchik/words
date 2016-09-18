using UnityEngine;
using System.Collections.Generic;

public class WordPanel : MonoBehaviour
{
	public WordPanelPair WordPanelPairPrefab;

	public WordPanelTitle Title;

	private WordPanelPair[] mWordPanelPairs;
	private Word[] mWords;

	private int mWordCount;

	public void Initialise(WordPair[] wordPairs)
	{
		//wordPairs.Sort();

		mWordCount = wordPairs.Length;
		int halfCount = Mathf.CeilToInt(mWordCount * 0.5f);
		mWordPanelPairs = new WordPanelPair[halfCount];
		mWords = new Word[mWordCount];

		Vector3 position = Vector3.zero;
		int wordIndex = 0;
		for (int pairIndex = 0; pairIndex < halfCount; ++pairIndex)
		{
			WordPanelPair newPair = Instantiate(WordPanelPairPrefab, position, Quaternion.identity, transform) as WordPanelPair;
			newPair.transform.SetParent(transform);
			newPair.gameObject.name = "Pair " + (pairIndex + 1);

			RectTransform newPairRectTrans = newPair.GetComponent<RectTransform>();
			position.y = -24 + (pairIndex * -24);
			newPairRectTrans.localPosition = position;

			newPair.LeftWord.SetText(wordPairs[pairIndex].Forward);
			if ((pairIndex + halfCount) < mWordCount)
			{
				newPair.RightWord.SetText(wordPairs[pairIndex + halfCount].Forward);
			}

			mWordPanelPairs[pairIndex] = newPair;
			mWords[wordIndex++] = newPair.LeftWord;
			mWords[wordIndex++] = newPair.RightWord;
		}

		Title.SetWordsLeftCount(mWordCount);

		int newHeight = ((halfCount + 1) * 24) - 12;
		RectTransform rectTrans = GetComponent<RectTransform>();
		rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, newHeight);
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

		foreach (Word sourceWord in mWords)
		{
			if (sourceWord.HasBeenFound)
			{
				continue;
			}

			string sourceText = sourceWord.GetText();
			if (sourceText == word || sourceText == reversedWord)
			{
				sourceWord.MarkWordAsFound();
				UpdateTitle(sourceWord);

				foundMatch = true;
			}
		}

		return foundMatch;
	}

	public void UpdateTitle(Word word)
	{
		--mWordCount;
		Title.SetWordsLeftCount(mWordCount);
	}
}