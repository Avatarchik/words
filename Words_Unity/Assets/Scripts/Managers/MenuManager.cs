using UnityEngine;
using System;
using System.Collections.Generic;

public enum EMenuType
{
	Invalid = 0,

	MainMenu,
	InGameMenu,
	PauseMenu,
	PuzzleCompleteMenu,
	PuzzleSelectionMenu,
	WordDefinitionMenu,

	Count,
}

public class MenuManager : SingletonMonoBehaviour<MenuManager>
{
	public ScreenFade ScreenFaderRef;

	public List<Menu> Menus = new List<Menu>();

	public Menu CurrentMenu { get; private set; }
	public Menu TemporaryMenu { get; private set; }

	private bool mIsFading;
	private EMenuType mNextMenuType;
	private Action mOnMenuSwitchedCallback;

	void Awake()
	{
		foreach (Menu menu in Menus)
		{
			menu.gameObject.SetActive(false);
		}

		CurrentMenu = Menus.FirstItem();
		CurrentMenu.Open();

		mIsFading = false;
		mNextMenuType = EMenuType.Invalid;
	}

	public void SwitchMenu(EMenuType menuType, Action onMenuSwitchedCallback = null)
	{
		if (mIsFading)
		{
			return;
		}

		mIsFading = true;
		mNextMenuType = menuType;
		mOnMenuSwitchedCallback = onMenuSwitchedCallback;
		ScreenFaderRef.BeginFade(OnFadeOutFinished);
	}

	public void SwitchTemporaryMenu(EMenuType menuType, Action onMenuSwitchedCallback = null)
	{
		for (int menuIndex = 0; menuIndex < Menus.Count; ++menuIndex)
		{
			Menu menu = Menus[menuIndex];
			if (menu.MenuType == menuType)
			{
				TemporaryMenu = menu;
				TemporaryMenu.Open();

				if (onMenuSwitchedCallback != null)
				{
					onMenuSwitchedCallback();
				}

				break;
			}
		}
	}

	public void CloseTemporaryMenu(Action onMenuClosedCallback = null)
	{
		if (TemporaryMenu)
		{
			TemporaryMenu.Close();
			TemporaryMenu = null;

			if (onMenuClosedCallback != null)
			{
				onMenuClosedCallback();
			}
		}
	}

	private void OnFadeOutFinished()
	{
		if (TemporaryMenu)
		{
			TemporaryMenu.Close();
			TemporaryMenu = null;
		}

		CurrentMenu.Close();

		for (int menuIndex = 0; menuIndex < Menus.Count; ++menuIndex)
		{
			Menu menu = Menus[menuIndex];
			if (menu.MenuType == mNextMenuType)
			{
				CurrentMenu = menu;
				CurrentMenu.Open();

				if (mOnMenuSwitchedCallback != null)
				{
					mOnMenuSwitchedCallback();
				}

				break;
			}
		}

		mIsFading = false;
	}
}