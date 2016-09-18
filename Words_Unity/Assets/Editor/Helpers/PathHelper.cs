using UnityEngine;
using System.IO;

static public class PathHelper
{
	static public string StandardiseSlashes(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			path = path.Replace("\\", "/");
			path = path.Replace("//", "/");
		}

		return path;
	}
	
	static public string Combine(string lhs, string rhs)
	{
		string path = Path.Combine(lhs, rhs);
		path = StandardiseSlashes(path);
		return path;
	}

	static public string MakeRelativeToAssetsFolder(string path)
	{
		if (!string.IsNullOrEmpty(path))
		{
			path = StandardiseSlashes(path);
			if (path.StartsWith(ApplicationHelper.ProjectRootPath))
			{
				path = path.Replace(ApplicationHelper.ProjectRootPath, string.Empty);
				if (!Path.HasExtension(path) && !path.EndsWith("/"))
				{
					path += "/";
				}
			}
		}

		return path;
	}
}