using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Word
{
	public string ActualWord = string.Empty;
	public string Definition = string.Empty;
}

public class Words : MonoBehaviour
{
	[Serializable]
	public class WordListPortion
	{
		public char InitialChar;
		public int WordLength;
		public int StartIndex;
		public int EndIndex;
		public int ContainedWordsCount;

		public WordListPortion(char initialChar, int wordLength, int startIndex, int endIndex)
		{
			InitialChar = initialChar;
			WordLength = wordLength;
			StartIndex = startIndex;
			EndIndex = endIndex;
			ContainedWordsCount = EndIndex - StartIndex;
		}
	}

	public int WordCount;
	public List<WordListPortion> ListPortions;
	public  Word[] WordList;

	public void Initialise()
	{
		WordCount = 0;
		ListPortions = new List<WordListPortion>();
		WordList = new Word[0];
	}

	public void SetList(char initialChar, List<string> words)
	{
		int additionalWordsCount = words.Count;

		Array.Resize(ref WordList, WordCount + additionalWordsCount);

		words.Sort((a, b) => a.Length.CompareTo(b.Length));

		int desiredLength = 3;
		int placedWords = 0;
		List<Word> newWords = new List<Word>(additionalWordsCount);
		while (placedWords < additionalWordsCount)
		{
			newWords.Clear();
			foreach (string word in words)
			{
				if (word.Length == desiredLength)
				{
					Word newWord = new Word();
					newWord.ActualWord = word;
					newWords.Add(newWord);
				}
			}
			newWords.Sort((a, b) => a.ActualWord.Length.CompareTo(b.ActualWord.Length));

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

	// TODO - this should all be preprocessed and stored in each puzzle's data
	public string GetDefinitionFor(string word)
	{
		string definition = null;

		char initialChar = word[0];
		int wordLength = word.Length;

		foreach (WordListPortion portion in ListPortions)
		{
			if ((portion.InitialChar == initialChar) && (portion.WordLength == wordLength))
			{
				for (int wordIndex = portion.StartIndex; wordIndex < portion.EndIndex; ++wordIndex)
				{
					if (WordList[wordIndex].ActualWord == word)
					{
						definition = WordList[wordIndex].Definition;
						break;
					}
				}

				break;
			}
		}

		if (string.IsNullOrEmpty(definition))
		{
			definition = "We aren't quite sure. Would you like to Google it?";
		}

		return definition;
	}
}