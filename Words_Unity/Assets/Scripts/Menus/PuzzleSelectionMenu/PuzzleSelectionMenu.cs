using UnityEngine;
using System.Collections.Generic;

public class PuzzleSelectionMenu : Menu, IMenu
{
	public GameObject PuzzleLoadButtonPrefab;

	public PuzzleManager PuzzleManagerRef;

	public RectTransform PuzzlesRoot;
	private List<RectTransform> mPuzzles;

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
		if (mPuzzles != null)
		{
			for (int puzzleIndex = 0; puzzleIndex < mPuzzles.Count; ++puzzleIndex)
			{
				Destroy(mPuzzles[puzzleIndex].gameObject);
			}
		}
	}

	public void Initialise(int puzzleDimension)
	{
		int puzzleCount = PuzzleManagerRef.PuzzleLists[puzzleDimension].Puzzles.Count;
		mPuzzles = new List<RectTransform>(puzzleCount);

		int columnIndex = 0;
		int rowIndex = 0;
		for (int puzzleIndex = 1; puzzleIndex <= puzzleCount; ++puzzleIndex)
		{
			columnIndex = (puzzleIndex - 1) / 7; // TODO - fix the literal
			rowIndex = ((puzzleIndex - 1) % 7) + 1; // TODO - fix the literal

			GameObject newButtonGO = Instantiate(PuzzleLoadButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(PuzzlesRoot);
#if UNITY_EDITOR
			newButtonGO.name = "Puzzle #" + puzzleIndex;
#endif // UNITY_EDITOR

			PuzzleLoadButton puzzleLoadButton = newButtonGO.GetComponent<PuzzleLoadButton>();
			puzzleLoadButton.rectTransform.localPosition = new Vector3(116 * columnIndex, -32 * rowIndex, 0); // TODO - fix the literals

			puzzleLoadButton.Initialise(PuzzleManagerRef, puzzleDimension, puzzleIndex);

			mPuzzles.Add(puzzleLoadButton.rectTransform);
		}
	}
}