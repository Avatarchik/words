using UnityEngine;
using System.Collections.Generic;

public enum EMenuType
{
	Invalid = 0,

	MainMenu,
	InGameMenu,
	PauseMenu,

	Count,
}

public class MenuManager : MonoBehaviour
{
	static public MenuManager Instance { get; private set; }

	public ScreenFade ScreenFaderRef;

	public List<Menu> Menus = new List<Menu>();

	private Menu mCurrentMenu;

	private bool mIsFading;
	private EMenuType mNextMenuType;

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;

		mCurrentMenu = Menus.FirstItem();
		mCurrentMenu.Open();

		mIsFading = false;
		mNextMenuType = EMenuType.Invalid;
	}

	public void SwitchMenu(EMenuType nextMenuType)
	{
		if (mIsFading)
		{
			return;
		}

		mIsFading = true;
		mNextMenuType = nextMenuType;
		ScreenFaderRef.BeginFade(OnFadeOutFinished);
	}

	private void OnFadeOutFinished()
	{
		mCurrentMenu.Close();

		for (int menuIndex = 0; menuIndex < Menus.Count; ++menuIndex)
		{
			Menu menu = Menus[menuIndex];
			if (menu.MenuType == mNextMenuType)
			{
				mCurrentMenu = menu;
				mCurrentMenu.Open();
				break;
			}
		}

		mIsFading = false;
	}
}