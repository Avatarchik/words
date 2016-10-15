using UnityEngine;
using UnityEngine.UI;

public class ButtonColourSetter : MonoBehaviour
{
	public Color ButtonColour;
	private Button mButtonRef;

	void Awake()
	{
		mButtonRef = GetComponent<Button>();
		ODebug.AssertNull(mButtonRef);

		ColorBlock colourBlock = mButtonRef.colors;
		colourBlock.highlightedColor = ButtonColour;
		colourBlock.pressedColor = ButtonColour;
		mButtonRef.colors = colourBlock;
	}
}