using UnityEngine;

static public class ColorHelper
{
	static public Color From255(int r, int g, int b)
	{
		return From255(r, g, b, 255);
	}
	
	static public Color From255(int r, int g, int b, int a)
	{
		return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
	}
	
	static public Color SetAlpha255(Color col, int a)
	{
		return new Color(col.r, col.g, col.b, a / 255f);
	}
	
	static public Color SetAlpha(Color col, float a)
	{
		col.a = a;
		return col;
	}

	static public Color Blend(Color from, Color to, float t)
	{
		t = MathfHelper.Clamp01(t);
		float oneMinusT = 1 - t;

		float r = (from.r * t) + to.r * oneMinusT;
		float g = (from.g * t) + to.g * oneMinusT;
		float b = (from.b * t) + to.b * oneMinusT;
		float a = (from.a * t) + to.a * oneMinusT;

		return new Color(r, g, b, a);
	}
}