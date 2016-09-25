using UnityEngine;
using System;
using System.Collections.Generic;

public class WordPanelGroup : UIMonoBehaviour
{
	public WordPanelPair WordPanelPairPrefab;

	public WordPanelGroupTitle GroupTitle;

	private WordPanelPair[] mWordPanelPairs;
	private WordPanelEntry[] mWordEntries;

	public void Initialise(int groupWordLength, WordPair[] groupWords, ref List<WordPanelEntry> panelEntries, Vector3 startPosition, out int panelSize)
	{
		// TODO - this can all be preprocessed

		GroupTitle.SetTitle(groupWordLength);

#if UNITY_EDITOR
		name = string.Format("{0} Length Words", groupWordLength);
#endif // UNITY_EDITOR

		Array.Sort(groupWords);
		
		int wordsInGroup = groupWords.Length;
		int halfCount = Mathf.CeilToInt(wordsInGroup * 0.5f);
		mWordPanelPairs = new WordPanelPair[halfCount];
		mWordEntries = new WordPanelEntry[halfCount * 2];

		rectTransform.localPosition = startPosition;
		Vector3 position = new Vector3(0, -14, 0); // TODO - fix the literals

		int wordIndex = 0;
		for (int pairIndex = 0; pairIndex < halfCount; ++pairIndex)
		{
			WordPanelPair newPair = Instantiate(WordPanelPairPrefab, position, Quaternion.identity, transform) as WordPanelPair;
			newPair.transform.SetParent(transform);
#if UNITY_EDITOR
			newPair.gameObject.name = "Pair " + (pairIndex + 1);
#endif // UNITY_EDITOR

			position.y -= 24; // TODO - fix the literals
			newPair.rectTransform.localPosition = position;

			newPair.LeftWord.Initialise(groupWords[pairIndex]);
			panelEntries.Add(newPair.LeftWord);
			if ((pairIndex + halfCount) < wordsInGroup)
			{
				newPair.RightWord.Initialise(groupWords[pairIndex + halfCount]);
				panelEntries.Add(newPair.RightWord);
			}

			mWordPanelPairs[pairIndex] = newPair;
			mWordEntries[wordIndex++] = newPair.LeftWord;
			mWordEntries[wordIndex++] = newPair.RightWord;
		}

		panelSize = (int)Mathf.Abs(position.y - 14); // TODO - fix the literals
	}
}