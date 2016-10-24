using UnityEngine;
using UnityEngine.EventSystems;

public class MenuStateChangeButton : UIMonoBehaviour, IPointerClickHandler
{
	public EMenuType TypeToSwitchTo;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (TypeToSwitchTo == EMenuType.Invalid)
			{
				ODebug.Log("Can't switch to an invalid type");
				return;
			}

			MenuManager.Instance.SwitchMenu(TypeToSwitchTo);
		}
	}
}