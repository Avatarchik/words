using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordPanelEntry : MonoBehaviour
	, IPointerClickHandler
{
	public Text TextRef;
	public RectTransform StrikeThrough;

	public bool HasBeenFound { get; private set; }

	private string mWord;
	private GridPosition mFromPosition;
	private GridPosition mToPosition;

	public float DoubleTapInterval = 0.4f;
	private float mLastTapTime = 0;

	void Awake()
	{
		HasBeenFound = false;
	}

	public void Initialise(WordPair wordPair)
	{
		mWord = wordPair.Forwards;
		TextRef.text = mWord;

		mFromPosition = wordPair.FromPosition;
		mToPosition = wordPair.ToPosition;
	}

	public EWordValidityResult DoesMatchSelection(string word, string reversedWord, CharacterTile startTile, CharacterTile endTile)
	{
		bool stringsMatch = (mWord == word) || (mWord == reversedWord);
		bool forwardsPositionsMatch = (mFromPosition == startTile.Position) && (mToPosition == endTile.Position);
		bool backwardsPositionsMatch = (mFromPosition == endTile.Position) && (mToPosition == startTile.Position);

		bool matchesCompletely = stringsMatch && (forwardsPositionsMatch || backwardsPositionsMatch);

		EWordValidityResult result = EWordValidityResult.NoMatch;
		if (matchesCompletely)
		{
			result = EWordValidityResult.CompleteMatch;
		}
		else if (stringsMatch)
		{
			result = EWordValidityResult.WrongInstance;
		}

		return result;
	}

	public void MarkWordAsFound()
	{
		HasBeenFound = true;

		StrikeThrough.gameObject.SetActive(true);
		StrikeThrough.sizeDelta = new Vector2(TextRef.preferredWidth * 1.2f, StrikeThrough.sizeDelta.y);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			// Double tap logic
			float timeNow = Time.time;
			if ((timeNow - mLastTapTime) <= DoubleTapInterval)
			{
				ShowWordDefinition();
			}
			mLastTapTime = timeNow;
		}
	}

	private void ShowWordDefinition()
	{
		TimeManager.Instance.Stop();
		MenuManager.Instance.SwitchTemporaryMenu(EMenuType.WordDefinitionMenu, OnMenuSwitched);
	}

	private void OnMenuSwitched()
	{
		// TODO - this is bad
		WordDefinitionViewer defViewer = FindObjectOfType<WordDefinitionViewer>();
		if (defViewer)
		{
			defViewer.ShowDefinitionFor(mWord);
		}
	}
}