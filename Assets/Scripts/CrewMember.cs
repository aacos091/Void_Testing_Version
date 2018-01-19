using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewMember {

	private int 			_id;
	private string 			_name;
	private string 			_title;
	// TODO May not need this field
	private int 			_age;
	private string 			_description;
	private bool 			_isCulprit;

	// Default constructor
	public CrewMember ()
	{
		_id = 0;
		_name = "No name";
		_title = "Basic Job";
		_age = 33;
		_description = "No given description";
	}

	// Constructor that specifies all fields
	public CrewMember (int c_id, string c_name, string c_title, int c_age, string cDesc, bool isCulp)
	{
		_id = c_id;
		_name = c_name;
		_title = c_title;
		_age = c_age;
		_description = cDesc;
		_isCulprit = isCulp;
	}


	/****************************
	 * 		Properties			*
	 ***************************/

	public int ID
	{
		get { return _id; }
		set { _id = value; }
	}

	public string Name
	{
		get { return _name; }
		set { _name	= value; }
	}

	public string Title
	{
		get { return _title; }
		set { _title = value; }
	}

	public int Age
	{
		get { return _age;  }
		set { _age = value; }
	}

	public string Description
	{
		get { return _description; }
		set { _description = value; }
	}

	public bool IsCulprit
	{
		get { return _isCulprit; }
		set { _isCulprit = value; }
	}
}
