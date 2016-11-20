using UnityEngine;
using UnityEngine.UI;

public class ImageColourSetter : MonoBehaviour
{
	private Image mImageRef;

	void Awake()
	{
		mImageRef = GetComponent<Image>();
		ODebug.AssertNull(mImageRef);

		mImageRef.color = GlobalSettings.Instance.UIHightlightColour;
	}
}