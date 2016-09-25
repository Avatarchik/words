using System;

[Serializable]
public class WordPair : IComparable
{
	public string Forwards;
	public string Backwards;
	public int Length;

	public GridPosition FromPosition;
	public GridPosition ToPosition;

	public WordPair(string word, GridPosition fromPosition, GridPosition toPosition)
	{
		Forwards = word;
		Backwards = WordHelper.ReverseWord(word);
		Length = word.Length;

		FromPosition = fromPosition;
		ToPosition = toPosition;
	}

	public int CompareTo(object obj)
	{
		WordPair wordPair = (WordPair)obj;
		return Forwards.CompareTo(wordPair.Forwards);
	}
}