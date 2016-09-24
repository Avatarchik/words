public class PotentialWordPlacement
{
	public GridPosition Position;
	public EWordDirection WordDirection;

	public PotentialWordPlacement(GridPosition position, EWordDirection wordDirection)
	{
		Position = position;
		WordDirection = wordDirection;
	}
}