static public class GridHelper
{
	static public bool IsGridPositionValid(int x, int y, int width, int height)
	{
		bool isValid = x >= 0 && y >= 0 && x < width && y < height;
		return isValid;
	}
}