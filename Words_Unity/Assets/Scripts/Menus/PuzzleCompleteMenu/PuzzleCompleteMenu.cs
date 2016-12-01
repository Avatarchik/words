using UnityEngine;
using System.Collections.Generic;

public class PuzzleCompleteMenu : Menu, IMenu, IOrientationChangedNotifiee
{
	public List<ParticleSystem> Effects = new List<ParticleSystem>();

	public override void OnEnable()
	{
		base.OnEnable();

		OrientationManager.Instance.RegisterForNotification(this);

		ParticleSystem randEffect = Effects.RandomItem();
		randEffect.gameObject.SetActive(true);
		randEffect.Stop();
		randEffect.Clear();
		randEffect.Play();
	}

	public override void OnDisable()
	{
		base.OnDisable();

		if (OrientationManager.Instance)
		{
			OrientationManager.Instance.UnregisterForNotification(this);
		}

		foreach (ParticleSystem effect in Effects)
		{
			effect.gameObject.SetActive(false);
		}
	}

	public override void OnScreenSizeChanged(Vector2 screenSize)
	{
		base.OnScreenSizeChanged(screenSize);

		float halfScreenWidth = screenSize.x * 0.5f;

		foreach (ParticleSystem effect in Effects)
		{
			ParticleSystem.ShapeModule sm = effect.shape;
			sm.radius = halfScreenWidth;
		}
	}
}