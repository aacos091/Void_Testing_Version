using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TeaspoonTools.TextboxSystem;
using TeaspoonTools.TextboxSystem.Utils;

[RequireComponent(typeof(Image))]
public class TSTTextboxNametag : TextboxNametag {}


public class TextboxNametag : MonoBehaviour 
{

	public RectTransform rectTransform;
	TextboxController textboxController;
	Image image;
	Text textField;

	public Sprite sprite
	{
		get { return image.sprite; }
		set { image.sprite = value; }
	}

	public string text
	{
		get { return textField.text; }
		set { textField.text = value; }
	}

    public Font font
    {
        get { return textField.font; }
        set { textField.font = value; }
    }

	public TextSettings textSettings { get { return textboxController.textSettings; } }

	void Awake()
	{
		rectTransform = 		GetComponent<RectTransform> ();
		image = 				GetComponent<Image> ();
		textField = 			GetComponentInChildren<Text> ();
	}

	public void Initialize(TextboxController tbController)
	{
		textboxController = tbController;
	}

	public void ApplyTextSettings()
	{
		font = textSettings.font;

	}


}
