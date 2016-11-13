using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordPanelGroupTitle : MonoBehaviour, IPointerClickHandler
{
	public Text TextRef;
	public string TitleFormat;

	private WordPanel mWordPanelRef;
	private WordPanelGroup mWordGroupRef;

	public void Initialise(WordPanel panelRef, WordPanelGroup wordGroupRef, int groupWordLength)
	{
		mWordPanelRef = panelRef;
		mWordGroupRef = wordGroupRef;
		TextRef.text = string.Format(TitleFormat, groupWordLength);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			mWordPanelRef.ToggleGroup(mWordGroupRef);
		}
	}
}