using UnityEngine;
using UnityEngine.UI;

public class InGameMenu : Menu, IMenu
{
	public Image PauseButton;

	public HandedPositionPair[] HandedPositionPairs;

	public void OnEnable()
	{
		PauseButton.gameObject.SetActive(true);

		// TODO: Temporary
		foreach (HandedPositionPair pair in HandedPositionPairs)
		{
			pair.SwitchTo(HandedPosition.EHandedPositionType.Right);
		}
	}

	public void OnDisable()
	{
	}

	public void HidePauseButton()
	{
		PauseButton.gameObject.SetActive(false);
	}

	void Update()
	{
		// TODO: Temporary
		if (Input.GetKeyUp(KeyCode.L))
		{
			foreach (HandedPositionPair pair in HandedPositionPairs)
			{
				pair.SwitchTo(HandedPosition.EHandedPositionType.Left);
			}
		}
		else if (Input.GetKeyUp(KeyCode.R))
		{
			foreach (HandedPositionPair pair in HandedPositionPairs)
			{
				pair.SwitchTo(HandedPosition.EHandedPositionType.Right);
			}
		}
	}
}