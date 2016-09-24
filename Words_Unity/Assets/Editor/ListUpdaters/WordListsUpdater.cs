using UnityEngine;
using UnityEditor;
using System;
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
				words.Initialise();

				string progressBarMessageFormat = "Processing '{0}' word list.";
				string progressBarMessage = string.Format(progressBarMessageFormat, "?");
				ProgressBarHelper.Begin(false, "Word List Updater", progressBarMessage, 1f / 26);

				int wordCount = 0;

				string[] wordListPaths = Directory.GetFiles(PathHelper.Combine(Application.dataPath, "Words"), "*.txt");
				foreach (string path in wordListPaths)
				{
					char initialChar = Path.GetFileNameWithoutExtension(path)[0];

					progressBarMessage = string.Format(progressBarMessageFormat, initialChar);
					ProgressBarHelper.Update(progressBarMessage);

					string fileContents = File.ReadAllText(path);
					string[] splitFileContents = fileContents.Split('\n');

					List<string> wordList = new List<string>(splitFileContents.Length);
					foreach (string word in splitFileContents)
					{
						if (IsWordValid(word))
						{
							wordList.Add(word.ToUpper());
							++wordCount;
						}
					}

					words.SetList(initialChar, wordList);
				}

				ProgressBarHelper.End();

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