using UnityEngine;
using UnityEngine.UI;

public class BackgroundPicker : MonoBehaviour
{
	public Image ImageRef;
	public Sprite[] Backgrounds;
	static private Sprite sChosenBackground;

	void Awake()
	{
		if (sChosenBackground == null)
		{
			int randIndex = Random.Range(0, Backgrounds.Length);
			sChosenBackground = Backgrounds[randIndex];

			ImageRef.sprite = sChosenBackground;
		}
	}
}