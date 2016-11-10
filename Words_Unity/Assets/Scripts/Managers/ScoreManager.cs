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
		mTargetScore += scoreToAdd;
		UpdateText();

		ScoreAdditionRef.ShowScoreAddition(scoreToAdd);
	}

	void Update()
	{
		if (CurrentScore < mTargetScore)
		{
			float delta = mTargetScore - CurrentScore;
			int scoreToAdd = (int)(delta * 0.05f);

			if (scoreToAdd > 0)
			{
				CurrentScore = Mathf.Clamp(CurrentScore + scoreToAdd, CurrentScore, mTargetScore);
			}
			else
			{
				CurrentScore = mTargetScore;
			}

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