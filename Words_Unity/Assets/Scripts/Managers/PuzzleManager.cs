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

	private int mCurrentPuzzleIndex = 0;
	private int mCurrentPuzzleDimension;

	void Awake()
	{
		if (PlayerPrefs.HasKey(kChosenIndexKey))
		{
			mCurrentPuzzleIndex = PlayerPrefs.GetInt(kChosenIndexKey, 0);
		}
	}

	public void OpenPuzzle(int puzzleDimension, int puzzleIndexToLoad)
	{
		mCurrentPuzzleIndex = puzzleIndexToLoad;

		LoaderRef.gameObject.SetActive(true);

		mCurrentPuzzleDimension = puzzleDimension;
		PuzzleContents contents = GetContentsFor(puzzleDimension, puzzleIndexToLoad);
		LoaderRef.LoadPuzzle(contents);

		PlayerPrefs.SetInt(kChosenIndexKey, mCurrentPuzzleIndex);
		PlayerPrefs.Save();
	}

	public void ResetPuzzle()
	{
		PuzzleContents contents = GetContentsFor(mCurrentPuzzleDimension, mCurrentPuzzleIndex);
		LoaderRef.LoadPuzzle(contents);
	}

	public void ClosePuzzle()
	{
		LoaderRef.CleanUp();
		LoaderRef.gameObject.SetActive(false);
	}

	public int GetListIndex(int dimension)
	{
		return dimension - 4; // TODO - fix the literal
	}

	private PuzzleList GetListForDimension(int puzzleDimension)
	{
		int listIndex = GetListIndex(puzzleDimension);
		PuzzleList list = PuzzleLists[listIndex];
		return list;
	}

	private PuzzleContents GetContentsFor(int puzzleDimension, int puzzleIndex)
	{
		PuzzleList list = GetListForDimension(puzzleDimension);
		PuzzleContents contents = list.Puzzles[puzzleIndex];
		return contents;
	}

	public int GetWordCountForPuzzle(int puzzleDimension, int puzzleIndex)
	{
		PuzzleContents contents = GetContentsFor(puzzleDimension, puzzleIndex);
		int wordCount = contents.WordCount;
		return wordCount;
	}

#if UNITY_EDITOR
	public void InitialiseLists()
	{
		PuzzleLists = new List<PuzzleList>(13); // TODO - fix the literal

		for (int i = 0; i < 13; ++i) // TODO - fix the literal
		{
			PuzzleLists.Add(new PuzzleList());
		}
	}

	public void RegisterPuzzle(PuzzleContents newContents, int puzzleDimension)
	{
		int listIndex = GetListIndex(puzzleDimension);
		PuzzleList list = PuzzleLists[listIndex];
		list.Puzzles.Add(newContents);
	}
#endif // UNITY_EDITOR
}