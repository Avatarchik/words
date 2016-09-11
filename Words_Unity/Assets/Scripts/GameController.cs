using UnityEngine;

public class GameController : MonoBehaviour
{
	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}