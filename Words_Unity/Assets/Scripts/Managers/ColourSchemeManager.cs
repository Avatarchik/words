using UnityEngine;
using System;
using System.Collections.Generic;

[ScriptOrder(100)]
public class ColourSchemeManager : MonoBehaviour
{
	static private readonly string kChosenIndexKey = "ColourSchemeIndex";

	public List<ColourScheme> Schemes;
	private int mChosenIndex = 0;

	static public ColourScheme sActiveColourScheme;
	static public Action<ColourScheme> OnSchemeSwitched;

	void Awake()
	{
		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mChosenIndex = PlayerPrefs.GetInt(kChosenIndexKey, 0);
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

		PlayerPrefs.SetInt(kChosenIndexKey, mChosenIndex);
		PlayerPrefs.Save();
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