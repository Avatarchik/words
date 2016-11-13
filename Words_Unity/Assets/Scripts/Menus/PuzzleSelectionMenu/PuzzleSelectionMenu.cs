using UnityEngine;
using System.Collections.Generic;

public class PuzzleSelectionMenu : Menu, IMenu
{
	public GameObject PuzzleLoadButtonPrefab;

	public PuzzleManager PuzzleManagerRef;

	public RectTransform PuzzlesRoot;
	private List<RectTransform> mPuzzles;

	static private int sLastChosenPuzzleSize;

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

	public void Initialise()
	{
		Initialise(sLastChosenPuzzleSize);
	}

	public void Initialise(int puzzleSize)
	{
		sLastChosenPuzzleSize = puzzleSize;

		int puzzleCount = PuzzleManagerRef.PuzzleLists[puzzleSize - GlobalSettings.PuzzleSizeMin].Puzzles.Count;
		mPuzzles = new List<RectTransform>(puzzleCount);

		for (int puzzleIndex = 0; puzzleIndex < puzzleCount; ++puzzleIndex)
		{
			GameObject newButtonGO = Instantiate(PuzzleLoadButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(PuzzlesRoot);
#if UNITY_EDITOR
			newButtonGO.name = "Puzzle #" + (puzzleIndex + 1);
#endif // UNITY_EDITOR

			PuzzleLoadButton puzzleLoadButton = newButtonGO.GetComponent<PuzzleLoadButton>();
			puzzleLoadButton.rectTransform.localPosition = new Vector3(0, -116 * puzzleIndex, 0); // TODO - fix the literals

			puzzleLoadButton.Initialise(PuzzleManagerRef, puzzleSize, puzzleIndex);

			mPuzzles.Add(puzzleLoadButton.rectTransform);
		}

		RectTransform rectTransform = PuzzlesRoot.GetComponent<RectTransform>();
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 116 * puzzleCount); // TODO - fix the literals
	}
}