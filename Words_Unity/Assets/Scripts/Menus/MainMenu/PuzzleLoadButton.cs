using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleLoadButton : UIMonoBehaviour
	, IPointerClickHandler
{
	public Text TextRef;
	public string TextFormat;

	private MainMenu mMainMenuRef;
	private PuzzleManager mPuzzleManagerRef;
	private int mPuzzleIndex;

	public void Initialise(MainMenu mainMenuRef, PuzzleManager puzzleManagerRef, int puzzleIndex)
	{
		mMainMenuRef = mainMenuRef;

		mPuzzleManagerRef = puzzleManagerRef;
		mPuzzleIndex = puzzleIndex - 1;

		TextRef.text = string.Format(TextFormat, puzzleIndex);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			mPuzzleManagerRef.LoadPuzzle(mPuzzleIndex);
			mMainMenuRef.gameObject.SetActive(false);
		}
	}
}