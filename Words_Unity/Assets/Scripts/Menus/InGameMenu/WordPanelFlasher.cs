using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WordPanelFlasher : SingletonMonoBehaviour<WordPanelFlasher>
{
	public enum EFlashReason
	{
		Found,
		NotFound,
		AlreadyFound,
		WrongInstance,
	}

	public float FlashTime = 1f;
	public EasingType EaseType = EasingType.Sine;

	private Image mImageRef;

	private bool mIsFlashing;

	void Awake()
	{
		mImageRef = GetComponent<Image>();
		mIsFlashing = false;
	}

	public void Flash(EFlashReason reason)
	{
		if (!mIsFlashing)
		{
			mIsFlashing = true;

			Color flashColour = ColorHelper.From255(255, 255, 255, 0);
			bool foundFlashColour = true;

			switch (reason)
			{
				case EFlashReason.Found: flashColour = GlobalSettings.Instance.WordFoundColour; break;
				case EFlashReason.NotFound: flashColour = GlobalSettings.Instance.WordNotFoundColour; break;
				case EFlashReason.AlreadyFound: flashColour = GlobalSettings.Instance.WordAlreadyFoundColour; break;
				case EFlashReason.WrongInstance: flashColour = GlobalSettings.Instance.WordWrongInstanceColour; break;
				default: foundFlashColour = false; break;
			}

			flashColour = ColorHelper.SetAlpha255(flashColour, 128);

			if (foundFlashColour)
			{
				StartCoroutine(FlashInternal(flashColour));
			}
		}
	}

	private IEnumerator FlashInternal(Color flashColour)
	{
		Color originalColour = mImageRef.color;

		WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();

		float t = 0;
		float halfFlashTime = FlashTime / 2;
		float endTime = Time.time + halfFlashTime;

		while (Time.time < endTime)
		{
			t = (endTime - Time.time) / halfFlashTime;
			t = Mathf.Clamp01(t);
			t = EasingCurves.EaseOut(t, EaseType);

			mImageRef.color = ColorHelper.Blend(originalColour, flashColour, t);
			yield return endOfFrame;
		}

		t = 1;
		endTime = Time.time + halfFlashTime;

		while (Time.time < endTime)
		{
			t = (endTime - Time.time) / halfFlashTime;
			t = Mathf.Clamp01(t);
			t = EasingCurves.EaseOut(t, EaseType);

			mImageRef.color = ColorHelper.Blend(flashColour, originalColour, t);
			yield return endOfFrame;
		}

		
		mImageRef.color = originalColour;
		mIsFlashing = false;
	}
}