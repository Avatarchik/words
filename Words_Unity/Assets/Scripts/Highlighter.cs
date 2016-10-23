using UnityEngine;
using System.Collections.Generic;

public class Highlighter : SingletonMonoBehaviour<Highlighter>
{
	public PuzzleLoader PuzzleLoaderRef;
	public WordPanel WordPanelRef;
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
		EWordValidityResult result = WordPanelRef.CheckWordValidity(wordFromHighlightedTiles, mHighlightedTiles);

		foreach (CharacterTile tile in mHighlightedTiles)
		{
			Vector3 effectPosition = tile.transform.position;
			effectPosition.z -= 5;

			switch (result)
			{
				case EWordValidityResult.WasRemoved:
					EffectsManagerRef.PlayFoundEffectAt(effectPosition);
					break;

				case EWordValidityResult.WrongInstance:
					EffectsManagerRef.PlayWrongInstanceEffectAt(effectPosition);
					break;

				case EWordValidityResult.WasAlreadyFound:
					EffectsManagerRef.PlayAlreadyFoundEffectAt(effectPosition);
					break;

				default:
					EffectsManagerRef.PlayNotFoundEffectAt(effectPosition);
					break;
			}
		}

		// Add score
		ScoreManager.Instance.AddScore(mHighlightedTiles.Count * 10);

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