using UnityEngine;
using UnityEngine.UI;

static public class ColorBlockHelper
{
	static public void SetNormalColour(Selectable obj, Color normalColour)
	{
		ColorBlock colours = obj.colors;
		colours.normalColor = normalColour;
		obj.colors = colours;
	}
}