using UnityEngine.EventSystems;

public class CloseWordDefinitionButton : UIMonoBehaviour, IPointerClickHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			MenuManager.Instance.CloseTemporaryMenu(OnMenuClosed);
		}
	}

	private void OnMenuClosed()
	{
		TimeManager.Instance.Start();
	}
}