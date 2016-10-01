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

	public CharacterUsage[] CharGrid;

	public void Initialise(int width, int height)
	{
		Width = width;
		Height = height;

		WordCount = 0;
		MaxCharacterUsage = 0;

		Words = new WordPair[1024];

		CharGrid = new CharacterUsage[Width * Height];

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif // UNITY_EDITOR
	}

	public bool Finalise(GridEntry[,] generatedGrid)
	{
		// TODO fix
		Array.Resize(ref Words, WordCount);

		SetCharacterUsage(generatedGrid);
		FindMaxCharacterUsage();
		bool foundDuplicatePlacements = CheckForDuplicatePlacements();

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif // UNITY_EDITOR

		bool wasSuccessful = !foundDuplicatePlacements;
		return wasSuccessful;
	}

	public void RegisterWord(string word, GridPosition fromPosition, GridPosition toPosition)
	{
		Words[WordCount] = new WordPair(word, fromPosition, toPosition);
		++WordCount;
	}

	private void SetCharacterUsage(GridEntry[,] generatedGrid)
	{
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
				usage.Character = entry.Character;
				usage.NumberOfUses = entry.NumberOfUses;

				CharGrid[charCount] = usage;

				++charCount;
			}
		}
	}

	private void FindMaxCharacterUsage()
	{
		int charCount = Width * Height;
		for (int charIndex = 0; charIndex < charCount; ++charIndex)
		{
			MaxCharacterUsage = Mathf.Max(MaxCharacterUsage, CharGrid[charIndex].NumberOfUses);
		}
	}

	private bool CheckForDuplicatePlacements()
	{
		bool foundDuplicate = false;

		int wordCount = Words.Length;
		WordPair outerWord;
		WordPair innerWord;
		for (int outerWordIndex = 0; outerWordIndex < wordCount; ++outerWordIndex)
		{
			outerWord = Words[outerWordIndex];

			for (int innerWordIndex = 0; innerWordIndex < wordCount; ++innerWordIndex)
			{
				if (outerWordIndex == innerWordIndex)
				{
					continue;
				}

				innerWord = Words[innerWordIndex];

				if (outerWord == innerWord)
				{
					foundDuplicate = true;
					break;
				}
			}
		}

		return foundDuplicate;
	}
}