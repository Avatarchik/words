using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class WordsUpdater
{
	[MenuItem("Words/List Updaters/Words")]
	static void UpdateWordLists()
	{
		GameObject wordListsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Words.prefab");
		if (wordListsPrefab)
		{
			Words words = wordListsPrefab.GetComponent<Words>();
			if (words)
			{
				string[] wordListPaths = Directory.GetFiles(PathHelper.Combine(Application.dataPath, "Words"), "*.txt");

				int wordCount = 0;

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
							++wordCount;
						}
					}

					string letter = Path.GetFileNameWithoutExtension(path);
					words.SetList(letter, wordList.ToArray());
				}

				EditorUtility.SetDirty(wordListsPrefab);

				Debug.Log("Word list updated");
				Debug.Log(string.Format("Now contains {0:n0} words", wordCount));
			}
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