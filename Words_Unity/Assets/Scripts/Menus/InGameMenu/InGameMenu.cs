using UnityEngine.UI;

public class InGameMenu : Menu, IMenu
{
	public Image PauseButton;

	public override void OnEnable()
	{
		base.OnEnable();

		PauseButton.gameObject.SetActive(true);
	}

	public override void OnDisable()
	{
		base.OnDisable();
	}

	public void HidePauseButton()
	{
		PauseButton.gameObject.SetActive(false);
	}
}