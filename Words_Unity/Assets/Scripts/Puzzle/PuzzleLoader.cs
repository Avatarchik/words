using UnityEngine;
using System.Collections.Generic;

[ScriptOrder(-100)]
public class PuzzleLoader : UIMonoBehaviour
{
	public GameObject CharacterTilePrefab;
	public WordPanel WordPanelRef;
	public ColourPanel ColourPanelRef;

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

		Vector3 gridSize = new Vector3(0, (mSize * 24) + ((mSize - 1) * 8), 0); // TODO - fix the literals
		Vector3 halfGridSize = (gridSize * 0.5f) - new Vector3(12, 12, 0); // TODO - fix the literals

		mCharacterTilesGrid = new CharacterTile[mSize, mSize];

		for (int x = 0; x < mSize; ++x)
		{
			for (int y = 0; y < mSize; ++y)
			{
				GameObject newTileGO = Instantiate(CharacterTilePrefab, Vector3.zero, Quaternion.identity, transform) as GameObject;

				CharacterTile characterTile = newTileGO.GetComponent<CharacterTile>();
				characterTile.transform.localPosition = new Vector3(x * 32, y * 32, 0) - halfGridSize; // TODO - fix the literals

				CharacterUsage charUsage = contentsToLoad.CharGrid[(x * mSize) + y];
				characterTile.Initialise(this, charUsage, new GridPosition(x, y));

				mCharacterTilesGrid[x, y] = characterTile;
			}
		}

		// Scale accordingly
		rectTransform.localScale = new Vector3(16f / mSize, 16f / mSize, 1); // TODO - fix the literals

		WordPanelRef.gameObject.SetActive(true);
		WordPanelRef.Initialise(contentsToLoad.Words);

		ColourPanelRef.Initialise(contentsToLoad.MaxCharacterUsage);
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
}