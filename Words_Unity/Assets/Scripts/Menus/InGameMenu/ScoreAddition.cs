using UnityEngine;
using UnityEngine.UI;

public class ScoreAddition : MonoBehaviour
{
	public Text TextRef;

	[Range(0f, 5f)]
	public float FadeTime = 1f;
	public EasingType EaseType;

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
			t = EasingCurves.EaseOut(t, EaseType);
			TextRef.color = ColorHelper.Blend(StartColour, EndColour, t);
			mIsFading = timeNow < mFadeEndTime;
		}
	}

	public void ShowScoreAddition(int scoreChange)
	{
		TextRef.text = (scoreChange > 0) ? "+ " : "- ";
		TextRef.text += Mathf.Abs(scoreChange);

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