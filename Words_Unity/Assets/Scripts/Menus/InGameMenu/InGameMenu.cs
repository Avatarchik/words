using UnityEngine.UI;

public class InGameMenu : Menu, IMenu
{
	public Image PauseButton;

	public void OnEnable()
	{
		PauseButton.gameObject.SetActive(true);
	}

	public void OnDisable()
	{
	}

	public void HidePauseButton()
	{
		PauseButton.gameObject.SetActive(false);
	}
}