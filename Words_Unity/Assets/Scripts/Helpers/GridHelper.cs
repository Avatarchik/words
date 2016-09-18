static public class GridHelper
{
	static public bool IsGridPositionValid(GridPosition position, int width, int height)
	{
		bool isValid = position.X >= 0 && position.Y >= 0 && position.X < width && position.Y < height;
		return isValid;
	}
}