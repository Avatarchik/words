using UnityEngine;

public class ResetProgressOption : MonoBehaviour
{
	public void OnResetButtonPressed()
	{
		MenuManager.Instance.SwitchTemporaryMenu(EMenuType.ProgressResetMenu);
	}
}