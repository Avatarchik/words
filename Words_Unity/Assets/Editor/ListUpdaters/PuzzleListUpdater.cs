using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
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

				for (int puzzleSize = GlobalSettings.PuzzleSizeMin; puzzleSize < GlobalSettings.PuzzleSizeMax; ++puzzleSize)
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

	[MenuItem("Words/List Updaters/Puzzles - Definitions Only")]
	static void UpdatePuzzleDefinitions()
	{
		WordDefinitions definitions = null;
		GameObject wordDefinitionsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WordDefinitions.prefab");
		if (wordDefinitionsPrefab)
		{
			definitions = wordDefinitionsPrefab.GetComponent<WordDefinitions>();
		}

		if (definitions == null)
		{
			Debug.LogWarning("Failed to find the WordDefinitions component");
			return;
		}

		List<string> puzzlePaths = new List<string>();

		for (int puzzleSize = GlobalSettings.PuzzleSizeMin; puzzleSize < GlobalSettings.PuzzleSizeMax; ++puzzleSize)
		{
			string searchDir = PathHelper.Combine(Application.dataPath, string.Format("Resources/Puzzles/Size {0}", puzzleSize));
			string[] foundPuzzlePaths = Directory.GetFiles(searchDir, "*.asset");

			puzzlePaths.AddRange(foundPuzzlePaths);
		}

		int puzzleCount = puzzlePaths.Count;
		int puzzlesUpdated = 0;
		ProgressBarHelper.Begin(true, "Puzzle Updater", "Updating puzzles", 1f / puzzleCount);
		foreach (string path in puzzlePaths)
		{
			string relativePath = PathHelper.MakeRelativeToAssetsFolder(path);
			PuzzleContents puzzle = AssetDatabase.LoadAssetAtPath(relativePath, typeof(PuzzleContents)) as PuzzleContents;
			puzzle.UpdateDefinitions(definitions);
			EditorUtility.SetDirty(puzzle);

			if (!ProgressBarHelper.Update(string.Format("Updated puzzles: {0}/{1}", ++puzzlesUpdated, puzzleCount)))
			{
				break;
			}
		}
		ProgressBarHelper.End();

		ODebug.Log("Puzzle definitions updated");
	}
}