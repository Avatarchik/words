using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class WordOfTheDay : UIMonoBehaviour
{
	private DateTime kEpoch = new DateTime(2016, 11, 27);
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
		int daysSinceEpoch = (DateTime.Today - kEpoch).Days;

		int lastRetrievedDayStamp = PlayerPrefsPlus.GetInt(PlayerPrefKeys.WotDLastRetrievedWordDayStamp, -1);
		if (lastRetrievedDayStamp != -1)
		{
			if (lastRetrievedDayStamp == daysSinceEpoch)
			{
				string word = PlayerPrefsPlus.GetString(PlayerPrefKeys.WotDLastRetrievedWordWord);
				string definition = PlayerPrefsPlus.GetString(PlayerPrefKeys.WotDLastRetrievedWordDefinition);
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
		int daysSinceEpoch = (DateTime.Today - kEpoch).Days;

		string url = string.Format(GAEURLFormat, daysSinceEpoch);

		WWW wotdFetch = new WWW(url);
		yield return wotdFetch;

		if (wotdFetch.error == null)
		{
			string[] wotdSplit = wotdFetch.text.Split(new string[] { "##" }, System.StringSplitOptions.None);
			if (wotdSplit.Length == 3)
			{
				string word = wotdSplit[1];
				string definition = wotdSplit[2];

				if (!string.IsNullOrEmpty(word) && !string.IsNullOrEmpty(definition))
				{
					UpdateText(word, definition);

					PlayerPrefsPlus.SetInt(PlayerPrefKeys.WotDLastRetrievedWordDayStamp, daysSinceEpoch);
					PlayerPrefsPlus.SetString(PlayerPrefKeys.WotDLastRetrievedWordWord, word);
					PlayerPrefsPlus.SetString(PlayerPrefKeys.WotDLastRetrievedWordDefinition, definition);
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