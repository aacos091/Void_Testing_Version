using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class DisplayTogglableUI : MonoBehaviour 
{
	/*
	For UI elements that need to be hideable without the issues that'd
	come with just disabling them.
	 */

	protected CanvasGroup canvasGroup;
	public virtual bool isVisible 
	{
		get { return canvasGroup.alpha > 0; }
	}

	protected virtual void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public virtual void Show()
	{
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
	}

	public virtual void Hide()
	{
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
	}

}
