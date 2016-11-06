using UnityEngine;
using System.Collections;

using SecPlayerPrefs;

/* Thanks you for downloading this asset. We've tried to make this as similar to use as the PlayerPrefs
 * already in Unity, thus all you need to do to access it is use "PlayerPrefsPlus" instead of "PlayerPrefs";
 * you then have the ability to save and retreive any of the following data types.
 * - int		\
 * - float		|-- Just so your code isn't so messy ;)
 * - String		/
 * - bool
 * - Color
 * - Vector2
 * - Vector3
 * - Vector4
 * - Quaternion
 * 
 * We hope this is as simple to use as we'd like and, whilst we'll continually update this with more types,
 * if you have any suggestions as to what we should add or find any problems you can reach us here:
 * 		assets@ninjapokestudios.co.uk
 * 
 * Thanks again,
 * -The NinjaPoke Studios team
*/

//	Copyright NinjaPoke Studios, You can change things but please don't redistrubute in any shape or form 
//	because we lose out :(

/* As a (rather pointless) example:
 * public class Test : MonoBehaviour{
 * 		void Start(){
 * 			PlayerPrefsPlus.SetBool("TestBoolean",true);
 * 		}
 * 
 * 		void Update(){
 * 			print( PlayerPrefsPlus.GetBool("TestBoolean") );
 * 		}
 * }
*/

public class PlayerPrefsPlus : MonoBehaviour
{
	//############################################## Save ##############################################
	public static void Save()
	{
		SecurePlayerPrefs.Save();
	}

	//############################################# HasKey #############################################

	//A has key method for PlayerPrefsPlus
	public static bool HasKey(string key)
	{
		string[] types = { "{0}", "PlayerPrefsPlus:bool:{0}", "PlayerPrefsPlus:Colour:{0}-r", "PlayerPrefsPlus:Colour32:{0}-r", "PlayerPrefsPlus:Vector2:{0}-x", "PlayerPrefsPlus:Vector3:{0}-x", "PlayerPrefsPlus:Vector4:{0}-x", "PlayerPrefsPlus:Vector3:Quaternion:{0}-x", "PlayerPrefsPlus:Vector4:Rect:{0}-x" };

		foreach (string type in types)
		{
			if (SecurePlayerPrefs.HasKey(string.Format(type, key)))
			{
				return true;
			}
		}
		return false;
	}

	//############################################### int ##############################################

	//Ints stored normally just to make things nice and similar in user projects
	public static void SetInt(string key, int value)
	{
		SecurePlayerPrefs.SetInt(key, value);
	}

	public static int GetInt(string key)
	{
		return SecurePlayerPrefs.GetInt(key);
	}

	public static int GetInt(string key, int defaultValue)
	{
		return SecurePlayerPrefs.GetInt(key, defaultValue);
	}

	//############################################### float ##############################################

	//Floats also stored normally just to make things nice and similar in user projects
	public static void SetFloat(string key, float value)
	{
		SecurePlayerPrefs.SetFloat(key, value);
	}

	public static float GetFloat(string key)
	{
		return SecurePlayerPrefs.GetFloat(key);
	}

	public static float GetFloat(string key, float defaultValue)
	{
		return SecurePlayerPrefs.GetFloat(key, defaultValue);
	}

	//############################################### String ##############################################

	//And strings
	public static void SetString(string key, string value)
	{
		SecurePlayerPrefs.SetString(key, value);
	}

	public static string GetString(string key)
	{
		return SecurePlayerPrefs.GetString(key);
	}

	public static string GetString(string key, string defaultValue)
	{
		return SecurePlayerPrefs.GetString(key, defaultValue);
	}

	//############################################## bool ##############################################

	//Store bool as 0 or 1
	public static void SetBool(string key, bool value)
	{
		if (value)
			SecurePlayerPrefs.SetInt("PlayerPrefsPlus:bool:" + key, 1);
		else
			SecurePlayerPrefs.SetInt("PlayerPrefsPlus:bool:" + key, 0);
	}

	public static bool GetBool(string key)
	{
		return GetBool(key, false);
	}

	public static bool GetBool(string key, bool defaultValue)
	{
		int value = SecurePlayerPrefs.GetInt("PlayerPrefsPlus:bool:" + key, 2);
		if (value == 2)     //Return default
			return defaultValue;
		else if (value == 1)    //Return true
			return true;
		else                    //Return false
			return false;
	}

	//############################################## Color ##############################################

