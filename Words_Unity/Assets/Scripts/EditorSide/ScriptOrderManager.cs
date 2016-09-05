#if UNITY_EDITOR
using UnityEditor;
using System;

[InitializeOnLoad]
public class ScriptOrderManager
{
	static ScriptOrderManager()
	{
		foreach (MonoScript monoScript in MonoImporter.GetAllRuntimeMonoScripts())
		{
			if (monoScript.GetClass() != null)
			{
				foreach (var a in Attribute.GetCustomAttributes(monoScript.GetClass(), typeof(ScriptOrder)))
				{
					var currentOrder = MonoImporter.GetExecutionOrder(monoScript);
					var newOrder = ((ScriptOrder)a).Order;
					if (currentOrder != newOrder)
					{
						MonoImporter.SetExecutionOrder(monoScript, newOrder);
					}
				}
			}
		}
	}
}
#endif // UNITY_EDITOR