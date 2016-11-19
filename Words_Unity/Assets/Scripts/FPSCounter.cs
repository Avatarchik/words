using UnityEngine;
using UnityEngine.UI;

// Taken from: http://wiki.unity3d.com/index.php?title=FramesPerSecond#CSharp_HUDFPS.cs

public class FPSCounter : MonoBehaviour
{
	/*
	 * It calculates frames/second over each updateInterval,
	 * so the display does not keep changing wildly.
	 *
	 * It is also fairly accurate at very low FPS counts (<10).
	 * We do this not by simply counting frames per interval, but
	 * by accumulating FPS for each frame. This way we end up with
	 * correct overall FPS even if the interval renders something like
	 * 5.5 frames.
	*/

	public Text TextRef;
	public float UpdateInterval = 0.5F;

	public Color GoodColour = Color.green;
	public Color WarningColour = Color.yellow;
	public Color BadColour = Color.red;

	private float mAccumulator = 0;		// FPS accumulated over the interval
	private int mFrames = 0;			// Frames drawn over the interval
	private float mTimeleft;			// Left time for current interval

	void Start()
	{
		mTimeleft = UpdateInterval;
	}

	void Update()
	{
		mTimeleft -= Time.deltaTime;
		mAccumulator += Time.timeScale / Time.deltaTime;
		++mFrames;

		// Interval ended - update GUI text and start new interval
		if (mTimeleft <= 0.0)
		{
			float fps = mAccumulator / mFrames;
			string format = string.Format("{0:F2} FPS", fps);
			TextRef.text = format;

			if (fps < 30)
			{
				TextRef.color = WarningColour;
			}
			else if (fps < 10)
			{
				TextRef.color = BadColour;
			}
			else
			{
				TextRef.color = GoodColour;
			}

			mTimeleft = UpdateInterval;
			mAccumulator = 0.0F;
			mFrames = 0;
		}
	}
}