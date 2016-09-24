public class ScoredWordPlacement : PotentialWordPlacement
{
	public int Score;

	public ScoredWordPlacement(int score, GridPosition position, EWordDirection wordDirection)
		: base(position, wordDirection)
	{
		Score = score;
	}
}