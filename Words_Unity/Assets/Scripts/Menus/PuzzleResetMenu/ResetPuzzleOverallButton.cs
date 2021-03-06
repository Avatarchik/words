using UnityEngine;
using UnityEngine.EventSystems;

public class ResetPuzzleOverallButton : MonoBehaviour, IPointerClickHandler
{
	private SerializableGuid mPuzzleGuid;
	private PuzzleLoadButton mPuzzleLoadButton;

	public void InitialiseForPuzzle(SerializableGuid puzzleGuid, PuzzleLoadButton puzzleLoadButton)
	{
		mPuzzleGuid = puzzleGuid;
		mPuzzleLoadButton = puzzleLoadButton;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			SaveGameManager.Instance.ResetPuzzleState(mPuzzleGuid);
			mPuzzleLoadButton.Reinitialise();
			MenuManager.Instance.CloseTemporaryMenu(OnMenuClose);
		}
	}

	private void OnMenuClose()
	{
		TimeManager.Instance.Reset();
		ScoreManager.Instance.Reset();

		mPuzzleLoadButton.PuzzleManagerRef.OpenPuzzle(mPuzzleGuid);
		MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitchedToInGame);
	}

	private void OnMenuSwitchedToInGame()
	{
		TimeManager.Instance.Start();
	}
}