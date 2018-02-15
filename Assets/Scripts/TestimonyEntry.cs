using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class TestimonyEntry : MonoBehaviour 
{
	[SerializeField] string _text;
	[SerializeField] Sprite _giverMugshot;

	[SerializeField] Text _textField;
	[SerializeField] Image giverImage;

	public Text textField 
	{ 
		get { return _textField; }
		protected set { _textField = value; }
	}

	public RectTransform rectTransform;

	public string text 
	{ 
		get { return _text; }
		set 
		{ 
			_text = value; 
			textField.text = value;
		}
	}

	public Sprite giverMugshot 
	{
		get { return _giverMugshot; }
		set 
		{ 
			_giverMugshot = value; 
			giverImage.sprite = value;
		}
	}

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}
	public void Init(string text, Sprite giverMug)
	{
		
		this.text = text;
		this.giverMugshot = giverMug;

	}
	
}
