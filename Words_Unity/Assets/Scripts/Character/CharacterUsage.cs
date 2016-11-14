using System;

[Serializable]
public class CharacterUsage
{
	public char Character;
	public int NumberOfUses;

	public CharacterUsage(char character)
	{
		Character = character;
		NumberOfUses = 0;
	}

	public void IncrementUse()
	{
		++NumberOfUses;
	}
}