using UnityEngine;
using UnityEngine.EventSystems;

public class ClosePuzzleResetButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			MenuManager.Instance.CloseTemporaryMenu();
		}
	}
}