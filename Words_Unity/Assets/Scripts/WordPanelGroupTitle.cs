using UnityEngine;
using UnityEngine.UI;

public class WordPanelGroupTitle : MonoBehaviour
{
	public Text TextRef;
	public string TitleFormat;

	public void SetTitle(int groupWordLength)
	{
		TextRef.text = string.Format(TitleFormat, groupWordLength);
	}
}