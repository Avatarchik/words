using UnityEngine;
using UnityEngine.UI;

public class PuzzleLayoutOption : MonoBehaviour
{
	public enum ePuzzleOrientation
	{
		Landscape = 0,
		Portrait,
	}

	public ePuzzleOrientation Orientation;

	public Image LeftHandedLayout;
	public Image RightHandedLayout;

	private Color mEnabledColour;
	private Color mDisabledColour;

	private string mPlayerPrefsKey;

	void Awake()
	{
		mEnabledColour = GlobalSettings.Instance.UIHightlightColour;
		mDisabledColour = GlobalSettings.Instance.UIDisabledHighlightColour;

		int defaultValue;
		if (Orientation == ePuzzleOrientation.Landscape)
		{
			mPlayerPrefsKey = PlayerPrefKeys.PuzzleLandscapeLayout;
			defaultValue = GlobalSettings.Instance.DefaultPuzzleLandscapeLayout;
		}
		else
		{
			mPlayerPrefsKey = PlayerPrefKeys.PuzzlePortraitLayout;
			defaultValue = GlobalSettings.Instance.DefaultPuzzlePortraitLayout;
		}

		int optionValue = PlayerPrefsPlus.GetInt(PlayerPrefKeys.PuzzleLandscapeLayout, defaultValue);
		UpdateUI(optionValue);
	}

	private void UpdateUI(int optionValue)
	{
		bool isRightHandedChosen = optionValue == 1;
		LeftHandedLayout.color = isRightHandedChosen ? mDisabledColour : mEnabledColour;
		RightHandedLayout.color = isRightHandedChosen ? mEnabledColour : mDisabledColour;
	}

	public void OnLeftHandedSelected()
	{
		UpdateUI(0);
		PlayerPrefsPlus.SetInt(mPlayerPrefsKey, 0);
		PlayerPrefsPlus.Save();
	}

	public void OnRightHandedSelected()
	{
		UpdateUI(1);
		PlayerPrefsPlus.SetInt(mPlayerPrefsKey, 1);
		PlayerPrefsPlus.Save();
	}

	public void OnTopHandedSelected()
	{
		UpdateUI(0);
		PlayerPrefsPlus.SetInt(mPlayerPrefsKey, 0);
		PlayerPrefsPlus.Save();
	}

	public void OnBottomHandedSelected()
	{
		UpdateUI(1);
		PlayerPrefsPlus.SetInt(mPlayerPrefsKey, 1);
		PlayerPrefsPlus.Save();
	}
}