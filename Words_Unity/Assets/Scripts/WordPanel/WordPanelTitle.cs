using UnityEngine;
using UnityEngine.UI;

public class WordPanelTitle : MonoBehaviour
{
	public Text TextRef;
	public string TitleFormat;
	public string OneWordLeftTitle;
	public string AllFoundTitle;
	public string ForEffectTitleFormat;

	private bool mIsForEffect;

	void Update()
	{
		if (mIsForEffect)
		{
			if (transform.localPosition.y < -700)
			{
				Destroy(gameObject);
			}
		}
	}

	public void SetTitle(int wordsLeft)
	{
		if (wordsLeft > 1)
		{
			TextRef.text = string.Format(TitleFormat, wordsLeft);
		}
		else if (wordsLeft == 1)
		{
			TextRef.text = OneWordLeftTitle;
		}
		else
		{
			TextRef.text = AllFoundTitle;
		}
	}

	public void WordsRemoved(int wordsRemoved, int nowRemaining)
	{
		WordPanelTitle titleCopy = Instantiate(this, transform.parent, true) as WordPanelTitle;
		titleCopy.gameObject.AddComponent<Rigidbody2D>();
		titleCopy.MarkAsForEffect(wordsRemoved);

		SetTitle(nowRemaining);
	}

	private void MarkAsForEffect(int wordsRemoved)
	{
		mIsForEffect = true;
		TextRef.text = string.Format(ForEffectTitleFormat, wordsRemoved);
	}
}