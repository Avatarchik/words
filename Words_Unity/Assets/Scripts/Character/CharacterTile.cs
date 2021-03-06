using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterTile : MonoBehaviour
{
	public CharacterBackground BackgroundRef;
	public Text TextRef;

	public char Character { get; private set; }
	private int mUsageLeft;
	public GridPosition Position { get; private set; }

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

	public void SetUsage(int newUsage)
	{
		DecreaseUsage(mUsageLeft - newUsage);
	}

	public void DecreaseUsage(int numberOfUses)
	{
		mUsageLeft -= numberOfUses;
		UpdateName();

		int charIndex = (mLoader.GetCurrentPuzzleSize() * Position.X) + Position.Y;
		SaveGameManager.Instance.ActivePuzzleState.SetCharacterUsageLeft(charIndex, mUsageLeft);

		if (mUsageLeft <= 0)
		{
			mLoader.RemoveTile(Position);
			enabled = false;
			transform.DOScale(0, 1f).OnComplete(() => Destroy(gameObject));
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
			BackgroundRef.AddHighlight(ColourSchemeManager.sActiveColourScheme.Highlight);
		}
		else
		{
			BackgroundRef.RemoveHighlight();
		}
	}

	public Color GetBackgroundColour()
	{
		return BackgroundRef.GetBaseColour();
	}
}