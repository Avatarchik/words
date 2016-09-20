using UnityEngine;
using UnityEditor;
using System.IO;

public class ColourSchemeListUpdater
{
	[MenuItem("Words/List Updaters/Colour Schemes")]
	static void UpdateColourSchemeList()
	{
		GameObject colourSchemeListPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Managers/ColourSchemesManager.prefab");
		if (colourSchemeListPrefab)
		{
			ColourSchemesManager schemeManager = colourSchemeListPrefab.GetComponent<ColourSchemesManager>();
			if (schemeManager)
			{
				schemeManager.ClearList();

				string[] schemePaths = Directory.GetFiles(PathHelper.Combine(Application.dataPath, "Resources/ColourSchemes/"), "*.asset");

				foreach (string path in schemePaths)
				{
					string relativePath = PathHelper.MakeRelativeToAssetsFolder(path);
					ColourScheme scheme = AssetDatabase.LoadAssetAtPath(relativePath, typeof(ColourScheme)) as ColourScheme;
					schemeManager.RegisterScheme(scheme);
				}

				EditorUtility.SetDirty(colourSchemeListPrefab);
				Debug.Log("Colour scheme list updated");
			}
		}
	}
}