using UnityEngine;
using System;

[Serializable]
public class SerializableGuid
{
	[SerializeField]
	public string Value;

	public SerializableGuid()
	{
		Value = Guid.NewGuid().ToString();
	}

	public static implicit operator SerializableGuid(Guid rhs)
	{
		return new SerializableGuid { Value = rhs.ToString("D") };
	}

	public static implicit operator SerializableGuid(string rhs)
	{
		return new SerializableGuid { Value = rhs };
	}

	public static implicit operator Guid(SerializableGuid rhs)
	{
		if (rhs.Value == null)
		{
			return Guid.Empty;
		}

		try
		{
			return new Guid(rhs.Value);
		}
		catch (FormatException)
		{
			return Guid.Empty;
		}
	}

	public override string ToString()
	{
		return Value;
	}
}