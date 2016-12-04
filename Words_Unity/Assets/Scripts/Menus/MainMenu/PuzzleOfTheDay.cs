using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using System.Collections;

using UnityEngine.Networking;

public class PuzzleOfTheDay : MonoBehaviour, IPointerClickHandler
{
	public Button ButtonRef;
	public PuzzleManager PuzzleManagerRef;

	public string GAEURLFormat;

	private AssetBundle mAssetBundle;

	void Awake()
	{
		string platform = null;
#if UNITY_IOS
		platform = "iOS";
#elif UNITY_ANDROID
		platform = "Android";
#endif

		if (platform != null)
		{
			GAEURLFormat = GAEURLFormat.Replace("#PLATFORM#", platform);
		}
		else
		{
			ODebug.LogWarning("Failed to find platform");
		}

		ButtonRef.interactable = false;

		CleanUpPrevious();
		StartCoroutine("FetchPoTD");
	}

	private void CleanUpPrevious()
	{
		int daysSinceEpoch = (DateTime.Today - GlobalSettings.kEpoch).Days;

		int lastRetrievedDayStamp = PlayerPrefsPlus.GetInt(PlayerPrefKeys.PotDLastRetrievedDayStamp, -1);
		if (lastRetrievedDayStamp != daysSinceEpoch)
		{
			PlayerPrefsPlus.SetInt(PlayerPrefKeys.PotDLastRetrievedDayStamp, -1);
			PlayerPrefsPlus.Save();

			SaveGameManager.Instance.ResetPuzzleState(GlobalSettings.Instance.PotDFixedGuid);
		}
	}

	private IEnumerator FetchPoTD()
	{
		string url = string.Format(GAEURLFormat, DateTime.Today.ToString("yyyy_MM_dd"));
		UnityWebRequest www = UnityWebRequest.GetAssetBundle(url);
		yield return www.Send();

		ButtonRef.interactable = !www.isError;
		if (www.isError)
		{
			mAssetBundle = null;
			ODebug.LogWarning(string.Format("Failed to get Puzzle of the Day. Error: " + www.error));
		}
		else
		{
			mAssetBundle = DownloadHandlerAssetBundle.GetContent(www);

			PuzzleContents potdContents = mAssetBundle.LoadAsset<PuzzleContents>(mAssetBundle.name + ".asset");
			PuzzleManagerRef.RegisterPuzzleOfTheDay(potdContents);

			int daysSinceEpoch = (DateTime.Today - GlobalSettings.kEpoch).Days;
			PlayerPrefsPlus.SetInt(PlayerPrefKeys.PotDLastRetrievedDayStamp, daysSinceEpoch);
			PlayerPrefsPlus.Save();

			AnalyticsManager.Instance.SendPuzzleOfTheDayDownloaded();
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			PuzzleState state = SaveGameManager.Instance.GetPuzzleStateFor(GlobalSettings.Instance.PotDFixedGuid);

			if (!state.IsCompleted)
			{
				TimeManager.Instance.Reset();
				ScoreManager.Instance.Reset();

				PuzzleManagerRef.OpenPuzzleOfTheDay();
				MenuManager.Instance.SwitchMenu(EMenuType.InGameMenu, OnMenuSwitched);
			}
			else
			{
				MenuManager.Instance.SwitchTemporaryMenu(EMenuType.DailyPuzzleCompleteMenu);
			}
		}
	}

	private void OnMenuSwitched()
	{
		TimeManager.Instance.Start();
	}
}