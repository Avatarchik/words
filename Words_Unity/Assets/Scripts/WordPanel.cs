using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class WordPanel : MonoBehaviour
{
	public WordPanelPair WordPanelPairPrefab;

	public WordPanelTitle Title;

	private List<WordPanelPair> WordPairs;

	private int mWordCount;

	public void Initialise(List<string> words)
	{
		mWordCount = words.Count;
		int halfCount = Mathf.CeilToInt(mWordCount * 0.5f);
		WordPairs = new List<WordPanelPair>(halfCount);

		Vector3 position = Vector3.zero;
		for (int pairIndex = 0; pairIndex < halfCount; ++pairIndex)
		{
			WordPanelPair newPair = Instantiate(WordPanelPairPrefab, position, Quaternion.identity, transform) as WordPanelPair;
			newPair.transform.SetParent(transform);
			newPair.gameObject.name = "Pair " + (pairIndex + 1);

			RectTransform newPairRectTrans = newPair.GetComponent<RectTransform>();
			position.y = -24 + (pairIndex * -24);
			newPairRectTrans.localPosition = position;

			newPair.LeftWord.TextComp.text = words[pairIndex];
			if ((pairIndex + halfCount) < mWordCount)
			{
				newPair.RightWord.TextComp.text = words[pairIndex + halfCount];
			}

			WordPairs.Add(newPair);
		}

		Title.SetWordsLeftCount(mWordCount);

		int newHeight = ((halfCount + 1) * 24) - 12;
		RectTransform rectTrans = GetComponent<RectTransform>();
		rectTrans.sizeDelta = new Vector2(rectTrans.sizeDelta.x, newHeight);
	}

	public void MarkWordAsFound(string word)
	{
		--mWordCount;
		Title.SetWordsLeftCount(mWordCount);
	}
}