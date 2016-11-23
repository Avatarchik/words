using UnityEngine;
using System;

public enum EHandedPositionType
{
	Left = 0,
	Right,
	Top,
	Bottom,
}

[Serializable]
public class HandedPosition
{
	public float PosX; // TODO - this should be a Vector2
	public float PosY;

	public float Width = -1; // TODO - this should be a Vector2
	public float Height = -1;

	[Range(0, 1)]
	public float AnchoredMinX;
	[Range(0, 1)]
	public float AnchoredMinY;

	[Range(0, 1)]
	public float AnchoredMaxX;
	[Range(0, 1)]
	public float AnchoredMaxY;

	[Range(0, 1)]
	public float PivotX;
	[Range(0, 1)]
	public float PivotY;

	public Vector3 Rotation;
}

public class HandedPositionSet : MonoBehaviour
{
	public RectTransform RectTransRef;

	public HandedPosition Left;
	public HandedPosition Right;
	public HandedPosition Top;
	public HandedPosition Bottom;

	void Awake()
	{
		ODebug.AssertNull(RectTransRef);
	}

	public void SwitchTo(EHandedPositionType type)
	{
		HandedPosition pos;
		switch (type)
		{
			case EHandedPositionType.Left:
				pos = Left;
				break;

			case EHandedPositionType.Right:
				pos = Right;
				break;

			case EHandedPositionType.Top:
				pos = Top;
				break;

			default:
				pos = Bottom;
				break;
		}

		RectTransRef.anchoredPosition = new Vector2(pos.PosX, pos.PosY);
		if (pos.Width >= 0 && pos.Height >= 0) // TODO - bad!
		{
			RectTransRef.sizeDelta = new Vector2(pos.Width, pos.Height);
		}

		RectTransRef.pivot = new Vector2(pos.PivotX, pos.PivotY);

		RectTransRef.anchorMin = new Vector2(pos.AnchoredMinX, pos.AnchoredMinY);
		RectTransRef.anchorMax = new Vector2(pos.AnchoredMaxX, pos.AnchoredMaxY);

		RectTransRef.localRotation = Quaternion.Euler(pos.Rotation);
	}
}