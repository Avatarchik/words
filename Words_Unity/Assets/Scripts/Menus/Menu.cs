using UnityEngine;

public class Menu : MonoBehaviour
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