using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourSchemeSwitchButton : UIMonoBehaviour, IPointerClickHandler
{
	public Image ImageRef;
	public Text TextRef;

	public Color NonSelectedColour;
	public Color SelectedColour;

	public RectTransform ColourExamplesRoot;
	public Image[] ColourExamples;

	private ColourSchemeManager mColourSchemeManagerRef;
	private int mSchemeIndex;

	static private Image sSelectedButton;

	public void Initialise(ColourSchemeManager colourSchemeManagerRef, int schemeIndex)
	{
		mColourSchemeManagerRef = colourSchemeManagerRef;
		mSchemeIndex = schemeIndex - 1;

		ColourScheme scheme = colourSchemeManagerRef.Schemes[mSchemeIndex];
		TextRef.text = scheme.Name;
		SetupColourExamples(scheme);

		if (colourSchemeManagerRef.IsActiveScheme(mSchemeIndex))
		{
			ImageRef.color = SelectedColour;
			sSelectedButton = ImageRef;
		}
		else
		{
			ImageRef.color = NonSelectedColour;
		}
	}

	private void SetupColourExamples(ColourScheme scheme)
	{
		int examplesCount = ColourExamples.Length;
		for (int exampleIndex = 0; exampleIndex < examplesCount; ++exampleIndex)
		{
			float t = (1f / (examplesCount - 1)) * exampleIndex;
			t = MathfHelper.Clamp01(t);

			ColourExamples[exampleIndex].color = ColorHelper.Blend(scheme.High, scheme.Low, t);
		}

#if UNITY_EDITOR
		ColourExamplesRoot.name = string.Format("{0} (examples)", name);
#endif // UNITY_EDITOR
		ColourExamplesRoot.transform.SetParent(transform.parent);
	}

	void OnDestroy()
	{
		if (ColourExamplesRoot != null)
		{
			Destroy(ColourExamplesRoot.gameObject);
			ColourExamplesRoot = null;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			mColourSchemeManagerRef.SwitchScheme(mSchemeIndex);

			if (sSelectedButton != null)
			{
				sSelectedButton.color = NonSelectedColour;
			}
			ImageRef.color = SelectedColour;
			sSelectedButton = ImageRef;
		}
	}
}