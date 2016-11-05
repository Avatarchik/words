using UnityEngine.EventSystems;

public class CompleteContinueButton : UIMonoBehaviour, IPointerClickHandler
{
	public PuzzleManager PuzzleManagerRef;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			PuzzleManagerRef.ClosePuzzle();
			MenuManager.Instance.SwitchMenu(EMenuType.PuzzleSelectionMenu, OnMenuSwitched);
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