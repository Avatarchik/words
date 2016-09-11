using UnityEngine;
using UnityEngine.EventSystems;

public class CharacterBackground : MonoBehaviour
	, IEventSystemHandler
	, IPointerUpHandler
	, IPointerDownHandler
	, IPointerEnterHandler
	, IPointerExitHandler
{
	static public GameObject From;
	static public GameObject To;

	public void OnPointerDown(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			From = transform.parent.gameObject;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (To == null)
			{
				To = From;
			}

			Debug.Log(string.Format("From: {0} To: {1}", From.name, To.name));
			
			From = null;
			To = null;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			To = transform.parent.gameObject;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			To = From;
		}
	}

	public void OnDrawGizmosSelected()
	{
		if (From && To)
		{
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(From.transform.position, To.transform.position);
		}
	}
}