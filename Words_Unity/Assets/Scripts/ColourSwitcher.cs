using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public struct ColourPair
{
	public Color High;
	public Color Low;
}

public class ColourSwitcher : MonoBehaviour
{
	static private readonly string kChosenIndexKey = "ColourPairIndex";

	public List<ColourPair> Pairs;
	private int mChosenIndex = 0;

	static public Action<ColourPair> OnColourSwitched;

	void Start()
	{
		bool requireSave = false;

		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mChosenIndex = PlayerPrefs.GetInt("ColourPairIndex", 0);
			requireSave = true;
		}

		SetColour(requireSave);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			mChosenIndex = 0;
			SetColour(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			mChosenIndex = 1;
			SetColour(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			mChosenIndex = 2;
			SetColour(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha4))
		{
			mChosenIndex = 3;
			SetColour(true);
		}
	}

	private void SetColour(bool saveChange)
	{
		if (OnColourSwitched != null)
		{
			OnColourSwitched(Pairs[mChosenIndex]);
		}

		if (saveChange)
		{
			PlayerPrefs.SetInt(kChosenIndexKey, mChosenIndex);
			PlayerPrefs.Save();
		}
	}
}