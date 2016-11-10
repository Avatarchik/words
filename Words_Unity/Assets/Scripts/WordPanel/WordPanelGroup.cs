using UnityEngine;
using System;
using System.Collections.Generic;

public class WordPanelGroup : UIMonoBehaviour
{
	public WordPanelPair WordPanelPairPrefab;

	public WordPanelGroupTitle GroupTitle;

	private WordPanelPair[] mWordPanelPairs;

	public int Height { get; private set; }
	public int HeightMinusTitle { get; private set; }

	public bool IsCollapsed { get; private set; }

	private int mWordsInGroup;
	private int mWordsFound;

	private WordPanel mWordPanelRef;

	public void Initialise(WordPanel panelRef, int groupWordLength, WordPair[] groupWords, ref List<WordPanelEntry> panelEntries, Vector3 startPosition)
	{
		// TODO - this can all be preprocessed

		GroupTitle.Initialise(panelRef, this, groupWordLength);

#if UNITY_EDITOR
		name = string.Format("{0} Length Words", groupWordLength);
#endif // UNITY_EDITOR

		Array.Sort(groupWords);

		mWordsInGroup = groupWords.Length;
		int halfCount = Mathf.CeilToInt(mWordsInGroup * 0.5f);
		mWordPanelPairs = new WordPanelPair[halfCount];

		rectTransform.localPosition = startPosition;
		Vector3 position = new Vector3(0, -22, 0); // TODO - fix the literals

		for (int pairIndex = 0; pairIndex < halfCount; ++pairIndex)
		{
			WordPanelPair newPair = Instantiate(WordPanelPairPrefab, position, Quaternion.identity, transform) as WordPanelPair;
			newPair.transform.SetParent(transform);
#if UNITY_EDITOR
			newPair.gameObject.name = "Pair " + (pairIndex + 1);
#endif // UNITY_EDITOR

			position.y -= 24; // TODO - fix the literals
			newPair.rectTransform.localPosition = position;

			newPair.LeftWord.Initialise(this, groupWords[pairIndex], panelEntries.Count);
			panelEntries.Add(newPair.LeftWord);
			if ((pairIndex + halfCount) < mWordsInGroup)
			{
				newPair.RightWord.Initialise(this, groupWords[pairIndex + halfCount], panelEntries.Count);
				panelEntries.Add(newPair.RightWord);
			}

			mWordPanelPairs[pairIndex] = newPair;
		}

		Height = (int)Mathf.Abs(position.y - 14); // TODO - fix the literal
		HeightMinusTitle = Height - 36; // TODO - fix the literal

		IsCollapsed = false;

		mWordsFound = 0;

		mWordPanelRef = panelRef;
	}

	public void ToggleCollapsedState()
	{
		IsCollapsed = !IsCollapsed;
		SetCollapsedState();
	}

	private void SetCollapsedState()
	{
		foreach (WordPanelPair pair in mWordPanelPairs)
		{
			pair.gameObject.SetActive(!IsCollapsed);
		}
	}

	public void ModifyYDelta(float deltaModifier)
	{
		Vector3 pos = rectTransform.localPosition;
		pos.y += deltaModifier;
		rectTransform.localPosition = pos;
	}

	public void IncrementWordsFound()
	{
		++mWordsFound;

		if (!IsCollapsed)
		{
			if (mWordsFound >= mWordsInGroup)
			{
				Invoke("AutoHideGroup", 2);
			}
		}
	}

	private void AutoHideGroup()
	{
		mWordPanelRef.ToggleGroup(this);
	}
}