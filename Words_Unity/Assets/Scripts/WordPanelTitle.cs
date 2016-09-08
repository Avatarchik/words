using UnityEngine;
using UnityEngine.UI;

public class WordPanelTitle : MonoBehaviour
{
	public Text TextComp;
	public string TitleFormat;

	public void SetWordsLeftCount(int wordsLeft)
	{
		TextComp.text = string.Format(TitleFormat, wordsLeft);
	}
}