using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour 
{

	public AudioSource open;

	public AudioSource close;

	public AudioSource click;

	public AudioSource click2;

	public AudioSource inspect;

	public AudioSource clue;

	public AudioSource select;

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

	public void Inspect()
	{


		inspect.Play ();

	}

	public void Clue()
	{

		clue.Play ();

	}

	public void Select()
	{

		select.Play ();

	}

}
