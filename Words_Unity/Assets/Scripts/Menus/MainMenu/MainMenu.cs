using UnityEngine;
using System.Collections.Generic;

public class MainMenu : Menu, IMenu
{
	public GameObject ColourSchemeButtonPrefab;

	public ColourSchemeManager ColourSchemeManagerRef;

	public RectTransform ColourSchemesRoot;

	private List<RectTransform> mColourSchemes;

	public override void OnEnable()
	{
		base.OnEnable();

		SetupColourSchemes();
	}

	public override void OnDisable()
	{
		base.OnDisable();

		for (int colourSchemeIndex = 0; colourSchemeIndex < mColourSchemes.Count; ++colourSchemeIndex)
		{
			Destroy(mColourSchemes[colourSchemeIndex].gameObject);
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
			schemeSwitchButton.rectTransform.localPosition = new Vector3(32, -96 - (GlobalSettings.Instance.ColourSchemeButtonSpacing * schemeIndex), 0);

			schemeSwitchButton.Initialise(ColourSchemeManagerRef, schemeIndex);

			mColourSchemes.Add(schemeSwitchButton.rectTransform);
		}
	}
}