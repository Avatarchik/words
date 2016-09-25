using UnityEngine;
using UnityEngine.UI;
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

		for (int puzzleIndex = 1; puzzleIndex <= puzzleCount; ++puzzleIndex)
		{
			GameObject newButtonGO = Instantiate(PuzzleLoadButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(LevelsRoot);
#if UNITY_EDITOR
			newButtonGO.name = "Puzzle #" + puzzleIndex;
#endif // UNITY_EDITOR

			PuzzleLoadButton puzzleLoadButton = newButtonGO.GetComponent<PuzzleLoadButton>();
			puzzleLoadButton.rectTransform.localPosition = new Vector3(0, -32 * puzzleIndex, 0); // TODO - fix the literal

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

			schemeSwitchButton.Initialise(ColourSchemeManagerRef.Schemes[schemeIndex - 1]);

#if UNITY_EDITOR
			newButtonGO.name = string.Format("Scheme #{0} - {1}", schemeIndex, ColourSchemeManagerRef.Schemes[schemeIndex - 1].Name);
#endif // UNITY_EDITOR

			mColourSchemes.Add(schemeSwitchButton.rectTransform);
		}
	}
}