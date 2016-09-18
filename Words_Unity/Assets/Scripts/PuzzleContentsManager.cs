using UnityEngine;
using System.Collections.Generic;

[ScriptOrder(101)]
public class PuzzleContentsManager : MonoBehaviour
{
	public List<PuzzleContents> Puzzles;

#if UNITY_EDITOR
	public void ClearList()
	{
		Puzzles.Clear();
	}

	public void RegisterPuzzle(PuzzleContents newContents)
	{
		Puzzles.Add(newContents);
	}
#endif // UNITY_EDITOR
}