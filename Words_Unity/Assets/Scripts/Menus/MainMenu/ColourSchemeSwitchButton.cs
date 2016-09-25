using UnityEngine.UI;

public class ColourSchemeSwitchButton : UIMonoBehaviour
{
	public Text TextRef;
	public Image[] ColourExamples;

	public void Initialise(ColourScheme scheme)
	{
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
}