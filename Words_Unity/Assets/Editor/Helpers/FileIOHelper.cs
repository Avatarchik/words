using UnityEngine;
using UnityEditor;
using System;
using System.IO;

static public class FileIOHelper
{
	static public DirectoryInfo CreateDirectoryIfDoesntExist(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			Debug.LogWarning("FileIOHelper: Null or empty path param");
			return null;
		}

		DirectoryInfo dirInfo = null;

		// Strip the file name and extension off if they are there
		path = Path.GetDirectoryName(path);

		// If it doesn't exist, create it otherwise clean it if its required
		if (!Directory.Exists(path))
		{
			dirInfo = Directory.CreateDirectory(path);

			if (!Directory.Exists(path))
			{
				Debug.LogWarning("FileIOHelper: Unable to create a directory at: " + path);
				return null;
			}

			return dirInfo;
		}
		
		return dirInfo;
	}

	static public void CleanDirectory(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			Debug.LogWarning("FileIOHelper: Null or empty path param");
			return;
		}

		DirectoryInfo info = new DirectoryInfo(path);
		if (info == null)
		{
			Debug.LogWarning("FileIOHelper: Unable to get the DirectoryInfo for: " + path);
			return;
		}
		
		foreach(FileInfo file in info.GetFiles())
		{
			file.Delete();
		}
		
		foreach(DirectoryInfo subDirectory in info.GetDirectories())
		{
			subDirectory.Delete(true);
		}
	}

	static public string MakePathRelativeToAssetsFolder(string path)
	{
		if (!path.Contains(Application.dataPath))
		{
			return path;
		}

		string strippedPath = path.Replace("\\", "/");
		strippedPath = strippedPath.Replace(Application.dataPath, string.Empty);
		strippedPath = strippedPath.TrimStart(new char[] { '/' });
		strippedPath = PathHelper.Combine("Assets", strippedPath);

		return strippedPath;
	}

	static public string MakePathAbsolute(string path)
	{
		path = ApplicationHelper.ProjectRootPath + path;
		path = PathHelper.StandardiseSlashes(path);
		return path;
	}
}