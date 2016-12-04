using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Collections;

public class WordOfTheDay : UIMonoBehaviour
{
	public Text TextRef;
	public string TextFormat;

	public string GAEURLFormat;

	void Awake()
	{
		TextRef.enabled = false;
		TextFormat = TextFormat.Replace("\\n", Environment.NewLine);
	}

	void OnEnable()
	{
		int daysSinceEpoch = (DateTime.Today - GlobalSettings.kEpoch).Days;

		int lastRetrievedDayStamp = PlayerPrefsPlus.GetInt(PlayerPrefKeys.WotDLastRetrievedDayStamp, -1);
		if (lastRetrievedDayStamp != -1)
		{
			if (lastRetrievedDayStamp == daysSinceEpoch)
			{
				string word = PlayerPrefsPlus.GetString(PlayerPrefKeys.WotDLastRetrievedWord);
				string definition = PlayerPrefsPlus.GetString(PlayerPrefKeys.WotDLastRetrievedDefinition);
				UpdateText(word, definition);
			}
			else
			{
				StartCoroutine("FetchWoTD");
			}
		}
		else
		{
			StartCoroutine("FetchWoTD");
		}
	}

	private IEnumerator FetchWoTD()
	{
		int daysSinceEpoch = (DateTime.Today - GlobalSettings.kEpoch).Days;

		string url = string.Format(GAEURLFormat, daysSinceEpoch);
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.Send();

		if (www.isError)
		{
			ODebug.LogWarning(string.Format("Failed to get Word of the Day. Error: " + www.error));
		}
		else
		{
			string[] wotdSplit = www.downloadHandler.text.Split(new string[] { "##" }, System.StringSplitOptions.None);
			if (wotdSplit.Length == 3)
			{
				string word = wotdSplit[1];
				string definition = wotdSplit[2];

				if (!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(definition))
				{
					UpdateText(word, definition);

					PlayerPrefsPlus.SetInt(PlayerPrefKeys.WotDLastRetrievedDayStamp, daysSinceEpoch);
					PlayerPrefsPlus.SetString(PlayerPrefKeys.WotDLastRetrievedWord, word);
					PlayerPrefsPlus.SetString(PlayerPrefKeys.WotDLastRetrievedDefinition, definition);
					PlayerPrefsPlus.Save();
				}
			}
		}
	}

	private void UpdateText(string word, string definition)
	{
		TextRef.enabled = true;
		TextRef.text = string.Format(TextFormat, word, definition);

		Vector2 sizeDelta = rectTransform.sizeDelta;
		sizeDelta.y = TextRef.preferredHeight;
		rectTransform.sizeDelta = sizeDelta;
	}
}