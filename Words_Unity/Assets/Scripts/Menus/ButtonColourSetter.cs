using UnityEngine;
using UnityEngine.UI;

public class ButtonColourSetter : MonoBehaviour
{
	public Color ButtonColour;
	private Button mButtonRef;

	public bool SetHighlightedColour = true;
	public bool SetPressedColour = true;

	void Awake()
	{
#if UNITY_ANDROID || UNITY_IOS
		SetHighlightedColour = false;
#endif // UNITY_ANDROID || UNITY_IOS

		mButtonRef = GetComponent<Button>();
		ODebug.AssertNull(mButtonRef);

		ColorBlock colourBlock = mButtonRef.colors;

		if (SetHighlightedColour)
		{
			colourBlock.highlightedColor = ButtonColour;
		}
		if (SetPressedColour)
		{
			colourBlock.pressedColor = ButtonColour;
		}

		mButtonRef.colors = colourBlock;
	}
}