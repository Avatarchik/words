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
		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mChosenIndex = PlayerPrefs.GetInt(kChosenIndexKey, 0);
		}
	}

	public void OpenPuzzle(int puzzleIndexToLoad)
	{
		mChosenIndex = puzzleIndexToLoad;

		LoaderRef.gameObject.SetActive(true);
		LoaderRef.LoadPuzzle(Puzzles[mChosenIndex]);

		PlayerPrefs.SetInt(kChosenIndexKey, mChosenIndex);
		PlayerPrefs.Save();
	}

	public void ResetPuzzle()
	{
		LoaderRef.LoadPuzzle(Puzzles[mChosenIndex]);
	}

	public void ClosePuzzle()
	{
		LoaderRef.CleanUp();
		LoaderRef.gameObject.SetActive(false);
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