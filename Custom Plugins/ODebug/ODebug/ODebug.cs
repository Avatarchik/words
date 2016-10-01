using UnityEngine;

static public class ODebug
{
	static public bool IsEnabled = true;

	#region Log
	static public void Log(object message)
	{
		if (IsEnabled)
		{
			Debug.Log(message);
		}
	}

	static public void LogIf(bool check, object message)
	{
		if (IsEnabled && check)
		{
			Debug.Log(message);
		}
	}
	#endregion

	#region LogWarning
	static public void LogWarning(object message)
	{
		if (IsEnabled)
		{
			Debug.LogWarning(message);
		}
	}

	static public void LogWarningIf(bool check, object message)
	{
		if (IsEnabled && check)
		{
			Debug.LogWarning(message);
		}
	}
	#endregion

	#region LogError
	static public void LogError(object message)
	{
		if (IsEnabled)
		{
			Debug.LogError(message);
		}
	}

	static public void LogErrorIf(bool check, object message)
	{
		if (IsEnabled && check)
		{
			Debug.LogError(message);
		}
	}
	#endregion

	#region Assert
	static public void Assert(bool check)
	{
		if (IsEnabled && !check)
		{
			Debug.LogError("Assert failed");
			Debug.Break();
		}
	}

	static public void AssertNull(bool check)
	{
		if (IsEnabled && !check)
		{
			Debug.LogError("Assert failed: Object is null");
			Debug.Break();
		}
	}

	static public void AssertValidIndex(int current, int start, int end)
	{
		if (IsEnabled)
		{
			bool check = current >= start && current < end;
			if (!check)
			{
				Debug.LogError(string.Format("Assert failed: Index is invalid. Current: {0}, Start {1}, End {2}", current, start, end));
				Debug.Break();
			}
		}
	}

	static public void AssertTODO()
	{
		if (IsEnabled)
		{
			Debug.LogError("Implement me :-)");
			Debug.Break();
		}
	}
	#endregion
}