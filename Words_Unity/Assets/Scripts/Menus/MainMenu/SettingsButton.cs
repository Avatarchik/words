using UnityEngine;
using UnityEngine.EventSystems;

public class SettingsButton : MonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			MenuManager.Instance.SwitchMenu(EMenuType.OptionsMenu);
		}
	}
}