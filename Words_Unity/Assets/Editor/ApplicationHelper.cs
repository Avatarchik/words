using UnityEngine;

static public class ApplicationHelper
{
	static public string ProjectRootPath
	{
		get
		{
			string path = Application.dataPath.Substring(0, Application.dataPath.Length - 6); // Removes "Assets" from the end of the path
			return path;
		}
	}
}