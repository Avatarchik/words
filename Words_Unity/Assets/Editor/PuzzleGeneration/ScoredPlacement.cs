public class ScoredPlacement
{
	public int Score;
	public GridPosition Position;
	public EWordDirection WordDirection;

	public ScoredPlacement(int score, GridPosition position, EWordDirection wordDirection)
	{
		Score = score;
		Position = position;
		WordDirection = wordDirection;
	}
}