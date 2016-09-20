using UnityEngine;
using System;
using System.Collections.Generic;

[ScriptOrder(100)]
public class ColourSchemesManager : MonoBehaviour
{
	static private readonly string kChosenIndexKey = "ColourSchemeIndex";

	public List<ColourScheme> Schemes;
	private int mChosenIndex = 0;

	static public ColourScheme sActiveColourScheme;
	static public Action<ColourScheme> OnSchemeSwitched;

	void Awake()
	{
		bool hasLoadedKey = false;
		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mChosenIndex = PlayerPrefs.GetInt(kChosenIndexKey, 0);
			hasLoadedKey = true;
		}

		UpdateScheme(hasLoadedKey);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Q))
		{
			mChosenIndex = 0;
			UpdateScheme(true);
		}
		if (Input.GetKeyUp(KeyCode.W))
		{
			mChosenIndex = 1;
			UpdateScheme(true);
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			mChosenIndex = 2;
			UpdateScheme(true);
		}
		if (Input.GetKeyUp(KeyCode.R))
		{
			mChosenIndex = 3;
			UpdateScheme(true);
		}
	}

	private void UpdateScheme(bool saveChange)
	{
		sActiveColourScheme = Schemes[mChosenIndex];

		if (OnSchemeSwitched != null)
		{
			OnSchemeSwitched(sActiveColourScheme);
		}

		if (saveChange)
		{
			PlayerPrefs.SetInt(kChosenIndexKey, mChosenIndex);
			PlayerPrefs.Save();
		}
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