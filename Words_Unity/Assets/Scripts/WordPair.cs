using System;

[Serializable]
public class WordPair : IComparable
{
	public string Forwards;
	public string Backwards;
	public int Length;

	//[NonSerialized]
	public GridPosition FromPosition;
	//[NonSerialized]
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