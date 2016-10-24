using UnityEngine;
using UnityEditor;
using System.IO;

public class PuzzleListUpdater
{
	[MenuItem("Words/List Updaters/Puzzles")]
	static void UpdatePuzzleList()
	{
		GameObject puzzleListPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Managers/PuzzleManager.prefab");
		if (puzzleListPrefab)
		{
			PuzzleManager puzzleManager = puzzleListPrefab.GetComponent<PuzzleManager>();
			if (puzzleManager)
			{
				puzzleManager.InitialiseLists();

				for (int puzzleSize = 4; puzzleSize < 17; ++puzzleSize)
				{
					string searchDir = PathHelper.Combine(Application.dataPath, string.Format("Resources/Puzzles/Size {0}", puzzleSize));
					string[] puzzlePaths = Directory.GetFiles(searchDir, "*.asset");

					foreach (string path in puzzlePaths)
					{
						string relativePath = PathHelper.MakeRelativeToAssetsFolder(path);
						PuzzleContents puzzle = AssetDatabase.LoadAssetAtPath(relativePath, typeof(PuzzleContents)) as PuzzleContents;
						puzzleManager.RegisterPuzzle(puzzle, puzzleSize);
					}
				}

				EditorUtility.SetDirty(puzzleListPrefab);
				ODebug.Log("Puzzle list updated");
			}
		}
	}
}