using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
	public Text ScoreTextRef;
	public string ScoreFormat;

	public ScoreAddition ScoreAdditionRef;

	private int mScore;
	private int mTargetScore;

	public void AddScore(int scoreToAdd)
	{
		mTargetScore += scoreToAdd;
		UpdateText();

		ScoreAdditionRef.ShowScoreAddition(scoreToAdd);
	}

	void Update()
	{
		if (mScore < mTargetScore)
		{
			float delta = mTargetScore - mScore;
			int scoreToAdd = (int)(delta * 0.05f);

			if (scoreToAdd > 0)
			{
				mScore = Mathf.Clamp(mScore + scoreToAdd, mScore, mTargetScore);
			}
			else
			{
				mScore = mTargetScore;
			}

			UpdateText();
		}
	}

	public void Reset()
	{
		mScore = 0;
		mTargetScore = 0;

		UpdateText();

		ScoreAdditionRef.Reset();
	}

	private void UpdateText()
	{
		ScoreTextRef.text = string.Format(ScoreFormat, mScore);
	}
}