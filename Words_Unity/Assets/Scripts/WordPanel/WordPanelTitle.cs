using UnityEngine;
using UnityEngine.UI;

public class WordPanelTitle : MonoBehaviour
{
	public WordPanelFoundWord FoundWordPrefab;

	public Text TextRef;
	public string TitleFormat;
	public string OneWordLeftTitle;
	public string AllFoundTitle;

	public void SetTitle(int wordsLeft)
	{
		if (wordsLeft > 1)
		{
			TextRef.text = string.Format(TitleFormat, wordsLeft);
		}
		else if (wordsLeft == 1)
		{
			TextRef.text = OneWordLeftTitle;
		}
		else
		{
			TextRef.text = AllFoundTitle;
		}
	}

	public void WordsRemoved(WordValidityResult validityResult, int wordsNowRemaining)
	{
		WordPanelFoundWord foundWordsInst = Instantiate(FoundWordPrefab, transform.position, Quaternion.identity, transform) as WordPanelFoundWord;
		foundWordsInst.SetAsFoundWords(validityResult.WordsFound, 0);

		int wordCount = validityResult.WordsFound;
		for (int wordIndex = 0; wordIndex < wordCount; ++wordIndex)
		{
			WordPanelFoundWord foundWordInst = Instantiate(FoundWordPrefab, transform.position, Quaternion.identity, transform) as WordPanelFoundWord;
			foundWordInst.SetAsFoundWord(validityResult.Words[wordIndex], (wordIndex + 1) * 0.08f);
		}

		SetTitle(wordsNowRemaining);
	}
}