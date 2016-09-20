using UnityEngine;
using System.Collections.Generic;

[ScriptOrder(101)]
public class PuzzleManager : MonoBehaviour
{
	static private readonly string kChosenIndexKey = "PuzzleIndex";

	public PuzzleLoader LoaderRef;

	public List<PuzzleContents> Puzzles;
	private int mChosenIndex = 0;

	void Awake()
	{
		bool hasLoadedKey = false;
		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mChosenIndex = PlayerPrefs.GetInt(kChosenIndexKey, 0);
			hasLoadedKey = true;
		}

		LoadPuzzle(hasLoadedKey);
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Alpha1))
		{
			mChosenIndex = 0;
			LoadPuzzle(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha2))
		{
			mChosenIndex = 1;
			LoadPuzzle(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha3))
		{
			mChosenIndex = 2;
			LoadPuzzle(true);
		}
		if (Input.GetKeyUp(KeyCode.Alpha4))
		{
			mChosenIndex = 3;
			LoadPuzzle(true);
		}
	}

	private void LoadPuzzle(bool saveChange)
	{
		LoaderRef.LoadPuzzle(Puzzles[mChosenIndex]);

		if (saveChange)
		{
			PlayerPrefs.SetInt(kChosenIndexKey, mChosenIndex);
			PlayerPrefs.Save();
		}
	}

#if UNITY_EDITOR
	public void ClearList()
	{
		Puzzles.Clear();
	}

	public void RegisterPuzzle(PuzzleContents newContents)
	{
		Puzzles.Add(newContents);
	}
#endif // UNITY_EDITOR
}