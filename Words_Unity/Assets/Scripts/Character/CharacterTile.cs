using UnityEngine;
using UnityEngine.UI;

public class CharacterTile : MonoBehaviour
{
	public CharacterBackground BackgroundRef;
	public Text TextRef;

	public char Character { get; private set; }
	private int mUsageLeft;
	public GridPosition Position;

	private PuzzleLoader mLoader;

	public void Initialise(PuzzleLoader loader, CharacterUsage charUsage, GridPosition gridPosition)
	{
		mLoader = loader;

		Character = charUsage.Character;
		TextRef.text = Character.ToString();

		mUsageLeft = charUsage.NumberOfUses;
		UpdateBackgroundColour();

		Position = gridPosition;

		UpdateName();
	}

	private void UpdateName()
	{
#if UNITY_EDITOR
		gameObject.name = string.Format("[{0}, {1}] = {2} ({3})", Position.X, Position.Y, Character, mUsageLeft);
#endif // UNITY_EDITOR
	}

	public void DecreaseUsage(int numberOfUses)
	{
		mUsageLeft -= numberOfUses;
		UpdateName();

		if (mUsageLeft <= 0)
		{
			mLoader.RemoveTile(Position);
			Destroy(gameObject);
		}
		else
		{
			UpdateBackgroundColour();
		}
	}

	public void UpdateBackgroundColour()
	{
		BackgroundRef.UpdateBaseColour(mUsageLeft);
	}

	public void SetHighlight(bool isHighlighted)
	{
		if (isHighlighted)
		{
			BackgroundRef.AddHighlight(ColourSchemesManager.sActiveColourScheme.Highlight);
		}
		else
		{
			BackgroundRef.RemoveHighlight();
		}
	}
}