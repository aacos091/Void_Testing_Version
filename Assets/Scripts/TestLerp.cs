using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLerp : MonoBehaviour {

	public float lerpDuration = 2f;
	public float biggerNum = 300;
	public float smallerNum = 100;

	// Use this for initialization
	void Start () {
		Test ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Test()
	{
		// tests lerping from a bigger value to a smaller one
		float testNum = biggerNum;
		float timer = 0;
		float frameRate = 1f / Time.deltaTime;
		float framesUntilCompletion = frameRate * lerpDuration;

		while (timer < framesUntilCompletion) 
		{
			testNum = Mathf.Lerp (biggerNum, smallerNum, timer / framesUntilCompletion);
			timer++;
		}
	}
}
