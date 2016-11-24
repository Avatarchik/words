using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
	public Text ScoreTextRef;
	public string ScoreFormat;

	public ScoreAddition ScoreAdditionRef;

	public int CurrentScore { get; private set; }
	private int mTargetScore;

	public void SetScore(int newScore)
	{
		CurrentScore = newScore;
		mTargetScore = CurrentScore;
		UpdateText();
	}

	public void AddScore(int scoreToAdd)
	{
		if (scoreToAdd != 0)
		{
			mTargetScore += scoreToAdd;
			mTargetScore = Mathf.Clamp(mTargetScore, 0, int.MaxValue);	
			UpdateText();

			ScoreAdditionRef.ShowScoreAddition(scoreToAdd);
		}
	}

	void Update()
	{
		if (CurrentScore != mTargetScore)
		{
			float delta = mTargetScore - CurrentScore;
			int scoreChange = (int)(delta * 0.05f);

			if (scoreChange > 0)
			{
				CurrentScore = Mathf.Clamp(CurrentScore + scoreChange, CurrentScore, mTargetScore);
			}
			else if (scoreChange < 0)
			{
				CurrentScore = Mathf.Clamp(CurrentScore + scoreChange, mTargetScore, CurrentScore);
			}
			else
			{
				CurrentScore = mTargetScore;
			}

			CurrentScore = Mathf.Clamp(CurrentScore, 0, int.MaxValue);

			UpdateText();
		}
	}

	public void Reset()
	{
		CurrentScore = 0;
		mTargetScore = 0;

		UpdateText();

		ScoreAdditionRef.Reset();
	}

	private void UpdateText()
	{
		ScoreTextRef.text = string.Format(ScoreFormat, CurrentScore);
	}
}