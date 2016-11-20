using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
	public Image ImageRef;

	[Range(0.1f, 2f)]
	public float FadeDuration = 2;

	private bool mIsFading;

	void Awake()
	{
		ImageRef.sprite = BackgroundPicker.sChosenBackground;
	}

	public void BeginFade(Action fadeOutFinishedCallback)
	{
		if (mIsFading)
		{
			return;
		}
		mIsFading = true;

		float halfFadeDuration = FadeDuration * 0.5f;
		StartCoroutine(Fade(0, 1, halfFadeDuration));					// Fade out
		StartCoroutine(Fade(halfFadeDuration, 0, halfFadeDuration));	// Fade in
		StartCoroutine(CallFadeOutCallback(halfFadeDuration, fadeOutFinishedCallback));
		StartCoroutine(ResetStatus(FadeDuration));
	}

	private IEnumerator Fade(float delay, float targetAlpha, float duration)
	{
		yield return new WaitForSeconds(delay);

		float startTime = Time.time;
		float endTime = Time.time + duration;
		float startAlpha = ImageRef.color.a;

		Color colour = ImageRef.color;

		while (Time.time < endTime)
		{
			float t = (Time.time - startTime) / duration;
			colour.a = Mathf.SmoothStep(startAlpha, targetAlpha, t);
			ImageRef.color = colour;
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator CallFadeOutCallback(float delay, Action fadeOutFinishedCallback)
	{
		yield return new WaitForSeconds(delay);

		if (fadeOutFinishedCallback != null)
		{
			fadeOutFinishedCallback();
		}
	}

	private IEnumerator ResetStatus(float delay)
	{
		yield return new WaitForSeconds(delay);
		mIsFading = false;
	}
}