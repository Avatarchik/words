using UnityEngine;

static public class MathfHelper
{
	static public float TwoPi = Mathf.PI * 2f;
	static public float PiOverTwo = Mathf.PI * 0.5f;
	static public float PiOverFour = Mathf.PI * 0.25f;

	static public int Repeat(int value, int length)
	{
		return (int)Mathf.Repeat(value, length);
	}

	static public int Repeat(int value, int from, int to)
	{
		return (int)Mathf.Repeat(value - from, to - from) + from;
	}

	static public float Repeat(float value, float from, float to)
	{
		return Mathf.Repeat(value - from, to - from) + from;
	}

	static public int Clamp0(int value)
	{
		if (value < 0)
		{
			value = 0;
		}

		return value;
	}

	static public float Clamp0(float value)
	{
		if (value < 0)
		{
			value = 0;
		}

		return value;
	}
}