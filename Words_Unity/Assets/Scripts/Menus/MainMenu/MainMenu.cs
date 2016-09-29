using UnityEngine;
using System.Collections.Generic;

public class MainMenu : Menu, IMenu
{
	public GameObject PuzzleLoadButtonPrefab;
	public GameObject ColourSchemeButtonPrefab;

	public PuzzleManager PuzzleManagerRef;
	public ColourSchemeManager ColourSchemeManagerRef;

	public RectTransform LevelsRoot;
	public RectTransform ColourSchemesRoot;

	private List<RectTransform> mLevels;
	private List<RectTransform> mColourSchemes;

	public void OnEnable()
	{
		SetupLevels();
		SetupColourSchemes();
	}

	public void OnDisable()
	{
		for (int levelIndex = 0; levelIndex < mLevels.Count; ++levelIndex)
		{
			Destroy(mLevels[levelIndex].gameObject);
		}

		for (int colourSchemeIndex = 0; colourSchemeIndex < mColourSchemes.Count; ++colourSchemeIndex)
		{
			Destroy(mColourSchemes[colourSchemeIndex].gameObject);
		}
	}

	private void SetupLevels()
	{
		int puzzleCount = PuzzleManagerRef.Puzzles.Count;
		mLevels = new List<RectTransform>(puzzleCount);

		int columnIndex = 0;
		int rowIndex = 0;
		for (int puzzleIndex = 1; puzzleIndex <= puzzleCount; ++puzzleIndex)
		{
			columnIndex = (puzzleIndex - 1) / 7; // TODO - fix the literal
			rowIndex = ((puzzleIndex - 1) % 7) + 1; // TODO - fix the literal

			GameObject newButtonGO = Instantiate(PuzzleLoadButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(LevelsRoot);
#if UNITY_EDITOR
			newButtonGO.name = "Puzzle #" + puzzleIndex;
#endif // UNITY_EDITOR

			PuzzleLoadButton puzzleLoadButton = newButtonGO.GetComponent<PuzzleLoadButton>();
			puzzleLoadButton.rectTransform.localPosition = new Vector3(116 * columnIndex, -32 * rowIndex, 0); // TODO - fix the literals

			puzzleLoadButton.Initialise(PuzzleManagerRef, puzzleIndex);

			mLevels.Add(puzzleLoadButton.rectTransform);
		}
	}

	private void SetupColourSchemes()
	{
		int schemeCount = ColourSchemeManagerRef.Schemes.Count;
		mColourSchemes = new List<RectTransform>(schemeCount);

		for (int schemeIndex = 1; schemeIndex <= schemeCount; ++schemeIndex)
		{
			GameObject newButtonGO = Instantiate(ColourSchemeButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(ColourSchemesRoot);

			ColourSchemeSwitchButton schemeSwitchButton = newButtonGO.GetComponent<ColourSchemeSwitchButton>();
			schemeSwitchButton.rectTransform.localPosition = new Vector3(0, 32 * schemeIndex, 0); // TODO - fix the literal

			schemeSwitchButton.Initialise(ColourSchemeManagerRef, schemeIndex);

#if UNITY_EDITOR
			newButtonGO.name = string.Format("Scheme #{0} - {1}", schemeIndex, ColourSchemeManagerRef.Schemes[schemeIndex - 1].Name);
#endif // UNITY_EDITOR

			mColourSchemes.Add(schemeSwitchButton.rectTransform);
		}
	}
}