using System;

static public class WordHelper
{
	const int kMaxWordLength = 32;
	static char[] charArray = new char[kMaxWordLength];

	static public string ReverseWord(string word)
	{
		Array.Clear(charArray, 0, kMaxWordLength);

		for (int sourceCharIndex = (word.Length - 1), destCharIndex = 0; sourceCharIndex >= 0; --sourceCharIndex, ++destCharIndex)
		{
			charArray[destCharIndex] = word[sourceCharIndex];
		}

		string reversedWord = new string(charArray);
		return reversedWord;
	}
}