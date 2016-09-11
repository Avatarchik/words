using UnityEngine;
using UnityEngine.UI;

public class WordPanelTitle : MonoBehaviour
{
	public Text TextComp;
	public string TitleFormat;
	public string AllFoundTitle;

	public void SetWordsLeftCount(int wordsLeft)
	{
		if (wordsLeft > 0)
		{
			TextComp.text = string.Format(TitleFormat, wordsLeft);
		}
		else
		{
			TextComp.text = AllFoundTitle;
		}
	}
}