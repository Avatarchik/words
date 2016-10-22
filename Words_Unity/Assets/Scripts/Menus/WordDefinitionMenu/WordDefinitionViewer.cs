using UnityEngine;
using UnityEngine.UI;

public class WordDefinitionViewer : MonoBehaviour
{
	public GameObject ValidDefinitionButtonSetRoot;
	public GameObject InvalidDefinitionButtonSetRoot;
	public SearchGoogleButton SearchGoogleButtonRef;

	public Text TextRef;
	public string DefinitionFormat;

	public Words WordsRef;

	void Awake()
	{
		DefinitionFormat = DefinitionFormat.Replace("\\n", System.Environment.NewLine);
	}

	public void ShowDefinitionFor(string word)
	{
		string definition = string.Empty;
		bool hasValidDefinition = WordsRef.GetDefinitionFor(word, ref definition);

		TextRef.text = string.Format(DefinitionFormat, word, definition);

		ValidDefinitionButtonSetRoot.SetActive(hasValidDefinition);
		InvalidDefinitionButtonSetRoot.SetActive(!hasValidDefinition);

		if (!hasValidDefinition)
		{
			SearchGoogleButtonRef.Initialise(word);
		}
	}
}