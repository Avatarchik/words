using System.Collections.Generic;

static public class ListExtensions
{
	static public bool IsEmpty<T>(this List<T> list)
	{
		return list == null || list.Count == 0;
	}

	static public bool IsNotEmpty<T>(this List<T> list)
	{
		return list != null && list.Count > 0;
	}

	static public T RemoveFirst<T>(this List<T> list)
	{
		T item = default(T);

		if (list != null)
		{
			int count = list.Count;
			if (count > 0)
			{
				item = list[0];
				list.RemoveAt(0);
			}
		}

		return item;
	}

	static public T RemoveLast<T>(this List<T> list)
	{
		T item = default(T);

		if (list != null)
		{
			int count = list.Count;
			if (count > 0)
			{
				item = list[count - 1];
				list.RemoveAt(count - 1);
			}
		}

		return item;
	}

	static public void Shuffle<T>(this IList<T> list)
	{
		int n = list.Count;
		while (n > 1)
		{
			--n;
			int k = UnityEngine.Random.Range(0, n + 1);
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}

	static public T FirstItem<T>(this List<T> list)
	{
		T item = default(T);

		if (list != null && list.Count > 0)
		{
			item = list[0];
		}

		return item;
	}

	static public T LastItem<T>(this List<T> list)
	{
		T item = default(T);

		if (list != null)
		{
			int count = list.Count;
			if (count > 0)
			{
				item = list[count - 1];
			}
		}

		return item;
	}

	static public T RandomItem<T>(this List<T> list)
	{
		T item = default(T);

		if (list != null)
		{
			int count = list.Count;
			if (count > 0)
			{
				item = list[UnityEngine.Random.Range(0, count)];
			}
		}

		return item;
	}
}