using UnityEngine;
using System;
using System.Collections.Generic;

public class Words : MonoBehaviour
{
	public int WordCount;
	public List<WordListPortion> ListPortions;
	public string[] WordList;

	public void Initialise()
	{
		WordCount = 0;
		ListPortions = new List<WordListPortion>();
		WordList = new string[0];
	}

	public void SetList(char initialChar, List<string> words)
	{
		int additionalWordsCount = words.Count;

		Array.Resize(ref WordList, WordCount + additionalWordsCount);

		words.Sort((a, b) => a.Length.CompareTo(b.Length));

		int desiredLength = 3;
		int placedWords = 0;
		List<string> newWords = new List<string>(additionalWordsCount);
		while (placedWords < additionalWordsCount)
		{
			newWords.Clear();
			foreach (string word in words)
			{
				if (word.Length == desiredLength)
				{
					newWords.Add(word);
				}
			}
			newWords.Sort((a, b) => a.Length.CompareTo(b.Length));

			int newWordsCount = newWords.Count;
			Array.Copy(newWords.ToArray(), 0, WordList, WordCount, newWordsCount);

			ListPortions.Add(new WordListPortion(initialChar, desiredLength, WordCount, WordCount + newWordsCount));
			WordCount += newWordsCount;
			placedWords += newWordsCount;

			++desiredLength;
		}
	}

	public string[] GetAllWords(int maxWordLength)
	{
		string[] foundWords = new string[WordCount];
		int foundWordsCount = 0;

		foreach (WordListPortion portion in ListPortions)
		{
			if (portion.WordLength <= maxWordLength)
			{
				Array.Copy(WordList, portion.StartIndex, foundWords, foundWordsCount, portion.ContainedWordsCount);
				foundWordsCount += portion.ContainedWordsCount;
			}
		}

		Array.Resize(ref foundWords, foundWordsCount);
		return foundWords;
	}
}