using UnityEngine;
using System.Collections.Generic;

public class LastSelectionChecked : MonoBehaviour
{
	public LastSelectionTile[] LetterTiles;

	private int mTileCount;

	void Awake()
	{
		mTileCount = LetterTiles.Length;
		Reset();
	}

	public void Reset()
	{
		for (int tileIndex = 0; tileIndex < mTileCount; ++tileIndex)
		{
			LetterTiles[tileIndex].SetVisibility(false);
		}
	}

	public void SetSelection(string selection, List<CharacterTile> mHighlightedTiles)
	{
		int selectionLength = selection.Length;

		LastSelectionTile tile;
		for (int tileIndex = 0; tileIndex < selectionLength; ++tileIndex)
		{
			tile = LetterTiles[tileIndex];

			tile.SetVisibility(true);
			tile.SetText(selection[tileIndex]);
			tile.SetColour(mHighlightedTiles[tileIndex].GetBackgroundColour());
		}

		for (int tileIndex = selectionLength; tileIndex < mTileCount; ++tileIndex)
		{
			tile = LetterTiles[tileIndex];
			tile.SetVisibility(false);
		}
	}
}