using UnityEngine;
using UnityEngine.EventSystems;

public class ResumePuzzleButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitched);
		}
	}

	private void OnMenuSwitched()
	{
		TimeManager.Instance.Start();
	}
}