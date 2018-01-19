using System;
using System.Collections.Generic;


public static class TSTIContainerExtensions
{
    #region IList
    public static void AddRange<T>(this IList<T> list, IEnumerable<T> textToAdd)
	{
		foreach (T text in textToAdd)
			list.Add (text);
	}
		
	public static T Last<T>(this IList<T> list)
	{
		return list [list.Count - 1];
	}

    public static T First<T>(this IList<T> list)
    {
        return list[0];
    }

    #endregion

    #region Array

    public static T First<T>(this T[] arr)
    {
        return arr[0];
    }

    public static T Last<T>(this T[] arr)
    {
        return arr[arr.Length - 1];
    }

    #endregion
    
}


