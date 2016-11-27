using System;
using System.Threading;

static public class WordHelper
{
	const int kMaxWordLength = 32;
	static char[] charArray = new char[kMaxWordLength];

	static public string ReverseWord(string word)
	{
		Array.Clear(charArray, 0, kMaxWordLength);

		int originalWordLength = word.Length;
		for (int sourceCharIndex = (originalWordLength - 1), destCharIndex = 0; sourceCharIndex >= 0; --sourceCharIndex, ++destCharIndex)
		{
			charArray[destCharIndex] = word[sourceCharIndex];
		}

		string reversedWord = new string(charArray, 0, originalWordLength);
		return reversedWord;
	}

	static public string ConvertToTitleCase(string word)
	{
		word = word.ToLower();
		word = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(word);
		return word;
	}
}