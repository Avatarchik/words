using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordPanelGroupTitle : MonoBehaviour, IPointerClickHandler
{
	public Text TextRef;
	public string TitleFormat;

	private WordPanel mWordPanelRef;
	private WordPanelGroup mWordGroupRef;

	public Image CollapsedIconRef;
	public Image CompleteIconRef;

	public void Initialise(WordPanel panelRef, WordPanelGroup wordGroupRef, int groupWordLength)
	{
		mWordPanelRef = panelRef;
		mWordGroupRef = wordGroupRef;
		TextRef.text = string.Format(TitleFormat, groupWordLength);

		CollapsedIconRef.gameObject.SetActive(false);
		CompleteIconRef.gameObject.SetActive(false);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			bool isComplete;
			bool isCollapsed = mWordPanelRef.ToggleGroup(mWordGroupRef, out isComplete);
			SetCollapsedIconsState(isCollapsed, isComplete);
		}
	}

	public void SetCollapsedIconsState(bool isCollapsed, bool isComplete)
	{
		CollapsedIconRef.gameObject.SetActive(isCollapsed && !isComplete);
		CompleteIconRef.gameObject.SetActive(isCollapsed && isComplete);
	}
}