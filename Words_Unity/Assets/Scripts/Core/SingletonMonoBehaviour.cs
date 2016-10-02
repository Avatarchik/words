using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour
	where T : MonoBehaviour
{
	static private T _Instance;
	static public T Instance
	{
		get
		{
			if (_Instance == null)
			{
				_Instance = FindObjectOfType<T>();
			}

			return _Instance;
		}
	}
}