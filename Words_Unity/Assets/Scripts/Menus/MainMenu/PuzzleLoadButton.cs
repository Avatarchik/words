using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleLoadButton : UIMonoBehaviour, IPointerClickHandler
{
	public Text TextRef;
	public string TextFormat;

	private PuzzleManager mPuzzleManagerRef;
	private int mPuzzleIndex;

	public void Initialise(PuzzleManager puzzleManagerRef, int puzzleIndex)
	{
		mPuzzleManagerRef = puzzleManagerRef;
		mPuzzleIndex = puzzleIndex - 1;

		TextRef.text = string.Format(TextFormat, puzzleIndex);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			mPuzzleManagerRef.OpenPuzzle(mPuzzleIndex);
			MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu);
		}
	}
}