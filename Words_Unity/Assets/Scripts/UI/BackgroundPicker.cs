using UnityEngine;
using UnityEngine.UI;

[ScriptOrder(-50)]
public class BackgroundPicker : UIMonoBehaviour, IOrientationChangedNotifiee
{
	public Image ImageRef;
	public Sprite[] Backgrounds;
	static public Sprite sChosenBackground { get; private set; }

	public bool IsABand = false;

	void Awake()
	{
		if (sChosenBackground == null)
		{
			int randIndex = Random.Range(0, Backgrounds.Length);
			sChosenBackground = Backgrounds[randIndex];
		}

		ImageRef.sprite = sChosenBackground;
	}

	public virtual void OnEnable()
	{
		OrientationManager.Instance.RegisterForNotification(this);
	}

	public virtual void OnDisable()
	{
		if (OrientationManager.Instance)
		{
			OrientationManager.Instance.UnregisterForNotification(this);
		}
	}

	public void OnScreenSizeChanged(Vector2 screenSize)
	{
		if (IsABand)
		{
			rectTransform.sizeDelta = new Vector2(screenSize.x, rectTransform.sizeDelta.y);
		}
		else
		{
			rectTransform.sizeDelta = screenSize;
		}
	}
}