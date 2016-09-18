using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterBackground : MonoBehaviour
	, IEventSystemHandler
	, IPointerUpHandler
	, IPointerDownHandler
	, IPointerEnterHandler
	, IPointerExitHandler
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
		// TODO - this should be a lookup table in each colour scheme
		float t = (1f / (PuzzleLoader.sActivePuzzleContents.MaxCharacterUsage - 1)) * (characterUsageLeft - 1);
		t = MathfHelper.Clamp01(t);
		mBaseColour = ColorHelper.Blend(ColourSchemesManager.sActiveColourScheme.High, ColourSchemesManager.sActiveColourScheme.Low, t); ;
		ImageRef.color = mBaseColour; // TODO - might be an issue here
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			WordHighlighter.Instance.SetFrom(transform.parent.gameObject);
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (WordHighlighter.Instance.GetTo() == null)
			{
				WordHighlighter.Instance.SetTo(WordHighlighter.Instance.GetFrom());
			}

			WordHighlighter.Instance.CheckHighlightedValidity();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			WordHighlighter.Instance.SetTo(transform.parent.gameObject);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			WordHighlighter.Instance.SetTo(WordHighlighter.Instance.GetFrom());
		}
	}
}