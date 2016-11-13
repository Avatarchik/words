using UnityEngine;
using UnityEngine.EventSystems;

public class ProgressResetButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			SaveGameManager.Instance.ResetAll();
			MenuManager.Instance.CloseTemporaryMenu();
		}
	}
}