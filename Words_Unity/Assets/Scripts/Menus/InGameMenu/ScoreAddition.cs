using UnityEngine;
using UnityEngine.UI;

public class ScoreAddition : MonoBehaviour
{
	public Text TextRef;
	public string TextFormat;

	public float FadeTime = 1f;
	public Color StartColour = ColorHelper.SetAlpha(Color.black, 1);
	public Color EndColour = ColorHelper.SetAlpha(Color.black, 0);

	private bool mIsFading = false;
	private float mFadeStartTime = 0;
	private float mFadeEndTime = 0;

	void Update()
	{
		if (mIsFading)
		{
			float timeNow = Time.time;
			float t = (mFadeEndTime - timeNow) / (mFadeEndTime - mFadeStartTime);
			t = MathfHelper.Clamp01(t);
			TextRef.color = ColorHelper.Blend(StartColour, EndColour, t);
			mIsFading = timeNow < mFadeEndTime;
		}
	}

	public void ShowScoreAddition(int scoreAddition)
	{
		TextRef.text = string.Format(TextFormat, scoreAddition);
		TextRef.color = StartColour;
		mIsFading = true;
		mFadeStartTime = Time.time;
		mFadeEndTime = mFadeStartTime + FadeTime;
	}

	public void Reset()
	{
		TextRef.color = EndColour;
		mIsFading = false;
		mFadeStartTime = 0;
		mFadeEndTime = 0;
	}
}