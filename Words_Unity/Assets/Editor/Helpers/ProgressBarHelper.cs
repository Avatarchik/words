using UnityEngine;
using UnityEditor;

static public class ProgressBarHelper
{
	static private bool sIsCancelable;
	static private string sTitle = string.Empty;
	static private string sMessage = string.Empty;
	static private float sStep = 1f;
	static private float sProgress;

	static public void Begin(bool isCancelable, string title, string message, float step = 1)
	{
		sIsCancelable = isCancelable;
		sTitle = title;
		sMessage = message;
		sStep = step;
		sProgress = 0;

		Update(0);
	}

	static public void SetTitleAndMessage(string title, string message)
	{
		sTitle = title;
		sMessage = message;
	}

	static public bool Update(int numberOfSteps = 1)
	{
		return Update(numberOfSteps, null);
	}

	static public bool Update(string message)
	{
		return Update(1, message);
	}

	static public bool Update(int numberOfSteps, string message)
	{
		if (ApplicationHelper.IsAnAutomatedBuild)
		{
			return false;
		}

		sProgress += sStep * numberOfSteps;
		sProgress = Mathf.Clamp01(sProgress);

		if (!string.IsNullOrEmpty(message))
		{
			sMessage = message;
		}

		if (sIsCancelable)
		{
			if (EditorUtility.DisplayCancelableProgressBar(sTitle, sMessage, sProgress))
			{
				End();
				return true;
			}
		}
		else
		{
			EditorUtility.DisplayProgressBar(sTitle, sMessage, sProgress);
		}

		return false;
	}

	static public void End()
	{
		if (!ApplicationHelper.IsAnAutomatedBuild)
		{
			EditorUtility.ClearProgressBar();
		}
	}
}