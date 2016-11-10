using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleLoadButton : UIMonoBehaviour, IPointerClickHandler
{
	public Text TitleRef;
	public string TitleFormat;
	public Text TimeRef;
	public string TimeFormat;
	public Text ScoreRef;
	public string ScoreFormat;
	public RectTransform ProgressBarRef;
	public RectTransform ProgressBarFillerRef;
	public Text ProgressBarPercentageRef;
	public string ProgressBarPercentageFormat;
	public Image TickRef;

	private PuzzleManager mPuzzleManagerRef;

	private int mPuzzleSize;
	private int mPuzzleIndex;

	public void Initialise(PuzzleManager puzzleManagerRef, int puzzleSize, int puzzleIndex)
	{
		mPuzzleManagerRef = puzzleManagerRef;

		mPuzzleSize = puzzleSize;
		mPuzzleIndex = puzzleIndex - 1;

		int wordCount = mPuzzleManagerRef.GetWordCountForPuzzle(puzzleSize, mPuzzleIndex);

		TitleRef.text = string.Format(TitleFormat, puzzleIndex, wordCount);

		SerializableGuid puzzleGuid = mPuzzleManagerRef.GetGuidForPuzzle(puzzleSize, mPuzzleIndex);
		PuzzleState currentState = SaveGameManager.Instance.GetPuzzleStateFor(puzzleGuid);

		TimeRef.text = string.Format(TimeFormat, currentState.TimeMins, currentState.TimeSeconds, "-");
		ScoreRef.text = string.Format(ScoreFormat, currentState.Score, "-");

		float percentageComplete = Mathf.Clamp(currentState.PercentageComplete, 0, 100);

		Vector2 progressBarSizeDelta = ProgressBarFillerRef.sizeDelta;
		progressBarSizeDelta.x = Mathf.Lerp(0, ProgressBarRef.rect.width, percentageComplete / 100);
		ProgressBarFillerRef.sizeDelta = progressBarSizeDelta;

		ProgressBarPercentageRef.text = string.Format(ProgressBarPercentageFormat, percentageComplete);

		TickRef.gameObject.SetActive(currentState.IsCompleted);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			TimeManager.Instance.Reset();
			ScoreManager.Instance.Reset();

			mPuzzleManagerRef.OpenPuzzle(mPuzzleSize, mPuzzleIndex);

			MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitched);
		}
	}

	private void OnMenuSwitched()
	{
		TimeManager.Instance.Start();
	}
}