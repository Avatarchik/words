using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourSchemeSwitchButton : UIMonoBehaviour, IPointerClickHandler
{
	public Text TextRef;
	public Image[] ColourExamples;

	private ColourSchemeManager mColourSchemeManagerRef;
	private int mSchemeIndex;

	public void Initialise(ColourSchemeManager colourSchemeManagerRef, int schemeIndex)
	{
		mColourSchemeManagerRef = colourSchemeManagerRef;
		mSchemeIndex = schemeIndex - 1;

		ColourScheme scheme = colourSchemeManagerRef.Schemes[mSchemeIndex];
		TextRef.text = scheme.Name;
		SetupColourExamples(scheme);
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
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			mColourSchemeManagerRef.SwitchScheme(mSchemeIndex);
		}
	}
}