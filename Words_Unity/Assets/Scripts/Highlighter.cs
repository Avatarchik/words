using UnityEngine;
using System.Collections.Generic;

public class Highlighter : SingletonMonoBehaviour<Highlighter>
{
	public PuzzleLoader PuzzleLoaderRef;
	public WordPanel WordPanelRef;
	public LastSelectionChecked LastSelectionCheckedRef;
	public EffectsManager EffectsManagerRef;

	private GameObject mFrom;
	private GameObject mTo;

	private List<CharacterTile> mHighlightedTiles = new List<CharacterTile>();

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

		if (mFrom == null)
		{
			return;
		}

		CharacterTile fromTile = mFrom.GetComponent<CharacterTile>();
		if (mTo != null)
		{
			if (mFrom != mTo)
			{
				CharacterTile toTile = mTo.GetComponent<CharacterTile>();
				PuzzleLoaderRef.GetTilesBetween(fromTile, toTile, ref mHighlightedTiles);
			}
			else
			{
				mHighlightedTiles.Add(fromTile);
			}
		}

		foreach (CharacterTile tile in mHighlightedTiles)
		{
			tile.SetHighlight(true);
		}
	}

	public void CheckHighlightedValidity()
	{
		//ODebug.Log(string.Format("From: {0} To: {1}", mFrom.name, mTo.name));

		string wordFromHighlightedTiles = GetWordFromHighlightedTiles();

		LastSelectionCheckedRef.SetSelection(wordFromHighlightedTiles, mHighlightedTiles);

		WordValidityResult result = WordPanelRef.CheckWordValidity(wordFromHighlightedTiles, mHighlightedTiles);

		EWordValidityResult overallResult = EWordValidityResult.NoMatch;
		if (result.IsWrongInstance)
		{
			overallResult = EWordValidityResult.WrongInstance;
			WordPanelFlasher.Instance.Flash(WordPanelFlasher.EFlashReason.WrongInstance);
		}
		else if (result.WordsFound > 0)
		{
			overallResult = EWordValidityResult.WasRemoved;
			WordPanelFlasher.Instance.Flash(WordPanelFlasher.EFlashReason.Found);
		}
		else if (result.WordsAlreadyFound > 0)
		{
			overallResult = EWordValidityResult.WasAlreadyFound;
			WordPanelFlasher.Instance.Flash(WordPanelFlasher.EFlashReason.AlreadyFound);
		}
		else
		{
			WordPanelFlasher.Instance.Flash(WordPanelFlasher.EFlashReason.NotFound);
		}

		// Play an effect
		Vector3 effectStart = mHighlightedTiles[0].transform.position;
		effectStart.z -= 5;
		Vector3 effectEnd = effectStart;
		if (mHighlightedTiles.Count > 1)
		{
			effectEnd = mHighlightedTiles[mHighlightedTiles.Count - 1].transform.position;
			effectEnd.z -= 5;
		}
		EffectsManagerRef.PlayWordValidityEffectAt(overallResult, effectStart, effectEnd);

		// Add score. Possibly negative if the word wasn't a valid word
		int score = 0;
		if (result.WordsFound > 0)
		{
			score = result.TileDecrements * result.WordsFound * 10;
		}
		else
		{
			if (overallResult != EWordValidityResult.WasAlreadyFound && !result.WasInvalidLength && !result.IsWrongInstance)
			{
				score = wordFromHighlightedTiles.Length * -50;
			}
		}
		ScoreManager.Instance.AddScore(score);

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