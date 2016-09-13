using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordPanel : MonoBehaviour
{
	public WordPanelPair WordPanelPairPrefab;

	public WordPanelTitle Title;

	private List<WordPanelPair> mWordPairs;
	private List<Word> mWords;

	private int mWordCount;

	public void Initialise(List<string> words)
	{
		mWordCount = words.Count;
		int halfCount = Mathf.CeilToInt(mWordCount * 0.5f);
		mWordPairs = new List<WordPanelPair>(halfCount);
		mWords = new List<Word>(mWordCount);

		Vector3 position = Vector3.zero;
		for (int pairIndex = 0; pairIndex < halfCount; ++pairIndex)
		{
			WordPanelPair newPair = Instantiate(WordPanelPairPrefab, position, Quaternion.identity, transform) as WordPanelPair;
			newPair.transform.SetParent(transform);
			newPair.gameObject.name = "Pair " + (pairIndex + 1);

			RectTransform newPairRectTrans = newPair.GetComponent<RectTransform>();
			position.y = -24 + (pairIndex * -24);
			newPairRectTrans.localPosition = position;

			newPair.LeftWord.SetText(words[pairIndex]);
			if ((pairIndex + halfCount) < mWordCount)
			{
				newPair.RightWord.SetText(words[pairIndex + halfCount]);
			}

			mWordPairs.Add(newPair);
			mWords.Add(newPair.LeftWord);
			mWords.Add(newPair.RightWord);
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

		string wordReversed = string.Empty;
		for (int charIndex = (word.Length - 1); charIndex >= 0; --charIndex)
		{
			wordReversed += word[charIndex];
		}

		bool foundMatch = false;

		foreach (Word sourceWord in mWords)
		{
			if (sourceWord.HasBeenFound)
			{
				continue;
			}

			string sourceText = sourceWord.GetText();
			if (sourceText == word || sourceText == wordReversed)
			{
				sourceWord.MarkWordAsFound();
				UpdateTitle(sourceWord);

				foundMatch = true;
				break;
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