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


/// <summary>
/// Manages the size of the scrollbar's handle, as well as its visibility.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Scrollbar))]
public class ScrollbarSizer : MonoBehaviour 
{
	CanvasGroup cGroup;
	[SerializeField] ScrollbarType type = ScrollbarType.vertical;
	[SerializeField] RectTransform viewport;
	[SerializeField] RectTransform content;
	Scrollbar scrollbar;
	public bool isVisible { get; protected set; }

	[Header("For debugging")]
	[SerializeField] float _contentHeight;
	float contentWidth
	{
		get 
		{
			float width = 0;
			foreach (RectTransform child in content)
				if (child.gameObject.activeSelf)
					width += child.rect.width;
			
			return width;
		}
	}

	float contentHeight
	{
		get 
		{
			float height = 0;
			foreach (RectTransform child in content)
				if (child.gameObject.activeSelf)
					height += child.rect.height;

			_contentHeight = height;
			return height;
		}
	}
	bool shouldHideScrollbar 
	{ 
		get 
		{ 
			if (type == ScrollbarType.vertical)
				return contentHeight <= viewport.rect.height; 
			else 
				return contentWidth <= viewport.rect.width;
		} 
	}

	

	// Methods
	void Awake()
	{
		isVisible = 		true;
		cGroup = 			GetComponent<CanvasGroup>();
		scrollbar = 		GetComponent<Scrollbar>();
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