	//Store Color data as RGBA floats
	public static void SetColour(string key, Color value)
	{
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-r", value.r);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-g", value.g);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-b", value.b);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Colour:" + key + "-a", value.a);
	}

	//Rebuild Color data from RGBA floats
	public static Color GetColour(string key)
	{
		return GetColour(key, Color.clear);
	}

	public static Color GetColour(string key, Color defaultValue)
	{
		Color returnValue;
		returnValue.r = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-r", defaultValue.r);
		returnValue.g = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-g", defaultValue.g);
		returnValue.b = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-b", defaultValue.b);
		returnValue.a = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Colour:" + key + "-a", defaultValue.a);
		return returnValue;
	}

	//############################################ Color 32 #############################################

	//Store Color32 data RGBA Ints
	public static void SetColour32(string key, Color32 value)
	{
		SecurePlayerPrefs.SetInt("PlayerPrefsPlus:Colour32:" + key + "-r", value.r);
		SecurePlayerPrefs.SetInt("PlayerPrefsPlus:Colour32:" + key + "-g", value.g);
		SecurePlayerPrefs.SetInt("PlayerPrefsPlus:Colour32:" + key + "-b", value.b);
		SecurePlayerPrefs.SetInt("PlayerPrefsPlus:Colour32:" + key + "-a", value.a);
	}

	//Rebuild Color32 data from RGBA Ints
	public static Color32 GetColour32(string key)
	{
		return GetColour32(key, new Color32(0, 0, 0, 0));
	}

	public static Color32 GetColour32(string key, Color32 defaultValue)
	{
		Color32 returnValue;
		returnValue.r = (byte)SecurePlayerPrefs.GetInt("PlayerPrefsPlus:Colour32:" + key + "-r", defaultValue.r);
		returnValue.g = (byte)SecurePlayerPrefs.GetInt("PlayerPrefsPlus:Colour32:" + key + "-g", defaultValue.g);
		returnValue.b = (byte)SecurePlayerPrefs.GetInt("PlayerPrefsPlus:Colour32:" + key + "-b", defaultValue.b);
		returnValue.a = (byte)SecurePlayerPrefs.GetInt("PlayerPrefsPlus:Colour32:" + key + "-a", defaultValue.a);
		return returnValue;
	}
	//############################################# Vector2 #############################################

	//Store Vector2 data as as x & y floats
	public static void SetVector2(string key, Vector2 value)
	{
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector2:" + key + "-x", value.x);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector2:" + key + "-y", value.y);
	}

	//Rebuild Vector2 from floats
	public static Vector2 GetVector2(string key)
	{
		return GetVector2(key, Vector2.zero);
	}

	public static Vector2 GetVector2(string key, Vector2 defaultValue)
	{
		Vector2 returnValue;
		returnValue.x = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector2:" + key + "-x", defaultValue.x);
		returnValue.y = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector2:" + key + "-y", defaultValue.y);
		return returnValue;
	}

	//############################################# Vector3 #############################################

	//Store Vector3 data as as x, y & z floats
	public static void SetVector3(string key, Vector3 value)
	{
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector3:" + key + "-x", value.x);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector3:" + key + "-y", value.y);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector3:" + key + "-z", value.z);
	}

	//Rebuild Vector3 from floats
	public static Vector3 GetVector3(string key)
	{
		return GetVector3(key, Vector3.zero);
	}

	public static Vector3 GetVector3(string key, Vector3 defaultValue)
	{
		Vector3 returnValue;
		returnValue.x = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector3:" + key + "-x", defaultValue.x);
		returnValue.y = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector3:" + key + "-y", defaultValue.y);
		returnValue.z = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector3:" + key + "-z", defaultValue.z);
		return returnValue;
	}

	//############################################# Vector4 #############################################

	//Store Vector4 data as as x, y, z & w floats
	public static void SetVector4(string key, Vector4 value)
	{
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-x", value.x);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-y", value.y);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-z", value.z);
		SecurePlayerPrefs.SetFloat("PlayerPrefsPlus:Vector4:" + key + "-w", value.w);
	}

	//Rebuild Vector4 from floats
	public static Vector4 GetVector4(string key)
	{
		return GetVector4(key, Vector4.zero);
	}

	public static Vector4 GetVector4(string key, Vector4 defaultValue)
	{
		Vector4 returnValue;
		returnValue.x = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-x", defaultValue.x);
		returnValue.y = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-y", defaultValue.y);
		returnValue.z = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-z", defaultValue.z);
		returnValue.w = SecurePlayerPrefs.GetFloat("PlayerPrefsPlus:Vector4:" + key + "-w", defaultValue.w);
		return returnValue;
	}

	//############################################ Quaternion ############################################

	//For simplicity we are just going to put Quaternions into Vector3s with "Quaternion" before the key
	public static void SetQuaternion(string key, Quaternion value)
	{
		SetVector3("Quaternion:" + key, value.eulerAngles);
	}

	public static Quaternion GetQuaternion(string key)
	{
		return Quaternion.Euler(GetVector3("Quaternion:" + key, Quaternion.identity.eulerAngles));
	}

	public static Quaternion GetQuaternion(string key, Quaternion defaultValue)
	{
		return Quaternion.Euler(GetVector3("Quaternion:" + key, defaultValue.eulerAngles));
	}

	//############################################### Rect ###############################################

	//Similar to Quaternions we are just going to put Rects into Vector4s with "Rect" before the key
	public static void SetRect(string key, Rect value)
	{
		SetVector4("Rect:" + key, new Vector4(value.x, value.y, value.width, value.height));
	}

	public static Rect GetRect(string key)
	{
		Vector4 v4 = GetVector4("Rect:" + key, Vector4.zero);
		return new Rect(v4.x, v4.y, v4.z, v4.w);
	}

	public static Rect GetRect(string key, Rect defaultValue)
	{
		Vector4 v4 = GetVector4("Rect:" + key, new Vector4(defaultValue.x, defaultValue.y, defaultValue.width, defaultValue.height));
		return new Rect(v4.x, v4.y, v4.z, v4.w);
	}
}

public class PPP : PlayerPrefsPlus{}