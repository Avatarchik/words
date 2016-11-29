using UnityEngine.EventSystems;

public class PuzzleListButton : UIMonoBehaviour, IPointerClickHandler
{
	public int PuzzleSize;

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
			selectionMenu.Initialise(PuzzleSize);
		}
	}
}