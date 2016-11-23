using UnityEngine;
using UnityEngine.UI;
using System;

public class WordDefinitionViewer : UIMonoBehaviour
{
	public GameObject ValidDefinitionButtonSetRoot;
	public GameObject InvalidDefinitionButtonSetRoot;
	public SearchGoogleButton SearchGoogleButtonRef;

	public Text TextRef;
	public string DefinitionFormat;
	public string NoDefinitionMessage;

	void Awake()
	{
		DefinitionFormat = DefinitionFormat.Replace("\\n", Environment.NewLine);
	}

	public void ShowDefinitionFor(WordPair word)
	{
		if (word.HasDefinition)
		{
			TextRef.text = string.Format(DefinitionFormat, word.Forwards, word.Definition);
		}
		else
		{
			TextRef.text = string.Format(DefinitionFormat, word.Forwards, NoDefinitionMessage);
			SearchGoogleButtonRef.Initialise(word.Forwards);
		}

		Vector2 sizeDelta = rectTransform.sizeDelta;
		sizeDelta.y = TextRef.preferredHeight;
		rectTransform.sizeDelta = sizeDelta;

		ValidDefinitionButtonSetRoot.SetActive(word.HasDefinition);
		InvalidDefinitionButtonSetRoot.SetActive(!word.HasDefinition);
	}
}