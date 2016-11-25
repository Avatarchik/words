using UnityEngine;
using UnityEngine.EventSystems;

public class MenuStateReturnButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			MenuManager.Instance.ReturnToPreviousMenu();
		}
	}
}