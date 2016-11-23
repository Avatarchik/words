public class PuzzleResetMenu : Menu, IMenu
{
	public ResetPuzzleOverallButton ResetButtonRef;

	public override void OnEnable()
	{
		base.OnEnable();
	}

	public override void OnDisable()
	{
		base.OnDisable();
	}

	public void InitialiseForPuzzle(SerializableGuid puzzleGuid, PuzzleLoadButton puzzleLoadButton)
	{
		ResetButtonRef.InitialiseForPuzzle(puzzleGuid, puzzleLoadButton);
	}
}