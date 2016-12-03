using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PuzzleListButton : UIMonoBehaviour, IPointerClickHandler
{
	public Button ButtonRef;
	public PuzzleManager PuzzleManagerRef;

	public int PuzzleSize;
	public bool IsLastPlayedButton = false;

	private SerializableGuid mLastPuzzleGuid;

	void OnEnable()
	{
		if (IsLastPlayedButton)
		{
			mLastPuzzleGuid = PuzzleManager.sActivePuzzleGuid;
			ButtonRef.interactable = mLastPuzzleGuid != null && mLastPuzzleGuid != Guid.Empty;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && ButtonRef.interactable)
		{
			if (IsLastPlayedButton)
			{
				TimeManager.Instance.Reset();
				ScoreManager.Instance.Reset();

				PuzzleManagerRef.OpenPuzzle(mLastPuzzleGuid);
				MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitched);
			}
			else
			{
				MenuManager.Instance.SwitchMenu(EMenuType.PuzzleSelectionMenu, OnMenuSwitched);
			}
		}
	}

	private void OnMenuSwitched()
	{
		PuzzleSelectionMenu selectionMenu = MenuManager.Instance.CurrentMenu as PuzzleSelectionMenu;
		if (selectionMenu)
		{
			selectionMenu.Initialise(PuzzleSize);
		}
		else
		{
			if (IsLastPlayedButton)
			{
				TimeManager.Instance.Start();
			}
		}
	}
}