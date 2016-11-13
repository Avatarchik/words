using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : Menu, IMenu
{
	public Image PauseButton;

	public HandedPositionPair[] HandedPositionPairs;

	public void OnEnable()
	{
		PauseButton.gameObject.SetActive(true);

		int layoutOption = PlayerPrefsPlus.GetInt(PlayerPrefKeys.PuzzleLayout, 1);
		foreach (HandedPositionPair pair in HandedPositionPairs)
		{
			pair.SwitchTo((HandedPosition.EHandedPositionType)layoutOption);
		}
	}

	public void OnDisable()
	{
	}

	public void HidePauseButton()
	{
		PauseButton.gameObject.SetActive(false);
	}
}