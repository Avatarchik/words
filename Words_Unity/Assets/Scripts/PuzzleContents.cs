using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR
using System;

public class PuzzleContents : ScriptableObject
{
	public int Width;
	public int Height;

	public int WordCount;
	public int MaxCharacterUsage;

	public WordPair[] Words;
	public WordPlacement[] WordPlacements;

	public CharacterUsage[] CharGrid;

	public void Initialise(int width, int height)
	{
		Width = width;
		Height = height;

		WordCount = 0;
		MaxCharacterUsage = 0;

		Words = new WordPair[1024];
		WordPlacements = new WordPlacement[1024];

		CharGrid = new CharacterUsage[Width * Height];

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif // UNITY_EDITOR
	}

	public void Finalise(/*GridEntry[,] generatedGrid*/)
	{
		// TODO fix
		/*Array.Resize(ref Words, WordCount);
		Array.Resize(ref WordPlacements, WordCount);

		int charCount = 0;
		CharacterUsage usage;
		GridEntry entry;
		for (int x = 0; x < Width; ++x)
		{
			for (int y = 0; y < Height; ++y)
			{
				entry = generatedGrid[x, y];
				usage = CharGrid[charCount];

				// TODO - fix
				/ *usage.Character = entry.Character;
				usage.NumberOfUses = entry.CharacterCount;* /

				CharGrid[charCount] = usage;

				++charCount;
			}
		}

		for (int charIndex = 0; charIndex < charCount; ++charIndex)
		{
			MaxCharacterUsage = Mathf.Max(MaxCharacterUsage, CharGrid[charIndex].NumberOfUses);
		}

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif // UNITY_EDITOR*/
	}

	public void RegisterWord(string word, GridPosition fromPosition, GridPosition toPosition)
	{
		Words[WordCount] = new WordPair(word);
		WordPlacements[WordCount] = new WordPlacement(fromPosition, toPosition);

		++WordCount;
	}
}