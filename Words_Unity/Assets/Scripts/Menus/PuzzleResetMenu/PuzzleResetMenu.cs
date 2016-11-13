public class PuzzleResetMenu : Menu, IMenu
{
	public ResetPuzzleOverallButton ResetButtonRef;

	public void OnEnable()
	{
	}

	public void OnDisable()
	{
	}

	public void InitialiseForPuzzle(SerializableGuid puzzleGuid, PuzzleLoadButton puzzleLoadButton)
	{
		ResetButtonRef.InitialiseForPuzzle(puzzleGuid, puzzleLoadButton);
	}
}