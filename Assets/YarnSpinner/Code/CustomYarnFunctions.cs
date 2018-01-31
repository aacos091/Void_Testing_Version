using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn;
using Yarn.Unity;

public static class CustomYarnFunctions
{

	public static FunctionInfo either()
	{
		return new FunctionInfo("either", -1, delegate(Value[] parameters)
		{
			return parameters[Random.Range(0, parameters.Length)];
		});
	}

	public static FunctionInfo random()
	{
		return new FunctionInfo("random", 2, delegate(Value[] parameters)
		{
			return (parameters[Random.Range(0, 2)]).AsNumber;
		});
	}

	

	public static FunctionInfo array()
	{
		return new FunctionInfo("array", -1, delegate(Value[] parameters)
		{
			return parameters;
		});
	}
	
	
}
