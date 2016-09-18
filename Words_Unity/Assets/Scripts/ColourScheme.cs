using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR
using System;

[Serializable]
public class ColourScheme : ScriptableObject
{
	public string Name;
	public Color High;
	public Color Low;
	public Color Highlight;
}