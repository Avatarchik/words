using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleLoadButton : UIMonoBehaviour, IPointerClickHandler
{
	public Text TextRef;
	public string TextFormat;

	private PuzzleManager mPuzzleManagerRef;

	private int mPuzzleSize;
	private int mPuzzleIndex;

	public void Initialise(PuzzleManager puzzleManagerRef, int puzzleSize, int puzzleIndex)
	{
		mPuzzleManagerRef = puzzleManagerRef;

		mPuzzleSize = puzzleSize;
		mPuzzleIndex = puzzleIndex - 1;

		int wordCount = mPuzzleManagerRef.GetWordCountForPuzzle(puzzleSize, mPuzzleIndex);

		TextRef.text = string.Format(TextFormat, puzzleIndex, wordCount);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			mPuzzleManagerRef.OpenPuzzle(mPuzzleSize, mPuzzleIndex);

			TimeManager.Instance.Reset();
			ScoreManager.Instance.Reset();

			MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitched);
		}
	}

	private void OnMenuSwitched()
	{
		TimeManager.Instance.Start();
	}
}