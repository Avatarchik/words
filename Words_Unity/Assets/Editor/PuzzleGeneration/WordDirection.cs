public enum EWordDirection : byte
{
	North = 0,
	NorthEast,
	East,
	SouthEast,
	South,
	SouthWest,
	West,
	NorthWest,

	Count,
	Unknown,
}

public struct WordDirection
{
	static WordDirection[] kMappedDirections = new WordDirection[]
	{
		new WordDirection( 0,  1),	// EWordDirection.North
		new WordDirection( 1,  1),	// EWordDirection.NorthEast
		new WordDirection( 1,  0),	// EWordDirection.East
		new WordDirection( 1, -1),	// EWordDirection.SouthEast
		new WordDirection( 0, -1),	// EWordDirection.South
		new WordDirection(-1, -1),	// EWordDirection.SouthWest
		new WordDirection(-1,  0),	// EWordDirection.West
		new WordDirection(-1,  1),	// EWordDirection.NorthWest
	};

	private int XModifier;
	private int YModifier;

	public WordDirection(int xModifier, int yModifier)
	{
		XModifier = xModifier;
		YModifier = yModifier;
	}

	static public void GetModifiers(EWordDirection wordDirection, out int xModifier, out int yModifier)
	{
		WordDirection modifiers = kMappedDirections[(int)wordDirection];
		xModifier = modifiers.XModifier;
		yModifier = modifiers.YModifier;
	}
}