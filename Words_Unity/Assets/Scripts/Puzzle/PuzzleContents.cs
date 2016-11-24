using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR
using System;

public class PuzzleContents : ScriptableObject
{
	public SerializableGuid Guid;

	public int Size;

	public int WordCount;
	public int MaxCharacterUsage;

	public WordPair[] Words;

	public CharacterUsage[] CharGrid;

	public void Initialise(int size)
	{
		Size = size;

		WordCount = 0;
		MaxCharacterUsage = 0;

		Words = new WordPair[1024];

		CharGrid = new CharacterUsage[Size * Size];

#if UNITY_EDITOR
		EditorUtility.SetDirty(this);
#endif // UNITY_EDITOR
	}

	public bool Finalise(CharacterUsage[,] generatedGrid)
	{
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

	public void UpdateDefinitions(WordDefinitions definitions)
	{
		int wordCount = Words.Length;
		for (int wordIndex = 0; wordIndex < wordCount; ++wordIndex)
		{
			definitions.GetDefinitionFor(Words[wordIndex].Forwards, ref Words[wordIndex].Definition);
			Words[wordIndex].HasDefinition = !string.IsNullOrEmpty(Words[wordIndex].Definition);
		}
	}

	public void RegisterWord(string word, string definition, GridPosition fromPosition, GridPosition toPosition)
	{
		Words[WordCount] = new WordPair(word, definition, fromPosition, toPosition);
		++WordCount;
	}

	private void SetCharacterUsage(CharacterUsage[,] generatedGrid)
	{
		int charCount = 0;
		for (int x = 0; x < Size; ++x)
		{
			for (int y = 0; y < Size; ++y)
			{
				CharGrid[charCount] = generatedGrid[x, y];
				++charCount;
			}
		}
	}

	private void FindMaxCharacterUsage()
	{
		int charCount = Size * Size;
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
					//ODebug.Log("Duplicate placement of " + innerWord.Forwards);
					break;
				}
			}

			if (foundDuplicate)
			{
				break;
			}
		}

		return foundDuplicate;
	}
}