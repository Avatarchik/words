using UnityEngine.EventSystems;

public class PauseButton : UIMonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			TimeManager.Instance.Stop();
			MenuManager.Instance.SwitchMenu(EMenuType.PauseMenu);
		}
	}
}