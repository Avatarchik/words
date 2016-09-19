using System;

[Serializable]
public class WordPair : IComparable
{
	public string Forwards;
	public string Backwards;
	public int Length;

	public WordPair(string word)
	{
		Forwards = word;
		Backwards = WordHelper.ReverseWord(word);
		Length = word.Length;
	}

	public int CompareTo(object obj)
	{
		WordPair wordPair = (WordPair)obj;
		return Forwards.CompareTo(wordPair.Forwards);
	}
}