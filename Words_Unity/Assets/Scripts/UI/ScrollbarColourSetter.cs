using UnityEngine;
using UnityEngine.UI;

public class ScrollbarColourSetter : MonoBehaviour
{
	private Scrollbar mScrollbarRef;

	public bool SetNormalColour = true;
	public bool SetHighlightedColour = true;
	public bool SetPressedColour = true;

	void Awake()
	{
#if UNITY_ANDROID || UNITY_IOS
		SetHighlightedColour = false;
#endif // UNITY_ANDROID || UNITY_IOS

		mScrollbarRef = GetComponent<Scrollbar>();
		ODebug.AssertNull(mScrollbarRef);

		ColorBlock colourBlock = mScrollbarRef.colors;

		if (SetNormalColour)
		{
			colourBlock.normalColor = GlobalSettings.Instance.UIHightlightColour;
		}
		if (SetHighlightedColour)
		{
			colourBlock.highlightedColor = GlobalSettings.Instance.UIHightlightColour;
		}
		if (SetPressedColour)
		{
			colourBlock.pressedColor = GlobalSettings.Instance.UIHightlightColour;
		}

		mScrollbarRef.colors = colourBlock;
	}
}