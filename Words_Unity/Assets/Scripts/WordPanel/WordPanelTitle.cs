using UnityEngine;
using UnityEngine.UI;

public class WordPanelTitle : MonoBehaviour
{
	public Text TextRef;
	public string TitleFormat;
	public string AllFoundTitle;

	public void SetTitle(int wordsLeft)
	{
		if (wordsLeft > 0)
		{
			TextRef.text = string.Format(TitleFormat, wordsLeft);
		}
		else
		{
			TextRef.text = AllFoundTitle;
		}
	}
}