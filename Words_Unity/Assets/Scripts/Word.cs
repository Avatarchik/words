using UnityEngine;
using UnityEngine.UI;

public class Word : MonoBehaviour
{
	public Text TextComp;
	public RectTransform StrikeThrough;
	public bool HasBeenFound { get; private set; }

	private string mText;

	void Awake()
	{
		HasBeenFound = false;
	}

	public void MarkWordAsFound()
	{
		HasBeenFound = true;

		StrikeThrough.gameObject.SetActive(true);
		StrikeThrough.sizeDelta = new Vector2(TextComp.preferredWidth * 1.2f, StrikeThrough.sizeDelta.y);
	}

	public string GetText()
	{
		return mText;
	}

	public void SetText(string text)
	{
		mText = text;
		TextComp.text = text;
	}
}