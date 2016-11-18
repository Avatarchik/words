using System;

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