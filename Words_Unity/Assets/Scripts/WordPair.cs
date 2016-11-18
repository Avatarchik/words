using System;

[Serializable]
public class WordPair : IComparable
{
	public int Length;

	public string Forwards;
	public string Backwards;

	public string Definition;
	public bool HasDefinition;

	public GridPosition FromPosition;
	public GridPosition ToPosition;

	public WordPair(string word, string definition, GridPosition fromPosition, GridPosition toPosition)
	{
		Length = word.Length;

		Forwards = word;
		Backwards = WordHelper.ReverseWord(word);

		Definition = definition;

		FromPosition = fromPosition;
		ToPosition = toPosition;
	}

	public int CompareTo(object obj)
	{
		WordPair rhs = (WordPair)obj;
		return Forwards.CompareTo(rhs.Forwards);
	}

	static public bool operator ==(WordPair lhs, WordPair rhs)
	{
		bool areEqual = true;

		areEqual &= lhs.Forwards == rhs.Forwards;
		areEqual &= lhs.Backwards == rhs.Backwards;
		areEqual &= lhs.Length == rhs.Length;
		areEqual &= lhs.FromPosition == rhs.FromPosition;
		areEqual &= lhs.ToPosition == rhs.ToPosition;

		return areEqual;
	}

	static public bool operator !=(WordPair lhs, WordPair rhs)
	{
		return !(lhs == rhs);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}

		WordPair rhs = (WordPair)obj;

		bool areEqual = true;

		areEqual &= Forwards == rhs.Forwards;
		areEqual &= Backwards == rhs.Backwards;
		areEqual &= Length == rhs.Length;
		areEqual &= FromPosition == rhs.FromPosition;
		areEqual &= ToPosition == rhs.ToPosition;

		return areEqual;
	}

	public override int GetHashCode()
	{
		// TODO - not sure if this is correct
		return Forwards.GetHashCode();
	}
}