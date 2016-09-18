using UnityEngine;
using UnityEditor;
using System.IO;

public class PuzzleListUpdater
{
	[MenuItem("Words/List Updaters/Puzzles")]
	static void UpdatePuzzleList()
	{
		GameObject puzzleListPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Managers/PuzzleContentsManager.prefab");
		if (puzzleListPrefab)
		{
			PuzzleContentsManager puzzleManager = puzzleListPrefab.GetComponent<PuzzleContentsManager>();
			if (puzzleManager)
			{
				puzzleManager.ClearList();

				string[] puzzlePaths = Directory.GetFiles(PathHelper.Combine(Application.dataPath, "Prefabs/Puzzles/"), "*.asset");

				foreach (string path in puzzlePaths)
				{
					string relativePath = PathHelper.MakeRelativeToAssetsFolder(path);
					PuzzleContents puzzle = AssetDatabase.LoadAssetAtPath(relativePath, typeof(PuzzleContents)) as PuzzleContents;
					puzzleManager.RegisterPuzzle(puzzle);
				}

				EditorUtility.SetDirty(puzzleListPrefab);
				Debug.Log("Puzzle list updated");
			}
		}
	}
}