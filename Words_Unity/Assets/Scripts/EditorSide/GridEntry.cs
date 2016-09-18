using UnityEngine;

public class GridEntry
{
	public char Character;
	public int NumberOfUses;

	public GridEntry(char character)
	{
		Character = character;
		NumberOfUses = 0;
	}
}