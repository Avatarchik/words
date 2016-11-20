using UnityEngine;

public class GameController : MonoBehaviour
{
	void Awake()
	{
		Random.InitState(System.DateTime.Now.Millisecond);

#if !UNITY_EDITOR
		ODebug.IsEnabled = false;
#endif // !UNITY_EDITOR

		Application.targetFrameRate = 60;
	}

	void Update()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
}