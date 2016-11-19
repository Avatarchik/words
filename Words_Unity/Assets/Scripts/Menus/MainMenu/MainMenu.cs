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

		const int puzzlesPerColumn = 5;
		int columnIndex = 0;
		int rowIndex = 0;
		for (int listIndex = 1; listIndex <= puzzleListsCount; ++listIndex)
		{
			columnIndex = (listIndex - 1) / puzzlesPerColumn;
			rowIndex = ((listIndex - 1) % puzzlesPerColumn) + 1;

			GameObject newButtonGO = Instantiate(PuzzleListButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(PuzzleListsRoot);
#if UNITY_EDITOR
			newButtonGO.name = "Puzzle List #" + listIndex;
#endif // UNITY_EDITOR

			PuzzleListButton puzzleListButton = newButtonGO.GetComponent<PuzzleListButton>();
			puzzleListButton.rectTransform.localPosition = new Vector3(116 * columnIndex, -GlobalSettings.Instance.TileSizeWithSpacing * rowIndex, 0); // TODO - fix the literals
			puzzleListButton.Initialise(listIndex + 3); // TODO - fix the literal

			mPuzzleLists.Add(puzzleListButton.rectTransform);
		}
	}

	private void SetupColourSchemes()
	{
		int schemeCount = ColourSchemeManagerRef.Schemes.Count;
		mColourSchemes = new List<RectTransform>(schemeCount);

		for (int schemeIndex = 0; schemeIndex < schemeCount; ++schemeIndex)
		{
			GameObject newButtonGO = Instantiate(ColourSchemeButtonPrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;
			newButtonGO.transform.SetParent(ColourSchemesRoot);

#if UNITY_EDITOR
			newButtonGO.name = string.Format("Scheme #{0} - {1}", schemeIndex, ColourSchemeManagerRef.Schemes[schemeIndex].Name);
#endif // UNITY_EDITOR

			ColourSchemeSwitchButton schemeSwitchButton = newButtonGO.GetComponent<ColourSchemeSwitchButton>();
			schemeSwitchButton.rectTransform.localPosition = new Vector3(0, GlobalSettings.Instance.TileSizeWithSpacing * (schemeIndex + 1), 0);

			schemeSwitchButton.Initialise(ColourSchemeManagerRef, schemeIndex);

			mColourSchemes.Add(schemeSwitchButton.rectTransform);
		}
	}
}