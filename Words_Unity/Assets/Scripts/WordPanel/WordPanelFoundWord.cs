using UnityEngine;
using UnityEngine.UI;

public class WordPanelFoundWord : MonoBehaviour
{
	public Rigidbody2D RigidBodyRef;
	public Image HighlightRef;
	public Text TextRef;
	public string FoundWordFormat;
	public string FoundWordsFormat;

	private float mDelayedStartTime;
	private bool mHasStarted = false;

	void Update()
	{
		if (transform.localPosition.y < -700)
		{
			Destroy(gameObject);
		}

		if (!mHasStarted && (Time.time >= mDelayedStartTime))
		{
			mHasStarted = true;
			TextRef.enabled = true;
			RigidBodyRef.isKinematic = false;
		}
	}

	public void SetAsFoundWord(string word, float fallDelay)
	{
		UpdateText(word);
		mDelayedStartTime = Time.time + fallDelay;

#if UNITY_EDITOR
		name = TextRef.text;
#endif // UNITY_EDITOR
	}

	public void SetAsFoundWords(int wordsFound, float fallDelay)
	{
		if (wordsFound > 1)
		{
			UpdateText(string.Format(FoundWordsFormat, wordsFound));
		}
		else
		{
			UpdateText(FoundWordFormat);
		}

		mDelayedStartTime = Time.time + fallDelay;

		HighlightRef.enabled = false;

#if UNITY_EDITOR
		name = TextRef.text;
#endif // UNITY_EDITOR
	}

	private void UpdateText(string newText)
	{
		TextRef.text = newText;

		float textWidth = TextRef.preferredWidth;

		Vector2 textSizeDelta = TextRef.rectTransform.sizeDelta;
		textSizeDelta.x = textWidth;
		TextRef.rectTransform.sizeDelta = textSizeDelta;

		Vector2 highlightSizeDelta = HighlightRef.rectTransform.sizeDelta;
		highlightSizeDelta.x = textWidth + 64;
		HighlightRef.rectTransform.sizeDelta = highlightSizeDelta;
	}
}