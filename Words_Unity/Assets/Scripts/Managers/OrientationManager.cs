using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class OrientationManager : SingletonMonoBehaviour<OrientationManager>
{
	public enum ePuzzleOrientation
	{
		Landscape = 0,
		Portrait,
	}

	public enum eDeviceOrientation
	{
		LandscapeLeft = 0,
		LandscapeRight,
		Portrait,
		PortraitUpsideDown,
	}

	public ScreenOrientation CurrentOrientation { get; private set; }

	public CanvasScaler MainCanvasScaler;

	public Vector2 LandscapeReferenceResolution;
	public Vector2 PortraitReferenceResolution;

	private List<IOrientationChangedNotifiee> mOrientationChangedNotifiees = new List<IOrientationChangedNotifiee>();

#if UNITY_EDITOR
	public bool ForceSetOrientation = false;
	public ScreenOrientation ForcedOrientation;
#endif // UNITY_EDITOR

	public HandedPositionSet[] HandedPositionSets;

	void Awake()
	{
#if !UNITY_EDITOR
		CurrentOrientation = Screen.orientation;
#else
		CurrentOrientation = ScreenOrientation.Landscape;
#endif
	}

	void Update()
	{
#if !UNITY_EDITOR
		if (CurrentOrientation != Screen.orientation)
		{
			CurrentOrientation = Screen.orientation;
			ChangeOrientation();
		}
#else
		if (ForceSetOrientation)
		{
			ForceSetOrientation = false;
			CurrentOrientation = ForcedOrientation;
			ChangeOrientation();
		}
#endif // !UNITY_EDITOR
	}

	private void ChangeOrientation()
	{
		if (IsCurrentlyLandscape())
		{
			MainCanvasScaler.referenceResolution = LandscapeReferenceResolution;
			MainCanvasScaler.matchWidthOrHeight = 0;
		}
		else
		{
			MainCanvasScaler.referenceResolution = PortraitReferenceResolution;
			MainCanvasScaler.matchWidthOrHeight = 1;
		}

		UpdateScreenSizeChanged();
		UpdateHandedPositionSets();
	}

	public bool IsCurrentlyLandscape()
	{
		bool isLandscape = false;

		isLandscape |= CurrentOrientation == ScreenOrientation.Landscape;
		isLandscape |= CurrentOrientation == ScreenOrientation.LandscapeLeft;
		isLandscape |= CurrentOrientation == ScreenOrientation.LandscapeRight;

		return isLandscape;
	}

	public void RegisterForNotification(IOrientationChangedNotifiee notifiee)
	{
		mOrientationChangedNotifiees.Add(notifiee);
		UpdateScreenSizeChanged(); // TODO - this is bad!
		UpdateHandedPositionSets(); // TODO - this is bad!
	}

	public void UnregisterForNotification(IOrientationChangedNotifiee notifiee)
	{
		mOrientationChangedNotifiees.Remove(notifiee);
	}

	private void UpdateScreenSizeChanged()
	{
		foreach (IOrientationChangedNotifiee notifiee in mOrientationChangedNotifiees)
		{
			notifiee.OnScreenSizeChanged(MainCanvasScaler.referenceResolution);
		}
	}

	private void UpdateHandedPositionSets()
	{
		int layoutOption = 0;
		if (IsCurrentlyLandscape())
		{
			layoutOption = PlayerPrefsPlus.GetInt(PlayerPrefKeys.PuzzleLandscapeLayout, GlobalSettings.Instance.DefaultPuzzleLandscapeLayout);
		}
		else
		{
			layoutOption = (int)EHandedPositionType.Top + PlayerPrefsPlus.GetInt(PlayerPrefKeys.PuzzlePortraitLayout, GlobalSettings.Instance.DefaultPuzzlePortraitLayout);
		}

		EHandedPositionType positionType = (EHandedPositionType)(layoutOption);

		foreach (HandedPositionSet pair in HandedPositionSets)
		{
			pair.SwitchTo(positionType);
		}
	}
}