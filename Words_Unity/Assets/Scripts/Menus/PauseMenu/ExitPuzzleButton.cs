using UnityEngine;
using UnityEngine.EventSystems;

public class ExitPuzzleButton : MonoBehaviour, IPointerClickHandler
{
	public PuzzleManager PuzzleManagerRef;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			PuzzleManagerRef.ClosePuzzle();

			if (PuzzleManagerRef.IsPuzzleOfTheDay)
			{
				MenuManager.Instance.SwitchMenu(EMenuType.MainMenu);
			}
			else
			{
				MenuManager.Instance.SwitchMenu(EMenuType.PuzzleSelectionMenu, OnMenuSwitched);
			}
		}
	}

	private void OnMenuSwitched()
	{
		PuzzleSelectionMenu selectionMenu = MenuManager.Instance.CurrentMenu as PuzzleSelectionMenu;
		if (selectionMenu)
		{
			selectionMenu.Initialise();
		}
	}
}