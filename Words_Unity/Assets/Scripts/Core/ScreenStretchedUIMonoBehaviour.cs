using UnityEngine;

public class ScreenStretchedUIMonoBehaviour : UIMonoBehaviour, IOrientationChangedNotifiee
{
	public virtual void OnEnable()
	{
		OrientationManager.Instance.RegisterForNotification(this);
	}

	public virtual void OnDisable()
	{
		OrientationManager.Instance.UnregisterForNotification(this);
	}

	public void OnScreenSizeChanged(Vector2 screenSize)
	{
		rectTransform.sizeDelta = screenSize;
	}
}