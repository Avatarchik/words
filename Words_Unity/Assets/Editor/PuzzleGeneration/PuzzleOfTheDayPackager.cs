using UnityEngine;
using UnityEditor;
using System.IO;

public class PuzzleOfTheDayPackager
{
	[MenuItem("Words/Puzzle/PotD Packager/For All Platforms")]
	static void PackagePuzzleOfTheDays()
	{
		PackagePuzzleOfTheDaysForiOS();
		PackagePuzzleOfTheDaysForAndroid();
	}

	[MenuItem("Words/Puzzle/PotD Packager/For iOS")]
	static void PackagePuzzleOfTheDaysForiOS()
	{
		Package(BuildTarget.iOS);
	}

	[MenuItem("Words/Puzzle/PotD Packager/For Android")]
	static void PackagePuzzleOfTheDaysForAndroid()
	{
		Package(BuildTarget.Android);
	}

	static private void Package(BuildTarget target)
	{
		string targetAsString = target.ToString();
		string platformPath = string.Format("PuzzleOfTheDay/Bundles/{0}/", targetAsString);

		string sourcePath = PathHelper.Combine("Assets", platformPath);
		string sourceAbsolutePath = FileIOHelper.MakePathAbsolute(sourcePath);
		FileIOHelper.CleanDirectory(sourceAbsolutePath);

		BuildPipeline.BuildAssetBundles(sourcePath, BuildAssetBundleOptions.None, target);
		ODebug.Log(string.Format("Exported asset bundles for {0}", targetAsString));

		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);

		string[] bundlePaths = Directory.GetFiles(PathHelper.Combine(Application.dataPath, platformPath));
		foreach (string bundlePath in bundlePaths)
		{
			if (string.IsNullOrEmpty(bundlePath))
			{
				continue;
			}

			if (!Path.HasExtension(bundlePath) && !bundlePath.EndsWith(targetAsString))
			{
				File.Move(bundlePath, bundlePath + ".unity3d");
			}
		}

		ODebug.Log("Hacked around the missing file extensions");

		AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
	}
}