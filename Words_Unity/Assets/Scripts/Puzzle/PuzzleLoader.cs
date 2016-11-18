using UnityEngine;
using System.Collections.Generic;

[ScriptOrder(-100)]
public class PuzzleLoader : UIMonoBehaviour
{
	public GameObject CharacterTilePrefab;
	public WordPanel WordPanelRef;
	public ColourPanel ColourPanelRef;
	public LastSelectionChecked LastSelectionCheckedRef;

	private CharacterTile[,] mCharacterTilesGrid;

	private int mSize;

	static public PuzzleContents sActivePuzzleContents;

	void OnEnable()
	{
		ColourSchemeManager.OnSchemeSwitched += OnSchemeSwitched;
	}

	void OnDisable()
	{
		ColourSchemeManager.OnSchemeSwitched -= OnSchemeSwitched;
	}

	public void LoadPuzzle(PuzzleContents contentsToLoad)
	{
		CleanUp();

		sActivePuzzleContents = contentsToLoad;

		mSize = contentsToLoad.Size;

		Vector3 gridSize = new Vector3(0, (mSize * GlobalSettings.TileSize) + ((mSize - 1) * GlobalSettings.TileSpacing), 0);
		Vector3 halfGridSize = (gridSize * 0.5f) - new Vector3(GlobalSettings.TileSizeHalf, GlobalSettings.TileSizeHalf, 0);

		mCharacterTilesGrid = new CharacterTile[mSize, mSize];

		for (int x = 0; x < mSize; ++x)
		{
			for (int y = 0; y < mSize; ++y)
			{
				GameObject newTileGO = Instantiate(CharacterTilePrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;

				CharacterTile characterTile = newTileGO.GetComponent<CharacterTile>();
				characterTile.transform.localPosition = new Vector3(x * GlobalSettings.TileSizeWithSpacing, y * GlobalSettings.TileSizeWithSpacing, 0) - halfGridSize;

				CharacterUsage charUsage = contentsToLoad.CharGrid[(x * mSize) + y];
				characterTile.Initialise(this, charUsage, new GridPosition(x, y));

				mCharacterTilesGrid[x, y] = characterTile;
			}
		}

		// Scale accordingly
		rectTransform.localScale = new Vector3(16f / mSize, 16f / mSize, 1);

		WordPanelRef.gameObject.SetActive(true);
		WordPanelRef.Initialise(contentsToLoad.Words);

		ColourPanelRef.Initialise(contentsToLoad.MaxCharacterUsage);

		LastSelectionCheckedRef.Reset();

		int[] charUsesRemainingSavedState = SaveGameManager.Instance.ActivePuzzleState.CharUsageLeftStates;
		int charCount = mSize * mSize;
		for (int charStateIndex = 0; charStateIndex < charCount; ++charStateIndex)
		{
			int usageLeft = charUsesRemainingSavedState[charStateIndex];

			if (usageLeft != -1)
			{
				int x = charStateIndex / mSize;
				int y = charStateIndex % mSize;

				mCharacterTilesGrid[x, y].SetUsage(usageLeft);
			}
		}
	}

	public void CleanUp()
	{
		rectTransform.localScale = Vector3.one;

		if (mCharacterTilesGrid != null)
		{
			for (int x = 0; x < mSize; ++x)
			{
				for (int y = 0; y < mSize; ++y)
				{
					if (mCharacterTilesGrid[x, y] != null)
					{
						Destroy(mCharacterTilesGrid[x, y].gameObject);
					}
				}
			}
			mCharacterTilesGrid = null;
		}

		WordPanelRef.gameObject.SetActive(false);
	}

	public void RemoveTile(GridPosition position)
	{
		mCharacterTilesGrid[position.X, position.Y] = null;
	}

	private void OnSchemeSwitched(ColourScheme newScheme)
	{
		UpdateTileBackgroundColours();
	}

	private void UpdateTileBackgroundColours()
	{
		for (int x = 0; x < mSize; ++x)
		{
			for (int y = 0; y < mSize; ++y)
			{
				mCharacterTilesGrid[x, y].UpdateBackgroundColour();
			}
		}
	}

	public void GetTilesBetween(GridPosition fromPosition, GridPosition toPosition, ref List<CharacterTile> tiles)
	{
		CharacterTile fromTile = mCharacterTilesGrid[fromPosition.X, fromPosition.Y];
		CharacterTile toTile = mCharacterTilesGrid[toPosition.X, toPosition.Y];
		GetTilesBetween(fromTile, toTile, ref tiles);
	}

	public void GetTilesBetween(CharacterTile fromTile, CharacterTile toTile, ref List<CharacterTile> tiles)
	{
		tiles.Clear();

		int xDelta = toTile.Position.X - fromTile.Position.X;
		int yDelta = toTile.Position.Y - fromTile.Position.Y;
		int xModifier = MathfHelper.ClampM11(xDelta);
		int yModifier = MathfHelper.ClampM11(yDelta);

		bool isCardinal = false;
		isCardinal |= xModifier != 0 && yModifier == 0;
		isCardinal |= xModifier == 0 && yModifier != 0;
		isCardinal |= Mathf.Abs(xDelta) == Mathf.Abs(yDelta);

		if (!isCardinal)
		{
			tiles.Add(mCharacterTilesGrid[fromTile.Position.X, fromTile.Position.Y]);
			return;
		}

		GridPosition pos = new GridPosition(fromTile.Position);
		GridPosition endPos = new GridPosition(toTile.Position);
		endPos.X += xModifier;
		endPos.Y += yModifier;

		int checkedEntries = 0;
		do
		{
			CharacterTile tile = mCharacterTilesGrid[pos.X, pos.Y];
			if (tile == null)
			{
				break;
			}

			tiles.Add(tile);

			pos.X += xModifier;
			pos.Y += yModifier;

			++checkedEntries;
		}
		while (pos != endPos && checkedEntries < mSize);
	}

	public int GetCurrentPuzzleSize()
	{
		return mSize;
	}
}