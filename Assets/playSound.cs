using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour 
{

	public AudioSource open;

	public AudioSource close;

	public AudioSource click;

	public AudioSource click2;

	public void Open()
	{

		open.Play ();

	}

	public void Close()
	{

		close.Play ();

	}

	public void Click()
	{

		click.Play ();

	}

	public void Click_Two()
	{

		click2.Play ();

	}

}
