using System;

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
}