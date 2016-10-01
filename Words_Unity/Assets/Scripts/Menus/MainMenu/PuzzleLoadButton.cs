using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleLoadButton : UIMonoBehaviour, IPointerClickHandler
{
	public Text TextRef;
	public string TextFormat;

	private PuzzleManager mPuzzleManagerRef;

	private int mPuzzleDimension;
	private int mPuzzleIndex;

	public void Initialise(PuzzleManager puzzleManagerRef, int puzzleDimension, int puzzleIndex)
	{
		mPuzzleManagerRef = puzzleManagerRef;

		mPuzzleDimension = puzzleDimension;
		mPuzzleIndex = puzzleIndex - 1;

		int wordCount = mPuzzleManagerRef.GetWordCountForPuzzle(puzzleDimension, mPuzzleIndex);

		TextRef.text = string.Format(TextFormat, puzzleIndex, wordCount);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			mPuzzleManagerRef.OpenPuzzle(mPuzzleDimension, mPuzzleIndex);
			MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu);
		}
	}
}