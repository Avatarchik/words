using UnityEngine.UI;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
	public Text TextRef;
	public string ScoreFormat;

	private int mScore;

	public void AddScore(int scoreToAdd)
	{
		mScore += scoreToAdd;
		UpdateText();
	}

	public void Reset()
	{
		mScore = 0;
		UpdateText();
	}

	private void UpdateText()
	{
		TextRef.text = string.Format(ScoreFormat, mScore);
	}
}