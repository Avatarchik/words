static public class GridHelper
{
	static public bool IsGridPositionValid(int x, int y, int size)
	{
		bool isValid = x >= 0 && y >= 0 && x < size && y < size;
		return isValid;
	}
}