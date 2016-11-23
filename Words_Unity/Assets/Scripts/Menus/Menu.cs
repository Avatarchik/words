using UnityEngine;

public class Menu : ScreenStretchedUIMonoBehaviour
{
	public EMenuType MenuType;

	public void Open()
	{
		gameObject.SetActive(true);
	}

	public void Close()
	{
		gameObject.SetActive(false);
	}
}