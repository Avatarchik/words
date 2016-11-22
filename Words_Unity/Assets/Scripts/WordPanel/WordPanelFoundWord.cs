using UnityEngine;
using UnityEngine.UI;

public class WordPanelFoundWord : MonoBehaviour
{
	public Rigidbody2D RigidBodyRef;
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
		TextRef.text = word;
		mDelayedStartTime = Time.time + fallDelay;

#if UNITY_EDITOR
		name = TextRef.text;
#endif // UNITY_EDITOR
	}

	public void SetAsFoundWords(int wordsFound, float fallDelay)
	{
		if (wordsFound > 1)
		{
			TextRef.text = string.Format(FoundWordsFormat, wordsFound);
		}
		else
		{
			TextRef.text = FoundWordFormat;
		}

		mDelayedStartTime = Time.time + fallDelay;

#if UNITY_EDITOR
		name = TextRef.text;
#endif // UNITY_EDITOR
	}
}