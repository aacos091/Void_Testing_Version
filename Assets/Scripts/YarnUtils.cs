using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Yarn.Unity;
using System.Text;

public static class YarnUtils 
{

	/// <summary>
	/// Replaces all var names in the passed text with their values as drawn from the 
	/// passed variable storage.
	/// </summary>
	/// <param name="variableStorage"></param>
	/// <param name="text"></param>
	/// <returns></returns>
	public static string ParseYarnText(VariableStorageBehaviour variableStorage, string text)
	{
		Debug.Log("Calling YarnUtils' ParseYarnText!");
		StringBuilder result = new StringBuilder();
		string textToParse = string.Copy(text);

		string temp;

		char varMarker = '$';
		char unVarMarker = '/';

		int varMarkerIndex = textToParse.IndexOf(varMarker);
		StringBuilder varName = new StringBuilder();
		string varValue;

		while (varMarkerIndex > -1)
		{
			/*Stuff checking whether or not to ignore a var name. WIP
			// Check if this marker is supposed to actually mark a var; if it isn't, there will be 
			// just one forward slash in front of it.
			bool slashBeforeVar = 	varMarkerIndex > 0 && 
									textToParse[varMarkerIndex - 1] == unVarMarker;
			bool negateSlash = 		slashBeforeVar && varMarkerIndex > 1 && 
									textToParse[varMarkerIndex - 2] == unVarMarker;

			if (slashBeforeVar && !negateSlash)
			{
				// Remove the forward slash, and find the next var marker
				textToParse = textToParse.RemoveAt('/', varMarkerIndex - 1);

				varMarkerIndex = textToParse.Substring(varMarkerIndex + 2).IndexOf(varMarker);
				continue;
			}
			*/

			// Add all the text before the var marker to the result, trimming it out of the text
			// to parse
			temp = textToParse.Substring(0, varMarkerIndex);
			textToParse = textToParse.Substring(varMarkerIndex);
			result.Append(temp);
			
			// replace all double-slashes in the temp (//) with single-slashes
			temp = temp.Replace("//", "/");

			// Get the whole variable name from the remaining text
			// (Keep in mind var names can only have letters and/or digits)
			do 
			{
				varName.Append(textToParse[0]);
				textToParse = textToParse.Remove(0, 1);
			}
			while (textToParse.Length > 0 && char.IsLetterOrDigit(textToParse[0]));

			// Get the value from the variable storage, and add it to the result
			varValue = variableStorage.GetValue(varName.ToString()).AsString;

			result.Append(varValue);
			varName = new StringBuilder();
			
			// update the var marker index
			varMarkerIndex = textToParse.IndexOf(varMarker);
		}

		if (textToParse.Length > 0)
			result.Append(textToParse);
		
		return result.ToString();
	}
	
}
