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

	public PuzzleLoader PuzzleLoaderRef;
	public WordPanel WordPanelRef;

	public List<PuzzleList> PuzzleLists;

	private int mCurrentPuzzleSize;
	private int mCurrentPuzzleIndex;

	void Awake()
	{
		if (PlayerPrefsPlus.HasKey(kChosenIndexKey))
		{
			mCurrentPuzzleIndex = PlayerPrefsPlus.GetInt(kChosenIndexKey, 0);
		}
	}

	public void OpenPuzzle(int puzzleSize, int puzzleIndexToLoad)
	{
		mCurrentPuzzleIndex = puzzleIndexToLoad;
		PlayerPrefsPlus.SetInt(kChosenIndexKey, mCurrentPuzzleIndex);
		PlayerPrefsPlus.Save();

		PuzzleLoaderRef.gameObject.SetActive(true);

		mCurrentPuzzleSize = puzzleSize;
		PuzzleContents contents = GetContentsFor(puzzleSize, puzzleIndexToLoad);
		PuzzleState state = SaveGameManager.Instance.SetActivePuzzle(contents.Guid);
		PuzzleLoaderRef.LoadPuzzle(contents);

		TimeManager.Instance.SetTime(state.TimeMins, state.TimeSeconds);
		ScoreManager.Instance.SetScore(state.Score);
	}

	public void ResetPuzzle()
	{
		SaveGameManager.Instance.ResetActivePuzzleState();
		PuzzleContents contents = GetContentsFor(mCurrentPuzzleSize, mCurrentPuzzleIndex);
		PuzzleLoaderRef.LoadPuzzle(contents);
	}

	public void ClosePuzzle()
	{
		PuzzleState state = SaveGameManager.Instance.ActivePuzzleState;
		state.Score = ScoreManager.Instance.CurrentScore;
		state.TimeMins = TimeManager.Instance.GetCurrentMinutes();
		state.TimeSeconds = TimeManager.Instance.GetCurrentSeconds();
		state.PercentageComplete = WordPanelRef.GetCompletePercentage();
		SaveGameManager.Instance.SaveActivePuzzleState();

		PuzzleLoaderRef.CleanUp();
		PuzzleLoaderRef.gameObject.SetActive(false);
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

	public SerializableGuid GetGuidForPuzzle(int puzzleSize, int puzzleIndex)
	{
		PuzzleContents contents = GetContentsFor(puzzleSize, puzzleIndex);
		return contents.Guid;
	}

	public void PopulateGuidList(ref List<SerializableGuid> mGuids)
	{
		mGuids.Clear();

		foreach (PuzzleList puzzleList in PuzzleLists)
		{
			foreach (PuzzleContents puzzleContents in puzzleList.Puzzles)
			{
				mGuids.Add(puzzleContents.Guid);
			}
		}
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