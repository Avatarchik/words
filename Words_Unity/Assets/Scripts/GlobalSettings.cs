using UnityEngine;
using System;

[ScriptOrder(-9999)]
public class GlobalSettings : SingletonMonoBehaviour<GlobalSettings>
{
	static public readonly DateTime kEpoch = new DateTime(2016, 11, 27);

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

	public Color UIHightlightColour;
	public Color UIDisabledHighlightColour;

	public Color WordFoundColour;
	public Color WordNotFoundColour;
	public Color WordAlreadyFoundColour;
	public Color WordWrongInstanceColour;

	public int DefaultPuzzleLandscapeLayout = 1;
	public int DefaultPuzzlePortraitLayout = 0;

	public SerializableGuid PotDFixedGuid = "01234567-89ab-cdef-0123-456789abcdef";

	private void Awake()
	{
		PuzzleSizeMaxPlusOne = PuzzleSizeMax + 1;
		PuzzleSizeDifference = PuzzleSizeMax - PuzzleSizeMin;

		TileSizeHalf = TileSize / 2;
		TileSizeWithSpacing = TileSize + TileSpacing;
	}
}