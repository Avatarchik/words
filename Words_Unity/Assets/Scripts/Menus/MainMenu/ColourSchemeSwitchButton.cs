using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ColourSchemeSwitchButton : UIMonoBehaviour, IPointerClickHandler
{
	public Button ButtonRef;
	public Text TextRef;
	public ColourSchemeManager ColourSchemeManagerRef;

	public ColourScheme Scheme;

	public Image[] ColourExamples;

	private int mSchemeIndex;

	static private Button sSelectedButton;

	void Awake()
	{
		mSchemeIndex = int.Parse(Scheme.name.Split('_')[0]);

		ColourScheme scheme = ColourSchemeManagerRef.Schemes[mSchemeIndex];
		TextRef.text = scheme.Name;
		SetupColourExamples(scheme);

		if (ColourSchemeManagerRef.IsActiveScheme(mSchemeIndex))
		{
			ColorBlockHelper.SetNormalColour(ButtonRef, GlobalSettings.Instance.UIHightlightColour);
			sSelectedButton = ButtonRef;
		}
		else
		{
			ColorBlockHelper.SetNormalColour(ButtonRef, Color.white);
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
		TextRef.gameObject.name = string.Format("{0}_Text", name);
#endif // UNITY_EDITOR
		TextRef.transform.transform.SetParent(transform.parent);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			ColourSchemeManagerRef.SwitchScheme(mSchemeIndex);

			if (sSelectedButton != null)
			{
				ColorBlockHelper.SetNormalColour(sSelectedButton, Color.white);
			}

			ColorBlockHelper.SetNormalColour(ButtonRef, GlobalSettings.Instance.UIHightlightColour);
			sSelectedButton = ButtonRef;
		}
	}
}