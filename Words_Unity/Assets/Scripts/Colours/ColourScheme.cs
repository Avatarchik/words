using UnityEngine;
using System;

// http://uigradients.com/

[Serializable]
public class ColourScheme : ScriptableObject
{
	public string Name;
	public Color High;
	public Color Low;
	public Color Highlight;
}