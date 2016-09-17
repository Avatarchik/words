using UnityEditor;
using UnityEngine;

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

	static private void CreateNewScriptableObject<T>(string name)
		where T : ScriptableObject
	{
		T scriptInst = ScriptableObject.CreateInstance<T>();

		string path = PathHelper.Combine(GetSelectedAssetPath(), name + ".asset");
		AssetDatabase.CreateAsset(scriptInst, AssetDatabase.GenerateUniqueAssetPath(path));
		AssetDatabase.SaveAssets();

		Selection.activeObject = scriptInst;
		EditorUtility.FocusProjectWindow();
	}

	[MenuItem("Assets/Create/Scriptable Objects/Puzzle Contents")]
	static public void CreatePuzzleContents()
	{
		CreateNewScriptableObject<PuzzleContents>("New Puzzle Contents");
	}
}