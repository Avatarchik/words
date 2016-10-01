using System;

[Serializable]
public class GridPosition
{
	public int X;
	public int Y;

	public GridPosition(int x, int y)
	{
		X = x;
		Y = y;
	}

	public GridPosition(GridPosition position)
	{
		X = position.X;
		Y = position.Y;
	}

	static public bool operator ==(GridPosition lhs, GridPosition rhs)
	{
		bool areEqual = (lhs.X == rhs.X) && (lhs.Y == rhs.Y);
		return areEqual;
	}

	static public bool operator !=(GridPosition lhs, GridPosition rhs)
	{
		return !(lhs == rhs);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}

		GridPosition rhs = (GridPosition)obj;
		bool areEqual = (X == rhs.X) && (Y == rhs.Y);
		return areEqual;
	}

	public override int GetHashCode()
	{
		// TODO - not sure if this is correct
		return X ^ Y;
	}
}