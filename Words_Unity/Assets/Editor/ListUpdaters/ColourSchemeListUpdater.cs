using UnityEngine;
using UnityEditor;
using System.IO;

public class ColourSchemeListUpdater
{
	[MenuItem("Words/List Updaters/Colour Schemes")]
	static void UpdateColourSchemeList()
	{
		GameObject colourSchemeListPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Managers/ColourSchemeManager.prefab");
		if (colourSchemeListPrefab)
		{
			ColourSchemeManager schemeManager = colourSchemeListPrefab.GetComponent<ColourSchemeManager>();
			if (schemeManager)
			{
				schemeManager.ClearList();

				string[] schemePaths = Directory.GetFiles(PathHelper.Combine(Application.dataPath, "Resources/ColourSchemes/"), "*.asset");

				foreach (string path in schemePaths)
				{
					string relativePath = FileIOHelper.MakePathRelativeToAssetsFolder(path);
					ColourScheme scheme = AssetDatabase.LoadAssetAtPath(relativePath, typeof(ColourScheme)) as ColourScheme;
					schemeManager.RegisterScheme(scheme);
				}

				EditorUtility.SetDirty(colourSchemeListPrefab);
				ODebug.Log("Colour scheme list updated");
			}
		}
	}
}