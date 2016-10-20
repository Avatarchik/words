using UnityEngine;
using UnityEngine.UI;

public class WordDefinitionViewer : MonoBehaviour
{
	public Text TextRef;
	public string DefinitionFormat;

	public void UpdateDefinitionAsMissing()
	{
		TextRef.text = string.Format(DefinitionFormat, "We aren't quite sure. Would you like to Google it?");
	}

	public void UpdateDefinition(string newDefinition)
	{
		TextRef.text = string.Format(DefinitionFormat, newDefinition);
	}
}