using UnityEngine.Analytics;
using System.Collections.Generic;

public class AnalyticsManager : SingletonMonoBehaviour<AnalyticsManager>
{
	public PuzzleManager PuzzleManagerRef;

	public void SendLevelComplete()
	{
		int puzzleSize;
		int puzzleIndex;
		PuzzleManagerRef.GetPuzzleSizeAndIDFor(PuzzleManager.sActivePuzzleGuid, out puzzleSize, out puzzleIndex);

		PuzzleState state = SaveGameManager.Instance.ActivePuzzleState;

		Dictionary<string, object> dict = new Dictionary<string, object>
		{
			{ "PuzzleSize", puzzleSize },
			{ "PuzzleIndexInSize", puzzleIndex },
			{ "Score", state.Score },
			{ "Time", state.TotalTimeInSeconds },
		};

		Analytics.CustomEvent("LevelComplete", dict);
	}
}