using UnityEngine;
using UnityEngine.UI;
using System;

public class WordDefinitionViewer : UIMonoBehaviour, IOrientationChangedNotifiee
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

	public virtual void OnEnable()
	{
		OrientationManager.Instance.RegisterForNotification(this);
	}

	public virtual void OnDisable()
	{
		OrientationManager.Instance.UnregisterForNotification(this);
	}

	public void OnScreenSizeChanged(Vector2 screenSize)
	{
		Vector2 sizeDelta = rectTransform.sizeDelta;
		sizeDelta.y = TextRef.preferredHeight * 1.05f;
		rectTransform.sizeDelta = sizeDelta;
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