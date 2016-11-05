using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SimpleJSON;

public class WordsUpdater
{
	[MenuItem("Words/List Updaters/Words")]
	static void UpdateWordLists()
	{
		bool findDefinitions = EditorUtility.DisplayDialog(
			"Word List Updater",
			"Do you want to find the definitions too? It will take some time.\n\nThe progress will appear in the console window.",
			"Yes", "No");

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

				string[] wordListPaths = Directory.GetFiles(PathHelper.Combine(Application.dataPath, "WordLists"), "*.txt");
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

				if (findDefinitions)
				{
					GetDefinitions(words);
				}

				EditorUtility.SetDirty(wordListsPrefab);

				ODebug.Log("Word list updated");
				ODebug.Log(string.Format("Now contains {0:n0} words", wordCount));
			}
		}
	}

	static private bool IsWordValid(string word)
	{
		word = word.ToUpper();

		bool isValid = true;

		isValid &= !string.IsNullOrEmpty(word);
		isValid &= word.Length > 2;
		isValid &= word.Length < 13;
		isValid &= !word.Contains(" ");

		foreach (char character in word)
		{
			isValid &= character >= 'A' && character <= 'Z';
		}

		return isValid;
	}

	static private Words sWords;
	static private int sWordsToCheck;
	static private int sWordsChecked;
	static private int sWordDefinitionsFound;

	static private void GetDefinitions(Words words)
	{
		sWords = words;
		sWordsToCheck = 0;
		sWordsChecked = 0;
		sWordDefinitionsFound = 0;

		for (int portionIndex = 0; portionIndex < sWords.ListPortions.Count; ++portionIndex) // TODO - this shouldn't create all of them at once
		{
			Words.WordListPortion portion = words.ListPortions[portionIndex];
			sWordsToCheck += portion.ContainedWordsCount;

			Thread portionThread = new Thread(new ParameterizedThreadStart(GetWordDefinitionsForPortion));
			portionThread.Start(portion);
		}

		Thread completeCheckThread = new Thread(new ThreadStart(DefinitionWordCheck));
		completeCheckThread.Start();
	}

	static private void DefinitionWordCheck()
	{
		string progressMessageFormat = "Finding definitions. {0}/{1} ({2:n2}%)";

		while (sWordsChecked < sWordsToCheck)
		{
			Thread.Sleep(1000);

			float checkedPercentage = ((float)sWordsChecked / sWordsToCheck) * 100;
			ODebug.Log(string.Format(progressMessageFormat, sWordsChecked, sWordsToCheck, checkedPercentage));
		}

		sWords = null;

		float foundPercentage = ((float)sWordDefinitionsFound / sWordsToCheck) * 100;
		ODebug.Log(string.Format("Found {0}/{1} definitions ({2:n2}%)", sWordDefinitionsFound, sWordsToCheck, foundPercentage));
	}

	static private void GetWordDefinitionsForPortion(object data)
	{
		Words.WordListPortion portion = (Words.WordListPortion)data;

		for (int i = portion.StartIndex; i < portion.EndIndex; ++i)
		{
			Word word = sWords.WordList[i];

			const string urlFormat = "http://api.pearson.com/v2/dictionaries/laad3/entries?headword={0}&limit=1";
			string url = string.Format(urlFormat, word.ActualWord);

			System.Net.WebRequest request = System.Net.WebRequest.Create(url);
			System.Net.WebResponse response = request.GetResponse();

			Stream stream = response.GetResponseStream();
			StreamReader reader = new StreamReader(stream);

			try
			{
				JSONNode node = JSON.Parse(reader.ReadToEnd());
				string foundDefinition = node["results"][0]["senses"][0]["definition"];

				// Drop all of the characters until the first letter is found
				int firstLetterIndex = 0;
				while (!char.IsLetter(foundDefinition[firstLetterIndex]))
				{
					++firstLetterIndex;
				}
				foundDefinition = foundDefinition.Substring(firstLetterIndex);

				// Convert the first char into uppercase
				foundDefinition = char.ToUpper(foundDefinition[0]) + foundDefinition.Substring(1, foundDefinition.Length - 1);

				// Add a trailing "."
				if (!foundDefinition.EndsWith("."))
				{
					foundDefinition += ".";
				}

				word.Definition = foundDefinition;
				++sWordsChecked;
				++sWordDefinitionsFound;
			}
			catch
			{
				++sWordsChecked;
			}
		}
	}
}