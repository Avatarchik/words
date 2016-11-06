using UnityEngine;
using System;

[Serializable]
public class HandedPosition
{
	public enum EHandedPositionType
	{
		Left = 0,
		Right,
	}

	public float PosX;
	public float PosY;

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
}

public class HandedPositionPair : MonoBehaviour
{
	public RectTransform RectTransRef;

	public HandedPosition Left;
	public HandedPosition Right;

	public void SwitchTo(HandedPosition.EHandedPositionType type)
	{
		HandedPosition pos = (type == HandedPosition.EHandedPositionType.Left) ? Left : Right;

		RectTransRef.anchoredPosition = new Vector2(pos.PosX, pos.PosY);

		RectTransRef.pivot = new Vector2(pos.PivotX, pos.PivotY);

		RectTransRef.anchorMin = new Vector2(pos.AnchoredMinX, pos.AnchoredMinY);
		RectTransRef.anchorMax = new Vector2(pos.AnchoredMaxX, pos.AnchoredMaxY);
	}
}