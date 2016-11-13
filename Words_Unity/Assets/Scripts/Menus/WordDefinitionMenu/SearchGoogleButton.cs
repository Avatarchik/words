using UnityEngine;
using UnityEngine.EventSystems;

public class SearchGoogleButton : MonoBehaviour, IPointerClickHandler
{
	public string URLFormat;
	private string mSearchWord;

	public void Initialise(string searchWord)
	{
		mSearchWord = searchWord.ToLower();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			string searchURL = string.Format(URLFormat, mSearchWord);
			Application.OpenURL(searchURL);
		}
	}
}