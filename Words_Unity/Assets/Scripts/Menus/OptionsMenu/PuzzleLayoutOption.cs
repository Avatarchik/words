using UnityEngine;
using UnityEngine.UI;

public class PuzzleLayoutOption : MonoBehaviour
{
	public Image LeftHandedLayout;
	public Image RightHandedLayout;

	public Color EnabledColour = Color.white;
	public Color DisabledColour = Color.white;

	void Awake()
	{
		int optionValue = PlayerPrefsPlus.GetInt(PlayerPrefKeys.PuzzleLayout, 1);
		UpdateUI(optionValue);
	}

	private void UpdateUI(int optionValue)
	{
		bool isRightHandedChosen = optionValue == 1;
		LeftHandedLayout.color = isRightHandedChosen ? DisabledColour : EnabledColour;
		RightHandedLayout.color = isRightHandedChosen ? EnabledColour : DisabledColour;
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