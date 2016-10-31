using UnityEngine;
using UnityEngine.UI;

public class WordPanelTitle : MonoBehaviour
{
	public Text TextRef;
	public string TitleFormat;
	public string AllFoundTitle;
	public string ForEffectTitleFormat;

	[HideInInspector]
	public bool IsForEffect = false;

	void Update()
	{
		if (IsForEffect)
		{
			if (transform.localPosition.y < -700)
			{
				Destroy(gameObject);
			}
		}
	}

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

	public void WordsRemoved(int wordsRemoved, int nowRemaining)
	{
		WordPanelTitle titleCopy = Instantiate(this, transform.parent, true) as WordPanelTitle;
		titleCopy.gameObject.AddComponent<Rigidbody2D>();
		titleCopy.MarkAsForEffect(wordsRemoved);

		SetTitle(nowRemaining);
	}

	private void MarkAsForEffect(int wordsRemoved)
	{
		IsForEffect = true;
		TextRef.text = string.Format(ForEffectTitleFormat, wordsRemoved);
	}
}