using UnityEngine;
using System.Collections.Generic;

public class WordHighlighter : MonoBehaviour
{
	static public WordHighlighter Instance { get; private set; }

	public PuzzleLoader PuzzleLoaderRef;
	public WordPanel WordPanelRef;
	public EffectsManager EffectsManagerRef;

	private GameObject mFrom;
	private GameObject mTo;

	private List<CharacterTile> mHighlightedTiles = new List<CharacterTile>();

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
		foreach (CharacterTile tile in mHighlightedTiles)
		{
			tile.SetHighlight(false);
		}
		mHighlightedTiles.Clear();

		if (mFrom == null || mTo == null)
		{
			return;
		}

		CharacterTile fromTile = mFrom.GetComponent<CharacterTile>();
		CharacterTile toTile = mTo.GetComponent<CharacterTile>();

		PuzzleLoaderRef.GetTilesBetween(fromTile, toTile, ref mHighlightedTiles);

		foreach (CharacterTile tile in mHighlightedTiles)
		{
			tile.SetHighlight(true);
		}
	}

	public void CheckHighlightedValidity()
	{
		Debug.Log(string.Format("From: {0} To: {1}", mFrom.name, mTo.name));

		string wordFromHighlightedTiles = GetWordFromHighlightedTiles();
		bool wasWordRemoved;
		bool wasWordAlreadyFound;
		WordPanelRef.CheckWordValidity(wordFromHighlightedTiles, mHighlightedTiles, out wasWordRemoved, out wasWordAlreadyFound);

		foreach (CharacterTile tile in mHighlightedTiles)
		{
			if (wasWordRemoved)
			{
				EffectsManagerRef.PlayFoundEffectAt(tile.transform.position);
			}
			else
			{
				if (wasWordAlreadyFound)
				{
					EffectsManagerRef.PlayAlreadyFoundEffectAt(tile.transform.position);
				}
				else
				{
					EffectsManagerRef.PlayNotFoundEffectAt(tile.transform.position);
				}
			}
		}

		SetFrom(null);
		SetTo(null);
	}

	private string GetWordFromHighlightedTiles()
	{
		string word = string.Empty;

		foreach (CharacterTile tile in mHighlightedTiles)
		{
			word += tile.Character;
		}

		return word;
	}
}