using UnityEngine;

[ScriptOrder(-9999)]
public class GlobalSettings : SingletonMonoBehaviour<GlobalSettings>
{
	public int PuzzleSizeMin = 4;
	public int PuzzleSizeMax = 12;
	[HideInInspector]
	public int PuzzleSizeMaxPlusOne;
	[HideInInspector]
	public int PuzzleSizeDifference;
	public int PuzzleSizeMaxTileUsage = 12;

	public int TileSize = 24;
	[HideInInspector]
	public int TileSizeHalf;
	public int TileSpacing = 8;
	[HideInInspector]
	public int TileSizeWithSpacing;

	public Color FoundColour;
	public Color NotFoundColour;
	public Color AlreadyFoundColour;
	public Color WrongInstanceColour;

	private void Awake()
	{
		PuzzleSizeMaxPlusOne = PuzzleSizeMax + 1;
		PuzzleSizeDifference = PuzzleSizeMax - PuzzleSizeMin;

		TileSizeHalf = TileSize / 2;
		TileSizeWithSpacing = TileSize + TileSpacing;
	}
}