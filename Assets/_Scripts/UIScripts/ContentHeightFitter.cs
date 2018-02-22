using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Makes the rect transform of the game object expand when its active children
/// collectively get past a certain minimum height.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ContentHeightFitter : MonoBehaviour 
{
	public float minHeight = 1500;
	RectTransform rectTransform;
	float currentHeight 
	{
		get { return rectTransform.rect.height; }
	}

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	// Use this for initialization
	void Start () 
	{
		Vector2 newSize = rectTransform.sizeDelta;
		if (newSize.y < minHeight)
		{
			newSize.y = minHeight;
			rectTransform.sizeDelta = newSize;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		UpdateHeight();
	}

	void UpdateHeight()
	{
		float prevHeight = rectTransform.rect.height;
		float height = 0;
		Vector2 newSize = rectTransform.sizeDelta;

		foreach (RectTransform child in rectTransform)
			if (child.gameObject.activeSelf)
				height += child.rect.height;
		
		if (height != currentHeight && height >= minHeight)
		{
			Debug.Log("Updated height!");
			newSize.y = Mathf.Clamp(height, minHeight, 9999f);
			rectTransform.sizeDelta = newSize;

			// Move this up or down to keep it in place
			float heightDiff = Mathf.Abs(height - prevHeight);

			if (height > prevHeight)
				rectTransform.localPosition += new Vector3(0, heightDiff, 0);
			
			else 
				rectTransform.localPosition -= new Vector3(0, heightDiff, 0);
		}

		
	}
	
}
