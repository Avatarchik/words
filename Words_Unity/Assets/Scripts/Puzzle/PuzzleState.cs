using System;

[Serializable]
public class PuzzleState
{
	public int Score;
	public int TimeMins;
	public int TimeSeconds;
	public float PercentageComplete;

	public bool[] WordsFoundStates;
	public int[] CharUsageLeftStates;

	public bool IsCompleted
	{
		get
		{
			return PercentageComplete >= 100;
		}
	}

	public void EnsureInitialisation(int puzzleSize, int wordCount)
	{
		bool shouldReset = false;

		if (WordsFoundStates == null || WordsFoundStates.Length == 0)
		{
			WordsFoundStates = new bool[wordCount];
			shouldReset = true;
		}

		if (CharUsageLeftStates == null || CharUsageLeftStates.Length == 0)
		{
			CharUsageLeftStates = new int[puzzleSize * puzzleSize];
			shouldReset = true;
		}

		if (shouldReset)
		{
			Reset();
		}
	}

	public void Reset()
	{
		Score = 0;
		TimeMins = 0;
		TimeSeconds = 0;
		PercentageComplete = 0;

		int wordCount = WordsFoundStates.Length;
		for (int wordIndex = 0; wordIndex < wordCount; ++wordIndex)
		{
			WordsFoundStates[wordIndex] = false;
		}

		int charCount = CharUsageLeftStates.Length;
		for (int charIndex = 0; charIndex < charCount; ++charIndex)
		{
			CharUsageLeftStates[charIndex] = -1;
		}
	}

	public void MarkWordAsFound(int wordIndex)
	{
		WordsFoundStates[wordIndex] = true;
	}

	public void SetCharacterUsageLeft(int charIndex, int usageLeft)
	{
		CharUsageLeftStates[charIndex] = usageLeft;
	}
}