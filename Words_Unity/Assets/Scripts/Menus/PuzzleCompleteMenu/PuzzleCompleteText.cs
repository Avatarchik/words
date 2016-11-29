using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PuzzleCompleteText : MonoBehaviour
{
	public Text TextRef;
	public List<string> TextVariants;

	void OnEnable()
	{
		TextRef.text = TextVariants.RandomItem();
	}
}