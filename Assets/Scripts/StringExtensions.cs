using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Text;

public static class StringExtensions 
{
	public static string RemoveAt(this string str, char toRemove, int ind)
	{
		StringBuilder result = new StringBuilder();

		for (int i = 0; i < str.Length; i++)
		{
			if (i == ind)
				continue;
			
			result.Append(str[i]);
		}

		return result.ToString();

	}

	
}
