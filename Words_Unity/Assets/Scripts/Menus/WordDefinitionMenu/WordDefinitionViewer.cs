using UnityEngine;
using UnityEngine.UI;

public class WordDefinitionViewer : MonoBehaviour
{
	public Text TextRef;
	public string DefinitionFormat;

	public Words WordsRef;

	void Awake()
	{
		DefinitionFormat = DefinitionFormat.Replace("\\n", System.Environment.NewLine);
	}

	public void ShowDefinitionFor(string word)
	{
		string definition = WordsRef.GetDefinitionFor(word);
		TextRef.text = string.Format(DefinitionFormat, word, definition);
	}
}