using UnityEngine;
using System.Collections.Generic;

public class WordHighlighter : MonoBehaviour
{
	static public WordHighlighter Instance { get; private set; }

	public WordPanel WordPanelComp;

	private GameObject mFrom;
	private GameObject mTo;

	private List<GridEntry> mHighlightedTiles = new List<GridEntry>();

	void Awake()
	{
		if (Instance != null)
		{
			Destroy(gameObject);
			return;
		}

		Instance = this;
	}

	public GameObject GetFrom()
	{
		return mFrom;
	}

	public GameObject GetTo()
	{
		return mTo;
	}

	public void SetFrom(GameObject from)
	{
		mFrom = from;
		CorrectHighlighting();
	}

	public void SetTo(GameObject to)
	{
		mTo = to;
		CorrectHighlighting();
	}

	private void CorrectHighlighting()
	{
		foreach (GridEntry tile in mHighlightedTiles)
		{
			tile.RemoveTint();
		}
		mHighlightedTiles.Clear();

		if (mFrom == null || mTo == null)
		{
			return;
		}

		GridPositionReference fromPosition = mFrom.transform.GetComponent<GridPositionReference>();
		GridPositionReference toPosition = mTo.transform.GetComponent<GridPositionReference>();

		Generator.Instance.GetWord(fromPosition, toPosition, ref mHighlightedTiles);

		foreach (GridEntry tile in mHighlightedTiles)
		{
			tile.AddTint();
		}
	}

	public void CheckHighlightedValidity()
	{
		Debug.Log(string.Format("From: {0} To: {1}", mFrom.name, mTo.name));

		GridPositionReference fromPosition = mFrom.transform.GetComponent<GridPositionReference>();
		GridPositionReference toPosition = mTo.transform.GetComponent<GridPositionReference>();

		List<GridEntry> tiles = new List<GridEntry>();
		string word = Generator.Instance.GetWord(fromPosition, toPosition, ref tiles);
		if (WordPanelComp.RemoveWordIfExists(word))
		{
			Generator.Instance.DecrementCharacterCount(tiles);
		}

		SetFrom(null);
		SetTo(null);
	}
}