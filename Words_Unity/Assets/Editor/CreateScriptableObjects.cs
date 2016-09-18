using UnityEngine;
using UnityEditor;

static public class CreateScriptableObjects
{
	static private string GetSelectedAssetPath()
	{
		Object[] selectedAsset = Selection.GetFiltered(typeof(DefaultAsset), SelectionMode.DeepAssets);
		foreach (Object obj in selectedAsset)
		{
			if (obj != null)
			{
				return AssetDatabase.GetAssetPath(obj);
			}
		}

		return "Assets";
	}

	static private T CreateNewScriptableObject<T>(string name, string path = null)
		where T : ScriptableObject
	{
		T scriptInst = ScriptableObject.CreateInstance<T>();

		if (path == null)
		{
			path = PathHelper.Combine(GetSelectedAssetPath(), name + ".asset");
		}
		AssetDatabase.CreateAsset(scriptInst, AssetDatabase.GenerateUniqueAssetPath(path));
		AssetDatabase.SaveAssets();

		Selection.activeObject = scriptInst;
		EditorUtility.FocusProjectWindow();

		return scriptInst;
	}

	// Game specific functions

	[MenuItem("Assets/Create/Scriptable Objects/Puzzle Contents")]
	static public PuzzleContents CreateNewPuzzleContents(string path = null)
	{
		PuzzleContents newContents = CreateNewScriptableObject<PuzzleContents>("New Puzzle Contents", path);
		return newContents;
	}

	[MenuItem("Assets/Create/Scriptable Objects/Colour Scheme")]
	static public void CreateNewColourScheme()
	{
		CreateNewScriptableObject<ColourScheme>("New Colour Scheme");
	}
}