using UnityEngine.Analytics;
using System;
using System.Collections.Generic;

public class AnalyticsManager : SingletonMonoBehaviour<AnalyticsManager>
{
	public PuzzleManager PuzzleManagerRef;

	public void SendPuzzleComplete()
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

		Analytics.CustomEvent("PuzzleComplete", dict);
	}

	public void SendPuzzleOfTheDayComplete()
	{
		PuzzleState state = SaveGameManager.Instance.ActivePuzzleState;

		DateTime now = DateTime.Today;

		Dictionary<string, object> dict = new Dictionary<string, object>
		{
			{ "Day", now.Day },
			{ "Month", now.Month },
			{ "Year", now.Year },
			{ "Score", state.Score },
			{ "Time", state.TotalTimeInSeconds },
		};

		Analytics.CustomEvent("PuzzleComplete", dict);
	}
}