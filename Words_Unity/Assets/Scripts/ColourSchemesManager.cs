using UnityEngine;
using System;
using System.Collections.Generic;

[ScriptOrder(100)]
public class ColourSchemesManager : MonoBehaviour
{
	static private readonly string kChosenIndexKey = "ColourPairIndex";

	public List<ColourScheme> Schemes;
	private int mChosenIndex = 0;

	static public ColourScheme sActiveColourScheme;

	static public Action<ColourScheme> OnSchemeSwitched;

	void Awake()
	{
		bool requireSave = false;

		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mChosenIndex = PlayerPrefs.GetInt("ColourPairIndex", 0);
			requireSave = true;
		}

		UpdateScheme(requireSave);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			mChosenIndex = 0;
			UpdateScheme(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			mChosenIndex = 1;
			UpdateScheme(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			mChosenIndex = 2;
			UpdateScheme(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha4))
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