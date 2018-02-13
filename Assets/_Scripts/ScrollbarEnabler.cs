using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


[System.Serializable]
public enum ScrollbarType 
{
	vertical, 
	horizontal
}


[RequireComponent(typeof(CanvasGroup))]
public class ScrollbarEnabler : MonoBehaviour 
{
	CanvasGroup cGroup;
	[SerializeField] ScrollbarType type = ScrollbarType.vertical;
	[SerializeField] RectTransform viewport;
	[SerializeField] RectTransform content;
	public bool isVisible { get; protected set; }
	bool shouldHideScrollbar 
	{ 
		get 
		{ 
			if (type == ScrollbarType.vertical)
				return content.rect.height > viewport.rect.height; 
			else 
				return content.rect.width > viewport.rect.width;
		} 
	}

	void Awake()
	{
		isVisible = true;
		cGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (shouldHideScrollbar && isVisible)
			HideScrollbar();
		else if (!shouldHideScrollbar && !isVisible)
			ShowScrollbar();
		
	}

	void HideScrollbar()
	{
		cGroup.alpha = 			0;
		cGroup.interactable = 	false;
		cGroup.blocksRaycasts = false;
		isVisible = false;
	}

	void ShowScrollbar()
	{
		cGroup.alpha = 			1;
		cGroup.interactable = 	true;
		cGroup.blocksRaycasts = true;
		isVisible = true;
	}
	
}
