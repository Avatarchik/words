using System;

public class ScriptOrder : Attribute
{
	public int Order;

	public ScriptOrder(int order)
	{
		Order = order;
	}
}