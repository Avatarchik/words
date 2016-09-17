using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class GridEntry
{
	public GridPosition Position;

	private int _CharacterCount;
	public int CharacterCount
	{
		get
		{
			return _CharacterCount;
		}
		set
		{
			_CharacterCount = value;

			if (_CharacterCount > 0)
			{
				if (!Generator.Instance.IsRunning)
				{
					if (BackgroundComp)
					{
						SetBackgroundColour(Generator.Instance.Scheme.High, Generator.Instance.Scheme.Low, Generator.Instance.MaxCharacterUsage);
					}

					if (PrefabInstance)
					{
						PrefabInstance.name = string.Format("[{0}, {1}] = {2} ({3})", Position.X, Position.Y, Character, _CharacterCount);
					}
				}
			}
			else
			{
				GameObject.Destroy(_PrefabInstance);
				Generator.Instance.RemoveEntry(this);
			}
		}
	}

	private char _Character;
	public char Character
	{
		get
		{
			return _Character;
		}
		set
		{
			CharacterCount = (_Character == value) ? (CharacterCount + 1) : 1;

			_Character = value;
			if (PrefabInstance)
			{
				PrefabInstance.name = string.Format("[{0}, {1}] = {2} ({3})", Position.X, Position.Y, _Character, CharacterCount);

				if (TextComp)
				{
					TextComp.text = _Character.ToString();
				}
			}
		}
	}

	private GameObject _PrefabInstance;
	public GameObject PrefabInstance
	{
		get
		{
			return _PrefabInstance;
		}
		set
		{
			_PrefabInstance = value;
			TextComp = _PrefabInstance ? _PrefabInstance.GetComponentInChildren<Text>() : null;
			ImageComp = _PrefabInstance ? _PrefabInstance.GetComponentInChildren<Image>() : null;
			PositionReferenceComp = _PrefabInstance ? _PrefabInstance.GetComponent<GridPositionReference>() : null;
			BackgroundComp = _PrefabInstance ? _PrefabInstance.GetComponentInChildren<CharacterBackground>() : null;
		}
	}

	public void SetPosition(GridPosition position)
	{
		if (PositionReferenceComp)
		{
			PositionReferenceComp.Position = position;
		}
	}

	public void SetBackgroundColour(Color fromColour, Color toColour, int maxCharacterCount)
	{
		if (ImageComp)
		{
			float t = (1f / (maxCharacterCount - 1)) * (CharacterCount - 1);
			t = MathfHelper.Clamp01(t);

			Color newColour = ColorHelper.Blend(fromColour, toColour, t);
			ImageComp.color = newColour;

			if (BackgroundComp)
			{
				BackgroundComp.UpdateBaseColour(newColour);
			}
		}
	}

	public void AddTint(Color highlightColour)
	{
		if (BackgroundComp)
		{
			BackgroundComp.AddTint(highlightColour);
		}
	}

	public void RemoveTint()
	{
		if (BackgroundComp)
		{
			BackgroundComp.RemoveTint();
		}
	}

	private Text TextComp;
	private Image ImageComp;
	private GridPositionReference PositionReferenceComp;
	private CharacterBackground BackgroundComp;
}