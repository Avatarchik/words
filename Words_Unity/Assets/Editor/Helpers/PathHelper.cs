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
}