using UnityEngine;
using UnityEngine.EventSystems;

public class ResetPuzzleButton : MonoBehaviour, IPointerClickHandler
{
	public PuzzleManager PuzzleManagerRef;

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			PuzzleManagerRef.ResetPuzzle();
			TimeManager.Instance.Reset();
			ScoreManager.Instance.Reset();
			MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitched);
		}
	}

	private void OnMenuSwitched()
	{
		TimeManager.Instance.Start();
	}
}