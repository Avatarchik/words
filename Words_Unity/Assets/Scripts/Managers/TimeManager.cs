using UnityEngine;
using UnityEngine.UI;

public class TimeManager : SingletonMonoBehaviour<TimeManager>
{
	public Text TextRef;
	public string TimeFormat;

	private bool mIsTicking;
	private float mCurrentTime;
	private int mCurrentTimeInSeconds;
	private int mLastUpdateTimeInSeconds;

	public void Start()
	{
		mIsTicking = true;
	}

	public void Stop()
	{
		mIsTicking = false;
	}

	public void Reset()
	{
		mIsTicking = false;
		mCurrentTime = 0;
		mCurrentTimeInSeconds = 0;
		mLastUpdateTimeInSeconds = 0;

		UpdateText();
	}

	public void SetTime(int minutes, int seconds)
	{
		mCurrentTime = (minutes * 60) + seconds;
		mCurrentTimeInSeconds = (int)mCurrentTime;
		UpdateText();
	}

	void Update()
	{
		if (mIsTicking)
		{
			mCurrentTime += Time.deltaTime;
			mCurrentTimeInSeconds = (int)mCurrentTime;

			if ((mCurrentTimeInSeconds - mLastUpdateTimeInSeconds) >= 1)
			{
				mLastUpdateTimeInSeconds = mCurrentTimeInSeconds;
				UpdateText();
			}
		}
	}

	private void UpdateText()
	{
		int minutes = mCurrentTimeInSeconds / 60;
		int seconds = mCurrentTimeInSeconds % 60;
		TextRef.text = string.Format(TimeFormat, minutes, seconds);
	}

	public int GetCurrentMinutes()
	{
		return mCurrentTimeInSeconds / 60;
	}

	public int GetCurrentSeconds()
	{
		return mCurrentTimeInSeconds % 60;
	}
}