using UnityEngine;
using System;
using System.Collections.Generic;

[ScriptOrder(-101)]
public class ColourSchemeManager : MonoBehaviour
{
	public List<ColourScheme> Schemes;
	private int mChosenIndex = 0;

	static public ColourScheme sActiveColourScheme;
	static public Action<ColourScheme> OnSchemeSwitched;

	void Awake()
	{
		if (PlayerPrefsPlus.HasKey(PlayerPrefKeys.ColourSchemeIndex))
		{
			mChosenIndex = PlayerPrefsPlus.GetInt(PlayerPrefKeys.ColourSchemeIndex, 0);
		}
		SwitchScheme(mChosenIndex);
	}

	public void SwitchScheme(int newColourSchemeIndex)
	{
		mChosenIndex = newColourSchemeIndex;

		sActiveColourScheme = Schemes[mChosenIndex];

		if (OnSchemeSwitched != null)
		{
			OnSchemeSwitched(sActiveColourScheme);
		}

		PlayerPrefsPlus.SetInt(PlayerPrefKeys.ColourSchemeIndex, mChosenIndex);
		PlayerPrefsPlus.Save();
	}

	public bool IsActiveScheme(int colourSchemeIndex)
	{
		return mChosenIndex == colourSchemeIndex;
	}

#if UNITY_EDITOR
	public void ClearList()
	{
		Schemes.Clear();
	}

	public void RegisterScheme(ColourScheme newScheme)
	{
		Schemes.Add(newScheme);
	}
#endif // UNITY_EDITOR
}