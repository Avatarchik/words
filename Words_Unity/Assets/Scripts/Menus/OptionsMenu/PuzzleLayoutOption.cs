using UnityEngine;
using UnityEngine.UI;

public class PuzzleLayoutOption : MonoBehaviour
{
	public Image LeftHandedLayout;
	public Image RightHandedLayout;

	private Color mEnabledColour;
	private Color mDisabledColour;

	void Awake()
	{
		mEnabledColour = GlobalSettings.Instance.UIHightlightColour;
		mDisabledColour = GlobalSettings.Instance.UIDisabledHighlightColour;

		int optionValue = PlayerPrefsPlus.GetInt(PlayerPrefKeys.PuzzleLayout, 1);
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
		PlayerPrefsPlus.SetInt(PlayerPrefKeys.PuzzleLayout, 0);
		PlayerPrefsPlus.Save();
	}

	public void OnRightHandedSelected()
	{
		UpdateUI(1);
		PlayerPrefsPlus.SetInt(PlayerPrefKeys.PuzzleLayout, 1);
		PlayerPrefsPlus.Save();
	}
}