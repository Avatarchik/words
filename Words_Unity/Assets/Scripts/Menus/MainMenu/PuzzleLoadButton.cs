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

	public PuzzleManager PuzzleManagerRef { get; private set; }

	private int mPuzzleSize;
	private int mPuzzleIndex;

	private PuzzleState mCurrentState;

	public void Initialise(PuzzleManager puzzleManagerRef, int puzzleSize, int puzzleIndex)
	{
		PuzzleManagerRef = puzzleManagerRef;

		mPuzzleSize = puzzleSize;
		mPuzzleIndex = puzzleIndex;

		SerializableGuid puzzleGuid = PuzzleManagerRef.GetGuidForPuzzle(puzzleSize, mPuzzleIndex);
		mCurrentState = SaveGameManager.Instance.GetPuzzleStateFor(puzzleGuid);

		int wordCount = PuzzleManagerRef.GetWordCountForPuzzle(puzzleGuid);

		TitleRef.text = string.Format(TitleFormat, puzzleIndex + 1, wordCount);

		TimeRef.text = string.Format(TimeFormat, mCurrentState.TimeMins, mCurrentState.TimeSeconds, "-");
		ScoreRef.text = string.Format(ScoreFormat, mCurrentState.Score, "-");

		float percentageComplete = Mathf.Clamp(mCurrentState.PercentageComplete, 0, 100);

		Vector2 progressBarSizeDelta = ProgressBarFillerRef.sizeDelta;
		progressBarSizeDelta.x = Mathf.Lerp(0, ProgressBarRef.rect.width, percentageComplete / 100);
		ProgressBarFillerRef.sizeDelta = progressBarSizeDelta;

		ProgressBarPercentageRef.text = string.Format(ProgressBarPercentageFormat, percentageComplete);

		TickRef.gameObject.SetActive(mCurrentState.IsCompleted);
	}

	public void Reinitialise()
	{
		Initialise(PuzzleManagerRef, mPuzzleSize, mPuzzleIndex);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (!mCurrentState.IsCompleted)
			{
				TimeManager.Instance.Reset();
				ScoreManager.Instance.Reset();

				PuzzleManagerRef.OpenPuzzle(mPuzzleSize, mPuzzleIndex);
				MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitchedToInGame);
			}
			else
			{
				MenuManager.Instance.SwitchTemporaryMenu(EMenuType.PuzzleResetMenu, OnMenuSwitchedToResetQuestion);
			}
		}
	}

	private void OnMenuSwitchedToInGame()
	{
		TimeManager.Instance.Start();
	}

	private void OnMenuSwitchedToResetQuestion()
	{
		PuzzleResetMenu resetMenu = MenuManager.Instance.TemporaryMenu as PuzzleResetMenu;
		if (resetMenu)
		{
			SerializableGuid puzzleGuid = PuzzleManagerRef.GetGuidForPuzzle(mPuzzleSize, mPuzzleIndex);
			resetMenu.InitialiseForPuzzle(puzzleGuid, this);
		}
	}
}