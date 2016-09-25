using UnityEngine;
using System.Collections.Generic;

public enum EMenuType
{
	Invalid = 0,

	MainMenu,
	InGame,

	Count,
}

public class MenuManager : MonoBehaviour
{
	static public MenuManager Instance { get; private set; }
	public List<Menu> Menus = new List<Menu>();

	private Menu mCurrentMenu;
	private Menu mPreviousMenu;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		mCurrentMenu = Menus.FirstItem();
		mPreviousMenu = null;
	}

	public void SwitchMenu(EMenuType newMenuType)
	{
		mCurrentMenu.Close();
		mPreviousMenu = mCurrentMenu;

		for (int menuIndex = 0; menuIndex < Menus.Count; ++menuIndex)
		{
			Menu menu = Menus[menuIndex];
			if (menu.MenuType == newMenuType)
			{
				mCurrentMenu = menu;
				mCurrentMenu.Open();
				break;
			}
		}
	}
}