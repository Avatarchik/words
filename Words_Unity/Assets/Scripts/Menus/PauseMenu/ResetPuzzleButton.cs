using UnityEngine;
using UnityEngine.EventSystems;

public class ResetPuzzleButton : UIMonoBehaviour
	, IPointerClickHandler
{
	public PuzzleManager PuzzleManagerRef;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			PuzzleManagerRef.ResetPuzzle();
			MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu);
		}
	}
}