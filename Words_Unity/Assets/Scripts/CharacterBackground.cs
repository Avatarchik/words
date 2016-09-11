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
	public Image ImageComp;
	private Color mBaseColour;

	public void AddTint(Color highlightColour)
	{
		mBaseColour = ImageComp.color;
		ImageComp.color = highlightColour;
	}

	public void RemoveTint()
	{
		ImageComp.color = mBaseColour;
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

	public void OnDrawGizmosSelected()
	{
		GameObject From = WordHighlighter.Instance.GetFrom();
		GameObject To = WordHighlighter.Instance.GetTo();

		if (From && To)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(From.transform.position, To.transform.position);
		}
	}
}