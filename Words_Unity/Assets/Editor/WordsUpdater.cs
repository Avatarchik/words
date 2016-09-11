using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class WordsUpdater
{
	[MenuItem("Words/Words Updater")]
	static void UpdateWords()
	{
		GameObject wordsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Words.prefab");
		if (wordsPrefab)
		{
			Words words = wordsPrefab.GetComponent<Words>();
			if (words)
			{
				string[] wordListPaths = Directory.GetFiles(Path.Combine(Application.dataPath, "Words"), "*.txt");

				foreach (string path in wordListPaths)
				{
					string fileContents = File.ReadAllText(path);
					string[] splitFileContents = fileContents.Split('\n');
					int splitFileContentsLength = splitFileContents.Length;

					List<string> wordList = new List<string>(splitFileContentsLength);
					foreach (string word in splitFileContents)
					{
						if (IsWordValid(word))
						{
							wordList.Add(word.ToUpper());
						}
					}

					string letter = Path.GetFileNameWithoutExtension(path);
					words.SetList(letter, wordList.ToArray());

					EditorUtility.SetDirty(wordsPrefab);
				}

				Debug.Log("Word lists updated");
			}
			else
			{
				Debug.LogWarning("Failed to find Words script");
			}
		}
		else
		{
			Debug.LogWarning("Failed to find words prefab");
		}
	}

	static private bool IsWordValid(string word)
	{
		word = word.ToUpper();

		bool isValid = true;

		isValid &= !string.IsNullOrEmpty(word);
		isValid &= word.Length > 2;
		isValid &= !word.Contains(" ");

		foreach (char character in word)
		{
			isValid &= character >= 'A' && character <= 'Z';
		}

		return isValid;
	}
}