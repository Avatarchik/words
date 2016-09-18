using System;

[Serializable]
public class WordPair
{
	public string Forward;
	public string Backwards;

	public WordPair(string word)
	{
		Forward = word;
		Backwards = WordHelper.ReverseWord(word);
	}
}