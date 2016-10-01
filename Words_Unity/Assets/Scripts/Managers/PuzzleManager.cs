using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class PuzzleList
{
	public List<PuzzleContents> Puzzles = new List<PuzzleContents>();
}

[ScriptOrder(101)]
public class PuzzleManager : MonoBehaviour
{
	static private readonly string kChosenIndexKey = "PuzzleIndex";

	public PuzzleLoader LoaderRef;

	public List<PuzzleList> PuzzleLists;

	private int mCurrentPuzzleSize;
	private int mCurrentPuzzleIndex;

	void Awake()
	{
		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mCurrentPuzzleIndex = PlayerPrefs.GetInt(kChosenIndexKey, 0);
		}
	}

	public void OpenPuzzle(int puzzleSize, int puzzleIndexToLoad)
	{
		mCurrentPuzzleIndex = puzzleIndexToLoad;

		LoaderRef.gameObject.SetActive(true);

		mCurrentPuzzleSize = puzzleSize;
		PuzzleContents contents = GetContentsFor(puzzleSize, puzzleIndexToLoad);
		LoaderRef.LoadPuzzle(contents);

		PlayerPrefs.SetInt(kChosenIndexKey, mCurrentPuzzleIndex);
		PlayerPrefs.Save();
	}

	public void ResetPuzzle()
	{
		PuzzleContents contents = GetContentsFor(mCurrentPuzzleSize, mCurrentPuzzleIndex);
		LoaderRef.LoadPuzzle(contents);
	}

	public void ClosePuzzle()
	{
		LoaderRef.CleanUp();
		LoaderRef.gameObject.SetActive(false);
	}

	public int GetListIndex(int puzzleSize)
	{
		return puzzleSize - GlobalSettings.PuzzleSizeMin;
	}

	private PuzzleList GetListForPuzzleSize(int puzzleSize)
	{
		int listIndex = GetListIndex(puzzleSize);
		PuzzleList list = PuzzleLists[listIndex];
		return list;
	}

	private PuzzleContents GetContentsFor(int puzzleSize, int puzzleIndex)
	{
		PuzzleList list = GetListForPuzzleSize(puzzleSize);
		PuzzleContents contents = list.Puzzles[puzzleIndex];
		return contents;
	}

	public int GetWordCountForPuzzle(int puzzleSize, int puzzleIndex)
	{
		PuzzleContents contents = GetContentsFor(puzzleSize, puzzleIndex);
		int wordCount = contents.WordCount;
		return wordCount;
	}

#if UNITY_EDITOR
	public void InitialiseLists()
	{
		PuzzleLists = new List<PuzzleList>(GlobalSettings.PuzzleSizeDifference);
		for (int listIndex = 0; listIndex < GlobalSettings.PuzzleSizeDifference; ++listIndex)
		{
			PuzzleLists.Add(new PuzzleList());
		}
	}

	public void RegisterPuzzle(PuzzleContents newContents, int puzzleSize)
	{
		int listIndex = GetListIndex(puzzleSize);
		PuzzleList list = PuzzleLists[listIndex];
		list.Puzzles.Add(newContents);
	}
#endif // UNITY_EDITOR
}