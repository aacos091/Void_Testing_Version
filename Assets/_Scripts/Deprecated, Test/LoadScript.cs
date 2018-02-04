using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadScript : MonoBehaviour {
	public Image 			testImage;

	// Use this for initialization
	void Start () {
		testImage.sprite = Resources.Load ("Sprites\\Sink", typeof(Sprite)) as Sprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
