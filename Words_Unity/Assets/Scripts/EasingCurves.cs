using System;

/// <summary>The easing.</summary>
static public class EasingCurves
{
	// Adapted from source : http://www.robertpenner.com/easing/

	/// <summary>The ease.</summary>
	/// <param name="linearStep">The linear step.</param>
	/// <param name="acceleration">The acceleration.</param>
	/// <param name="type">The type.</param>
	/// <returns>The ease.</returns>
	static public float Ease(double linearStep, float acceleration, EEasingType type)
	{
		float easedStep = acceleration > 0
								? EaseIn(linearStep, type)
								: acceleration < 0
									? EaseOut(linearStep, type)
									: (float)linearStep;

		return MathHelper.Lerp(linearStep, easedStep, Math.Abs(acceleration));
	}

	/// <summary>The ease in.</summary>
	/// <param name="linearStep">The linear step.</param>
	/// <param name="type">The type.</param>
	/// <returns>The ease in.</returns>
	/// <exception cref="NotImplementedException"></exception>
	static public float EaseIn(double linearStep, EEasingType type)
	{
		switch (type)
		{
			case EEasingType.Step:
				return linearStep < 0.5 ? 0 : 1;
			case EEasingType.Linear:
				return (float)linearStep;
			case EEasingType.Sine:
				return Sine.EaseIn(linearStep);
			case EEasingType.Quadratic:
				return Power.EaseIn(linearStep, 2);
			case EEasingType.Cubic:
				return Power.EaseIn(linearStep, 3);
			case EEasingType.Quartic:
				return Power.EaseIn(linearStep, 4);
			case EEasingType.Quintic:
				return Power.EaseIn(linearStep, 5);
		}

		throw new NotImplementedException();
	}

	/// <summary>The ease out.</summary>
	/// <param name="linearStep">The linear step.</param>
	/// <param name="type">The type.</param>
	/// <returns>The ease out.</returns>
	/// <exception cref="NotImplementedException"></exception>
	static public float EaseOut(double linearStep, EEasingType type)
	{
		switch (type)
		{
			case EEasingType.Step:
				return linearStep < 0.5 ? 0 : 1;
			case EEasingType.Linear:
				return (float)linearStep;
			case EEasingType.Sine:
				return Sine.EaseOut(linearStep);
			case EEasingType.Quadratic:
				return Power.EaseOut(linearStep, 2);
			case EEasingType.Cubic:
				return Power.EaseOut(linearStep, 3);
			case EEasingType.Quartic:
				return Power.EaseOut(linearStep, 4);
			case EEasingType.Quintic:
				return Power.EaseOut(linearStep, 5);
		}

		throw new NotImplementedException();
	}

	/// <summary>The ease in out.</summary>
	/// <param name="linearStep">The linear step.</param>
	/// <param name="easeInType">The ease in type.</param>
	/// <param name="easeOutType">The ease out type.</param>
	/// <returns>The ease in out.</returns>
	static public float EaseInOut(double linearStep, EEasingType easeInType, EEasingType easeOutType)
	{
		return linearStep < 0.5 ? EaseInOut(linearStep, easeInType) : EaseInOut(linearStep, easeOutType);
	}

	/// <summary>The ease in out.</summary>
	/// <param name="linearStep">The linear step.</param>
	/// <param name="type">The type.</param>
	/// <returns>The ease in out.</returns>
	/// <exception cref="NotImplementedException"></exception>
	static public float EaseInOut(double linearStep, EEasingType type)
	{
		switch (type)
		{
			case EEasingType.Step:
				return linearStep < 0.5 ? 0 : 1;
			case EEasingType.Linear:
				return (float)linearStep;
			case EEasingType.Sine:
				return Sine.EaseInOut(linearStep);
			case EEasingType.Quadratic:
				return Power.EaseInOut(linearStep, 2);
			case EEasingType.Cubic:
				return Power.EaseInOut(linearStep, 3);
			case EEasingType.Quartic:
				return Power.EaseInOut(linearStep, 4);
			case EEasingType.Quintic:
				return Power.EaseInOut(linearStep, 5);
		}

		throw new NotImplementedException();
	}

#region Nested type: Power
/// <summary>The power.</summary>
static private class Power
{
	/// <summary>The ease in.</summary>
	/// <param name="s">The s.</param>
	/// <param name="power">The power.</param>
	/// <returns>The ease in.</returns>
	static public float EaseIn(double s, int power)
	{
		return (float)Math.Pow(s, power);
	}

	/// <summary>The ease out.</summary>
	/// <param name="s">The s.</param>
	/// <param name="power">The power.</param>
	/// <returns>The ease out.</returns>
	static public float EaseOut(double s, int power)
	{
		int sign = power % 2 == 0 ? -1 : 1;
		return (float)(sign * (Math.Pow(s - 1, power) + sign));
	}

	/// <summary>The ease in out.</summary>
	/// <param name="s">The s.</param>
	/// <param name="power">The power.</param>
	/// <returns>The ease in out.</returns>
	static public float EaseInOut(double s, int power)
	{
		s *= 2;
		if (s < 1) return EaseIn(s, power) / 2;
		int sign = power % 2 == 0 ? -1 : 1;
		return (float)(sign / 2.0 * (Math.Pow(s - 2, power) + sign * 2));
	}
}
#endregion

#region Nested type: Sine
/// <summary>The sine.</summary>
static private class Sine
{
	/// <summary>The ease in.</summary>
	/// <param name="s">The s.</param>
	/// <returns>The ease in.</returns>
	static public float EaseIn(double s)
	{
		return (float)Math.Sin(s * MathHelper.HalfPi - MathHelper.HalfPi) + 1;
	}

	/// <summary>The ease out.</summary>
	/// <param name="s">The s.</param>
	/// <returns>The ease out.</returns>
	static public float EaseOut(double s)
	{
		return (float)Math.Sin(s * MathHelper.HalfPi);
	}

	/// <summary>The ease in out.</summary>
	/// <param name="s">The s.</param>
	/// <returns>The ease in out.</returns>
	static public float EaseInOut(double s)
	{
		return (float)(Math.Sin(s * MathHelper.Pi - MathHelper.HalfPi) + 1) / 2;
	}
}

#endregion
}

/// <summary>The easing type.</summary>
public enum EEasingType
{
	/// <summary>The step.</summary>
	Step,

	/// <summary>The linear.</summary>
	Linear,

	/// <summary>The sine.</summary>
	Sine,

	/// <summary>The quadratic.</summary>
	Quadratic,

	/// <summary>The cubic.</summary>
	Cubic,

	/// <summary>The quartic.</summary>
	Quartic,

	/// <summary>The quintic.</summary>
	Quintic
}

/// <summary>The math helper.</summary>
static public class MathHelper
{
	/// <summary>The pi.</summary>
	public const float Pi = (float)Math.PI;

	/// <summary>The half pi.</summary>
	public const float HalfPi = (float)(Math.PI / 2);

	/// <summary>The lerp.</summary>
	/// <param name="from">The from.</param>
	/// <param name="to">The to.</param>
	/// <param name="step">The step.</param>
	/// <returns>The lerp.</returns>
	static public float Lerp(double from, double to, double step)
	{
		return (float)((to - from) * step + from);
	}
}