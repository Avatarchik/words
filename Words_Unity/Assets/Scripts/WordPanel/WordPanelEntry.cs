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
	public GridPosition FromPosition { get; private set; }
	public GridPosition ToPosition { get; private set; }

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

		FromPosition = wordPair.FromPosition;
		ToPosition = wordPair.ToPosition;
	}

	public EWordValidityResult DoesMatchSelection(string word, string reversedWord, CharacterTile startTile, CharacterTile endTile, out bool isCompleteMatch)
	{
		bool stringsMatch = (mWord == word) || (mWord == reversedWord);
		bool stringContains = false;
		if (!stringsMatch)
		{
			stringContains = word.Contains(mWord) || reversedWord.Contains(mWord);
		}

		bool forwardsPositionsMatch = false;
		bool backwardsPositionsMatch = false;
		bool isBetweenTiles = false;
		if (stringsMatch)
		{
			forwardsPositionsMatch = (FromPosition == startTile.Position) && (ToPosition == endTile.Position);
			backwardsPositionsMatch = (FromPosition == endTile.Position) && (ToPosition == startTile.Position);
		}
		else if (stringContains)
		{
			int x0 = Mathf.Min(startTile.Position.X, endTile.Position.X);
			int x1 = Mathf.Max(startTile.Position.X, endTile.Position.X);
			int y0 = Mathf.Min(startTile.Position.Y, endTile.Position.Y);
			int y1 = Mathf.Max(startTile.Position.Y, endTile.Position.Y);

			int x2 = Mathf.Min(FromPosition.X, ToPosition.X);
			int x3 = Mathf.Max(FromPosition.X, ToPosition.X);
			int y2 = Mathf.Min(FromPosition.Y, ToPosition.Y);
			int y3 = Mathf.Max(FromPosition.Y, ToPosition.Y);

			isBetweenTiles = (x2 >= x0) && (x3 <= x1) && (y2 >= y0) && (y3 <= y1);
		}

		isCompleteMatch = stringsMatch && (forwardsPositionsMatch || backwardsPositionsMatch);
		bool partialMatch = stringContains && isBetweenTiles;

		EWordValidityResult result = EWordValidityResult.NoMatch;
		if (isCompleteMatch || partialMatch)
		{
			result = EWordValidityResult.Match;
		}
		else if (stringContains)
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