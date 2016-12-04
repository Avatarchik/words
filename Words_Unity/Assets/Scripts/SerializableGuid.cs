using UnityEngine;
using System;

[Serializable]
public class SerializableGuid
{
	static public SerializableGuid Empty = new SerializableGuid(Guid.Empty);

	[SerializeField]
	public string Value = string.Empty;

	public SerializableGuid()
	{
		Value = Guid.NewGuid().ToString();
	}

	public SerializableGuid(Guid guid)
	{
		Value = guid.ToString();
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

	static public bool operator ==(SerializableGuid lhs, SerializableGuid rhs)
	{
		if (!object.ReferenceEquals(lhs, null) && !object.ReferenceEquals(rhs, null))
		{
			bool areEqual = lhs.Value == rhs.Value;
			return areEqual;
		}

		return false;
	}

	static public bool operator !=(SerializableGuid lhs, SerializableGuid rhs)
	{
		return !(lhs == rhs);
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}

		SerializableGuid rhs = (SerializableGuid)obj;
		bool areEqual = Value == rhs.Value;
		return areEqual;
	}

	public override int GetHashCode()
	{
		// TODO - not sure if this is correct
		return -1;
	}

	public override string ToString()
	{
		return Value;
	}
}