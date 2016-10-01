using UnityEngine;
using System.Collections.Generic;

public class MainMenu : Menu, IMenu
{
	public GameObject PuzzleListButtonPrefab;
	public GameObject ColourSchemeButtonPrefab;

	public PuzzleManager PuzzleManagerRef;
	public ColourSchemeManager ColourSchemeManagerRef;

	public RectTransform PuzzleListsRoot;
	public RectTransform ColourSchemesRoot;

	private List<RectTransform> mPuzzleLists;
	private List<RectTransform> mColourSchemes;

	public void OnEnable()
	{
		SetupPuzzleLists();
		SetupColourSchemes();
	}

	public void OnDisable()
	{
		for (int listIndex = 0; listIndex < mPuzzleLists.Count; ++listIndex)
		{
			Destroy(mPuzzleLists[listIndex].gameObject);
		}

		for (int colourSchemeIndex = 0; colourSchemeIndex < mColourSchemes.Count; ++colourSchemeIndex)
		{
			Destroy(mColourSchemes[colourSchemeIndex].gameObject);
		}
	}

	private void SetupPuzzleLists()
	{
		int puzzleListsCount = PuzzleManagerRef.PuzzleLists.Count;
		mPuzzleLists = new List<RectTransform>(puzzleListsCount);

		int columnIndex = 0;
		int rowIndex = 0;
		for (int listIndex = 1; listIndex <= puzzleListsCount; ++listIndex)
		{
			columnIndex = (listIndex - 1) / 5; // TODO - fix the literal
			rowIndex = ((listIndex - 1) % 5) + 1; // TODO - fix the literal

			GameObject newButtonGO = Instantiate(PuzzleListButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(PuzzleListsRoot);
#if UNITY_EDITOR
			newButtonGO.name = "Puzzle List #" + listIndex;
#endif // UNITY_EDITOR

			PuzzleListButton puzzleListButton = newButtonGO.GetComponent<PuzzleListButton>();
			puzzleListButton.rectTransform.localPosition = new Vector3(116 * columnIndex, -32 * rowIndex, 0); // TODO - fix the literals
			puzzleListButton.Initialise(listIndex + 3); // TODO - fix the literal

			mPuzzleLists.Add(puzzleListButton.rectTransform);
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