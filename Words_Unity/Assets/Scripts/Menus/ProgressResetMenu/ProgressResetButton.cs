using UnityEngine.EventSystems;

public class ProgressResetButton : UIMonoBehaviour, IPointerClickHandler
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