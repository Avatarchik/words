using UnityEngine;
using System.Collections.Generic;

using SecPlayerPrefs;

public class SaveGameManager : SingletonMonoBehaviour<SaveGameManager>
{
	public PuzzleManager PuzzleManagerRef;

	private List<SerializableGuid> mPuzzleGuids;
	private List<PuzzleState> mPuzzleStates;

	private SerializableGuid mActivePuzzleGuid;
	[HideInInspector]
	public PuzzleState ActivePuzzleState;
	private int mActivePuzzleIndex;

	void Awake()
	{
		mPuzzleGuids = new List<SerializableGuid>();
		PuzzleManagerRef.PopulateGuidList(ref mPuzzleGuids);

		mPuzzleStates = new List<PuzzleState>(mPuzzleGuids.Count);
		foreach (SerializableGuid puzzleGuid in mPuzzleGuids)
		{
			SecureDataManager<PuzzleState> dm = new SecureDataManager<PuzzleState>("PuzzleState:" + puzzleGuid.Value);
			mPuzzleStates.Add(dm.Get());
		}
	}

	public PuzzleState SetActivePuzzle(SerializableGuid newActivePuzzleGuid)
	{
		mActivePuzzleGuid = newActivePuzzleGuid;

		mActivePuzzleIndex = mPuzzleGuids.FindIndex(guid => guid.Equals(mActivePuzzleGuid));
		ActivePuzzleState = mPuzzleStates[mActivePuzzleIndex];

		return ActivePuzzleState;
	}

	public void SaveActivePuzzleState()
	{
		SecureDataManager<PuzzleState> dm = new SecureDataManager<PuzzleState>("PuzzleState:" + mActivePuzzleGuid.Value);
		dm.Save(ActivePuzzleState);
		SecurePlayerPrefs.Save();
	}

	public void ResetActivePuzzleState()
	{
		ActivePuzzleState.Reset();
		SaveActivePuzzleState();
	}

	public PuzzleState GetPuzzleStateFor(SerializableGuid puzzleGuid)
	{
		int puzzleIndex = mPuzzleGuids.FindIndex(guid => guid.Equals(puzzleGuid));
		return mPuzzleStates[puzzleIndex];
	}
}