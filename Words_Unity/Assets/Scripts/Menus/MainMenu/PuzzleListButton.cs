using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PuzzleListButton : UIMonoBehaviour, IPointerClickHandler
{
	public Text TextRef;
	public string TextFormat;

	private int mPuzzleDimension;

	public void Initialise(int puzzleDimension)
	{
		mPuzzleDimension = puzzleDimension;
		TextRef.text = string.Format(TextFormat, puzzleDimension);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			MenuManager.Instance.SwitchMenu(EMenuType.PuzzleSelectionMenu, OnMenuSwitched);
		}
	}

	private void OnMenuSwitched()
	{
		PuzzleSelectionMenu selectionMenu = MenuManager.Instance.CurrentMenu as PuzzleSelectionMenu;
		if (selectionMenu)
		{
			selectionMenu.Initialise(mPuzzleDimension);
		}
	}
}