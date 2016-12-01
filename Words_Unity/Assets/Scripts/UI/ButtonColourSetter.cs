using UnityEngine;
using UnityEngine.UI;

public class ButtonColourSetter : MonoBehaviour
{
	private Button mButtonRef;

	public bool SetHighlightedColour = true;
	public bool SetPressedColour = true;

	void Awake()
	{
		mButtonRef = GetComponent<Button>();
		ODebug.AssertNull(mButtonRef);

		ColorBlock colourBlock = mButtonRef.colors;

		if (SetHighlightedColour)
		{
			colourBlock.highlightedColor = GlobalSettings.Instance.UIHightlightColour;
		}
		if (SetPressedColour)
		{
			colourBlock.pressedColor = GlobalSettings.Instance.UIHightlightColour;
		}

		mButtonRef.colors = colourBlock;
	}
}