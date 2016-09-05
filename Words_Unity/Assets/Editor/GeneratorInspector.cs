using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Generator))]
public class GeneratorInspector : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Generator myTarget = (Generator)target;

		if (EditorApplication.isPlaying)
		{
			if (GUILayout.Button("Generate"))
			{
				myTarget.Generate();
			}
		}
	}
}