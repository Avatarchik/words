using UnityEngine;
using UnityEngine.UI;

[ScriptOrder(-50)]
public class BackgroundPicker : ScreenStretchedUIMonoBehaviour
{
	public Image ImageRef;
	public Sprite[] Backgrounds;
	static public Sprite sChosenBackground { get; private set; }

	void Awake()
	{
		if (sChosenBackground == null)
		{
			int randIndex = Random.Range(0, Backgrounds.Length);
			sChosenBackground = Backgrounds[randIndex];
		}

		ImageRef.sprite = sChosenBackground;
	}
}