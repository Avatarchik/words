using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterBackground : MonoBehaviour
	, IEventSystemHandler
	, IPointerUpHandler
	, IPointerDownHandler
	, IPointerEnterHandler
{
	public Image ImageRef;
	private Color mBaseColour;

	public void AddHighlight(Color highlightColour)
	{
		mBaseColour = ImageRef.color;
		ImageRef.color = highlightColour;
	}

	public void RemoveHighlight()
	{
		ImageRef.color = mBaseColour;
	}

	public void UpdateBaseColour(int characterUsageLeft)
	{
		float t = (1f / (PuzzleLoader.sActivePuzzleContents.MaxCharacterUsage - 1)) * (characterUsageLeft - 1);
		t = MathfHelper.Clamp01(t);
		mBaseColour = ColorHelper.Blend(ColourSchemeManager.sActiveColourScheme.High, ColourSchemeManager.sActiveColourScheme.Low, t); ;
		ImageRef.color = mBaseColour; // TODO - might be an issue here
	}

	public Color GetBaseColour()
	{
		return mBaseColour;
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			Highlighter.Instance.SetFrom(transform.gameObject);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Highlighter.Instance.GetTo() == null)
			{
				Highlighter.Instance.SetTo(Highlighter.Instance.GetFrom());
			}

			Highlighter.Instance.CheckHighlightedValidity();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			Highlighter.Instance.SetTo(transform.gameObject);
		}
	}
}