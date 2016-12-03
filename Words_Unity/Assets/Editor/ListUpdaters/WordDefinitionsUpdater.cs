using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Threading;
using SimpleJSON;

public class WordDefinitionsUpdater
{
	static private Words sWords;
	static private WordDefinitions sDefinitions;
	static private int sWordsToCheck;
	static private int sWordsChecked;
	static private int sWordDefinitionsFound;

	[MenuItem("Words/List Updaters/Definitions")]
	static void UpdateDefinitions()
	{
		bool findDefinitions = EditorUtility.DisplayDialog(
			"Word List Updater",
			"Do you want to find the definitions too? It will take some time.\n\nThe progress will appear in the console window.",
			"Yes", "No");

		if (!findDefinitions)
		{
			return;
		}

		GameObject wordsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Words.prefab");
		GameObject wordDefinitionsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WordDefinitions.prefab");
		if (wordDefinitionsPrefab)
		{
			sWords = wordsPrefab.GetComponent<Words>();
			sDefinitions = wordDefinitionsPrefab.GetComponent<WordDefinitions>();
			if (sWords && sDefinitions)
			{
				sDefinitions.Initialise(sWords.WordCount);
				GetDefinitions();
				EditorUtility.SetDirty(sDefinitions.gameObject);
			}
		}
	}

	[MenuItem("Words/List Updaters/Definitions - Clean only")]
	static void CleanDefinitions()
	{
		GameObject wordDefinitionsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WordDefinitions.prefab");
		if (wordDefinitionsPrefab)
		{
			sDefinitions = wordDefinitionsPrefab.GetComponent<WordDefinitions>();
			if (sDefinitions)
			{
				for (int definitionIndex = 0; definitionIndex < sDefinitions.DefinitionCount; ++definitionIndex)
				{
					CleanDefinition(ref sDefinitions.DefinitionList[definitionIndex]);
				}

				ODebug.Log("Definitions cleaned");

				EditorUtility.SetDirty(sDefinitions.gameObject);
				sDefinitions = null;
			}
		}
	}

	[MenuItem("Words/List Updaters/Export Definitions For GAE")]
	static void ExportDefinitions()
	{
		GameObject wordDefinitionsPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/WordDefinitions.prefab");
		if (wordDefinitionsPrefab)
		{
			sDefinitions = wordDefinitionsPrefab.GetComponent<WordDefinitions>();
			if (sDefinitions)
			{
				string path = EditorUtility.OpenFolderPanel("Export Definitions csv", string.Empty, string.Empty);

				if (!string.IsNullOrEmpty(path))
				{
					sDefinitions.DefinitionList.Shuffle();

					StringBuilder sb = new StringBuilder();
					int definitionIndex = 0;
					int definitionsRequired = 100;//365 * 10;

					foreach (WordDefinition definition in sDefinitions.DefinitionList)
					{
						sb.AppendLine(string.Format("\t\tWOTD(daystamp={0}, word='{1}', definition='{2}').put()",
							definitionIndex,
							WordHelper.ConvertToTitleCase(definition.ActualWord),
							definition.Definition));

						++definitionIndex;

						if (definitionIndex >= definitionsRequired)
						{
							break;
						}
					}

					path = Path.Combine(path, "definitions.txt");
					File.WriteAllText(path, sb.ToString());
					ODebug.Log("Exported definitions for GAE: " + path);
				}
			}
		}
	}

	static private void GetDefinitions()
	{
		sWordsToCheck = 0;
		sWordsChecked = 0;
		sWordDefinitionsFound = 0;

		for (int portionIndex = 0; portionIndex < sWords.ListPortions.Count; ++portionIndex) // TODO - this shouldn't create all of them at once
		{
			WordListPortion portion = sWords.ListPortions[portionIndex];
			sWordsToCheck += portion.ContainedWordsCount;

			Thread portionThread = new Thread(new ParameterizedThreadStart(GetWordDefinitionsForPortion));
			portionThread.Start(portion);
		}

		Thread completeCheckThread = new Thread(new ThreadStart(DefinitionWordCheck));
		completeCheckThread.Start();
	}

	static private void DefinitionWordCheck()
	{
		string progressMessageFormat = "Definitions: {0}/{1} ({2:n2}%)";

		while (sWordsChecked < sWordsToCheck)
		{
			Thread.Sleep(1000);

			float checkedPercentage = ((float)sWordsChecked / sWordsToCheck) * 100;
			ODebug.Log(string.Format(progressMessageFormat, sWordsChecked, sWordsToCheck, checkedPercentage));
		}

		float foundPercentage = ((float)sWordDefinitionsFound / sWordsToCheck) * 100;
		ODebug.Log(string.Format("Found {0}/{1} definitions ({2:n2}%)", sWordDefinitionsFound, sWordsToCheck, foundPercentage));

		sDefinitions.Finalise();
		sDefinitions = null;
	}

	static private void GetWordDefinitionsForPortion(object data)
	{
		WordListPortion portion = (WordListPortion)data;

		for (int i = portion.StartIndex; i < portion.EndIndex; ++i)
		{
			string word = sWords.WordList[i];

			const string urlFormat = "http://api.pearson.com/v2/dictionaries/laad3/entries?headword={0}&limit=1";
			string url = string.Format(urlFormat, word);

			System.Net.WebRequest request = System.Net.WebRequest.Create(url);
			System.Net.WebResponse response = request.GetResponse();

			Stream stream = response.GetResponseStream();
			StreamReader reader = new StreamReader(stream, Encoding.UTF8);

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

				CleanDefinition(ref foundDefinition);

				sDefinitions.AddNewDefinition(word, foundDefinition);
				++sWordsChecked;
				++sWordDefinitionsFound;
			}
			catch
			{
				++sWordsChecked;
			}
		}
	}

	static private void CleanDefinition(ref WordDefinition definition)
	{
		CleanDefinition(ref definition.Definition);
	}

	static private void CleanDefinition(ref string foundDefinition)
	{
		// Convert the first char into uppercase
		foundDefinition = char.ToUpper(foundDefinition[0]) + foundDefinition.Substring(1, foundDefinition.Length - 1);

		// Remove surrounding quotes
		if (foundDefinition.StartsWith("\""))
		{
			foundDefinition = foundDefinition.Substring(1, foundDefinition.Length - 1);
		}
		if (foundDefinition.EndsWith("\""))
		{
			foundDefinition = foundDefinition.Substring(0, foundDefinition.Length - 2);
		}

		// Add a trailing "."
		if (!foundDefinition.EndsWith("."))
		{
			foundDefinition += ".";
		}
	}
}