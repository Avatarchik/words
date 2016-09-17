using System;

[Serializable]
public struct WordPlacement
{
	public GridPosition FromPosition;
	public GridPosition ToPosition;

	public WordPlacement(GridPosition fromPosition, GridPosition toPosition)
	{
		FromPosition = fromPosition;
		ToPosition = toPosition;
	}
}