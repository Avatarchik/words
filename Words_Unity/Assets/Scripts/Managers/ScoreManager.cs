using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
	public Text ScoreTextRef;
	public string ScoreFormat;

	public ScoreAddition ScoreAdditionRef;

	private int mScore;

	public void AddScore(int scoreToAdd)
	{
		mScore += scoreToAdd;
		UpdateText();

		ScoreAdditionRef.ShowScoreAddition(scoreToAdd);
	}

	public void Reset()
	{
		mScore = 0;
		UpdateText();

		ScoreAdditionRef.Reset();
	}

	private void UpdateText()
	{
		ScoreTextRef.text = string.Format(ScoreFormat, mScore);
	}
}