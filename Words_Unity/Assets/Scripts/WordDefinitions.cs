using UnityEngine;
using System;

[Serializable]
public class WordDefinition
{
	public string ActualWord;
	public string Definition;

	public WordDefinition(string word, string definition)
	{
		ActualWord = word;
		Definition = definition;
	}
}

public class WordDefinitions : MonoBehaviour
{
	public int DefinitionCount;
	public WordDefinition[] DefinitionList;

	private int mNextFreeIndex;

	public void Initialise(int maximumSize)
	{
		DefinitionCount = 0;
		DefinitionList = new WordDefinition[maximumSize];
		mNextFreeIndex = 0;
	}

	public void AddNewDefinition(string word, string definition)
	{
		DefinitionList[mNextFreeIndex++] = new WordDefinition(word, definition);
	}

	public void Finalise()
	{
		Array.Resize(ref DefinitionList, mNextFreeIndex);

		Comparison<WordDefinition> comparison = (a, b) => a.ActualWord.CompareTo(b.ActualWord);
		Array.Sort(DefinitionList, comparison);

		DefinitionCount = DefinitionList.Length;
	}

	public bool GetDefinitionFor(string word, ref string definition)
	{
		definition = string.Empty;

		for (int definitionIndex = 0; definitionIndex < DefinitionCount; ++definitionIndex)
		{
			if (DefinitionList[definitionIndex].ActualWord == word)
			{
				definition = DefinitionList[definitionIndex].Definition;
				break;
			}
		}

		bool hasFoundDefinition = !string.IsNullOrEmpty(definition);
		return hasFoundDefinition;
	}
}