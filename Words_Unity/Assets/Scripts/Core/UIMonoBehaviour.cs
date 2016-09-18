using UnityEngine;

public class UIMonoBehaviour : MonoBehaviour
{
	private RectTransform _rectTransform;
	public RectTransform rectTransform
	{
		get
		{
			if (_rectTransform == null)
			{
				_rectTransform = GetComponent<RectTransform>();
			}

			return _rectTransform;
		}
	}
}