using UnityEngine;
using UnityEngine.UI;

public class WordPanelEntry : MonoBehaviour
{
	public Text TextRef;
	public RectTransform StrikeThrough;
	public bool HasBeenFound { get; private set; }

	private string _Word;
	public string Word
	{
		get
		{
			return _Word;
		}
		set
		{
			_Word = value;
			TextRef.text = _Word;
		}
	}

	void Awake()
	{
		HasBeenFound = false;
	}

	public void MarkWordAsFound()
	{
		HasBeenFound = true;

		StrikeThrough.gameObject.SetActive(true);
		StrikeThrough.sizeDelta = new Vector2(TextRef.preferredWidth * 1.2f, StrikeThrough.sizeDelta.y);
	}
}