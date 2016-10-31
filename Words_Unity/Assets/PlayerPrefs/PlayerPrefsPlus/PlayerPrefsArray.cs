using UnityEngine;
using System.Collections;

using SecPlayerPrefs;

public class PlayerPrefsArray : MonoBehaviour
{

	//############################################### int ##############################################

	//Set an array of ints
	public static void SetIntArray(string key, int[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Int:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			SecurePlayerPrefs.SetInt("PlayerPrefsArray:Int:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of ints
	public static int[] GetIntArray(string key)
	{
		int[] returns = new int[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Int:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Int:L:" + key))
		{
			returns.SetValue(SecurePlayerPrefs.GetInt("PlayerPrefsArray:Int:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### float ##############################################

	//Set an array of floats
	public static void SetFloatArray(string key, int[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Float:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			SecurePlayerPrefs.SetFloat("PlayerPrefsArray:Float:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of floats
	public static float[] GetFloatArray(string key)
	{
		float[] returns = new float[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Float:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Float:L:" + key))
		{
			returns.SetValue(SecurePlayerPrefs.GetFloat("PlayerPrefsArray:Float:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### String ##############################################

	//Set an array of strings
	public static void SetStringArray(string key, string[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:String:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			SecurePlayerPrefs.SetString("PlayerPrefsArray:String:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of strings
	public static string[] GetStringArray(string key)
	{
		string[] returns = new string[SecurePlayerPrefs.GetInt("PlayerPrefsArray:String:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:String:L:" + key))
		{
			returns.SetValue(SecurePlayerPrefs.GetString("PlayerPrefsArray:String:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### bool ##############################################

	//Set an array of bool
	public static void SetBoolArray(string key, bool[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Bool:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetBool("PlayerPrefsArray:Bool:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of bools
	public static bool[] GetBoolArray(string key)
	{
		bool[] returns = new bool[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Bool:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Bool:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetBool("PlayerPrefsArray:Bool:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### Color ##############################################

	//Set an array of Colours
	public static void SetColourArray(string key, Color[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Colour:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetColour("PlayerPrefsArray:Colour:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of Colours
	public static Color[] GetColourArray(string key)
	{
		Color[] returns = new Color[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Colour:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Colour:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetColour("PlayerPrefsArray:Colour:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### Color32 ##############################################

	//Set an array of Colour32s
	public static void SetColour32Array(string key, Color32[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Colour32:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetColour32("PlayerPrefsArray:Colour32:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of Colour32s
	public static Color32[] GetColour32Array(string key)
	{
		Color32[] returns = new Color32[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Colour32:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Colour32:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetColour32("PlayerPrefsArray:Colour32:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### Vector2 ##############################################

	//Set an array of Vector2s
	public static void SetVector2Array(string key, Vector2[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Vector2:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetVector2("PlayerPrefsArray:Vector2:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of Vector2s
	public static Vector2[] GetVector2Array(string key)
	{
		Vector2[] returns = new Vector2[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Vector2:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Vector2:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetVector2("PlayerPrefsArray:Vector2:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### Vector3 ##############################################

	//Set an array of Vector3s
	public static void SetVector3Array(string key, Vector3[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Vector3:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetVector3("PlayerPrefsArray:Vector3:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of Vector3s
	public static Vector3[] GetVector3Array(string key)
	{
		Vector3[] returns = new Vector3[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Vector3:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Vector3:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetVector3("PlayerPrefsArray:Vector3:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### Vector4 ##############################################

	//Set an array of Vector4s
	public static void SetVector4Array(string key, Vector4[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Vector4:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetVector4("PlayerPrefsArray:Vector4:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of Vector4s
	public static Vector4[] GetVector4Array(string key)
	{
		Vector4[] returns = new Vector4[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Vector4:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Vector4:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetVector4("PlayerPrefsArray:Vector4:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### Quaternion ##############################################

	//Set an array of Quaternions
	public static void SetQuaternionArray(string key, Quaternion[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Quaternion:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetQuaternion("PlayerPrefsArray:Quaternion:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of Quaternions
	public static Quaternion[] GetQuaternionArray(string key)
	{
		Quaternion[] returns = new Quaternion[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Quaternion:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Quaternion:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetQuaternion("PlayerPrefsArray:Quaternion:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}

	//############################################### Rect ##############################################

	//Set an array of Rects
	public static void SetRectArray(string key, Rect[] value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsArray:Rect:L:" + key, value.Length);
		int i = 0;
		while (i < value.Length)
		{
			PlayerPrefsPlus.SetRect("PlayerPrefsArray:Rect:" + key + i.ToString(), value[i]);
			++i;
		}
	}

	//Get an array of Rects
	public static Rect[] GetRectArray(string key)
	{
		Rect[] returns = new Rect[SecurePlayerPrefs.GetInt("PlayerPrefsArray:Rect:L:" + key)];

		int i = 0;

		while (i < SecurePlayerPrefs.GetInt("PlayerPrefsArray:Rect:L:" + key))
		{
			returns.SetValue(PlayerPrefsPlus.GetRect("PlayerPrefsArray:Rect:" + key + i.ToString()), i);
			++i;
		}
		return returns;
	}
}

public class PPA : PlayerPrefsArray{}