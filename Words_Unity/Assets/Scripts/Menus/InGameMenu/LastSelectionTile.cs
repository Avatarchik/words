using UnityEngine;
using UnityEngine.UI;

public class LastSelectionTile : MonoBehaviour
{
	public Image ImageRef;
	public Text TextRef;

	public void SetVisibility(bool isVisible)
	{
		ImageRef.gameObject.SetActive(isVisible);
	}

	public void SetText(char character)
	{
		TextRef.text = character.ToString();
	}

	public void SetColour(Color colour)
	{
		ImageRef.color = colour;
	}
}