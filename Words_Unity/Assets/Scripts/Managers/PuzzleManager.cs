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
	public PuzzleLoader PuzzleLoaderRef;
	public WordPanel WordPanelRef;

	public List<PuzzleList> PuzzleLists;

	private SerializableGuid mCurrentPuzzleGuid;

	void Awake()
	{
		if (PlayerPrefsPlus.HasKey(PlayerPrefKeys.CurrentPuzzleGuid))
		{
			mCurrentPuzzleGuid = PlayerPrefsPlus.GetString(PlayerPrefKeys.CurrentPuzzleGuid, string.Empty);
		}
	}

	public void OpenPuzzle(int puzzleSize, int puzzleIndex)
	{
		mCurrentPuzzleGuid = GetGuidForPuzzle(puzzleSize, puzzleIndex);

		PlayerPrefsPlus.SetString(PlayerPrefKeys.CurrentPuzzleGuid, mCurrentPuzzleGuid.Value);
		PlayerPrefsPlus.Save();

		PuzzleLoaderRef.gameObject.SetActive(true);

		PuzzleContents contents = GetContentsFor(mCurrentPuzzleGuid);
		PuzzleState state = SaveGameManager.Instance.SetActivePuzzle(contents.Guid);
		PuzzleLoaderRef.LoadPuzzle(contents);

		TimeManager.Instance.SetTime(state.TimeMins, state.TimeSeconds);
		ScoreManager.Instance.SetScore(state.Score);
	}

	public void ResetPuzzle()
	{
		SaveGameManager.Instance.ResetActivePuzzleState();
		PuzzleContents contents = GetContentsFor(mCurrentPuzzleGuid);
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
		return puzzleSize - GlobalSettings.Instance.PuzzleSizeMin;
	}

	private PuzzleContents GetContentsFor(SerializableGuid puzzleGuid)
	{
		foreach (PuzzleList puzzleList in PuzzleLists)
		{
			foreach (PuzzleContents puzzleContents in puzzleList.Puzzles)
			{
				if (puzzleContents.Guid == puzzleGuid)
				{
					return puzzleContents;
				}
			}
		}

		ODebug.Assert(false);
		return null;
	}

	public int GetWordCountForPuzzle(SerializableGuid puzzleGuid)
	{
		PuzzleContents contents = GetContentsFor(puzzleGuid);
		int wordCount = contents.WordCount;
		return wordCount;
	}

	public SerializableGuid GetGuidForPuzzle(int puzzleSize, int puzzleIndex)
	{
		PuzzleList puzzleList = PuzzleLists[puzzleSize - GlobalSettings.Instance.PuzzleSizeMin];
		PuzzleContents puzzleContents = puzzleList.Puzzles[puzzleIndex];
		return puzzleContents.Guid;
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
		PuzzleLists = new List<PuzzleList>(GlobalSettings.Instance.PuzzleSizeDifference);
		for (int listIndex = 0; listIndex < (GlobalSettings.Instance.PuzzleSizeDifference + 1); ++listIndex)
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