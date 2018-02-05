using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notes at the bottom of this script

// A list of gameobjects does not persist through scenes. So we have to store the clue's information
// in a static list of type ClueInfo (a Struct) in order to be able to access it through scenes.
public struct ClueInfo
{
    public int id;
    public int rating;
    public string clueName;
    public string description;
    public string clueOwner1;
    public string clueOwner2;
    public bool isClue;
    public bool addedToYarn;
     public string location;

    // Constructor that passes a ClueItem object by reference
    public ClueInfo(ref ClueItem item)
    {
        id = item.ID;
        clueName = item.ItemName;
        rating = item.Rating;
        description = item.Description;
        clueOwner1 = item.ClueOwner1;
        clueOwner2 = item.ClueOwner2;
        location = item.Location;
        addedToYarn = false;
        // Set if the Item is considered a clue if the ClueItem GameObject is tagged as "Clue" in Unity
        if (item.CompareTag("Clue"))
            isClue = true;
        else
            isClue = false;
        
    }

    // Copy Constructor for ClueInfo
    public ClueInfo(ref ClueInfo clue)
    {
        id = clue.id;
        rating = clue.rating;
        clueName = clue.clueName;
        description = clue.description;
        clueOwner1 = clue.clueOwner1;
        clueOwner2 = clue.clueOwner2;
        location = clue.location;
        isClue = clue.isClue;
        addedToYarn = clue.addedToYarn;
    }

    public ClueInfo(ClueInfo clue, bool b_Yarn)
    {
        id = clue.id;
        rating = clue.rating;
        clueName = clue.clueName;
        description = clue.description;
        clueOwner1 = clue.clueOwner1;
        clueOwner2 = clue.clueOwner2;
        location = clue.location;
        isClue = clue.isClue;
        addedToYarn = b_Yarn;
    }
}


public class ClueItem : MonoBehaviour {


    // fields
    // clue details
    [SerializeField]
    private int                         id;
    [SerializeField]
	private string 						itemName;
	[SerializeField]
	private int 						rating;
	[SerializeField]
	private string 						description;
	[SerializeField]
	private int 						xmlIndex;
	[SerializeField]
	private int 						pairedItemXmlIndex;
	[SerializeField]
	public bool 						isInspectable = false;
    [SerializeField]
    public float                        cloneScale = .75f;
    [SerializeField]
    public Vector3                      cloneRot = new Vector3(0,0,0);
    [SerializeField]
	public float 						rotateSpeed = 25f;
    [SerializeField]
    public bool                         isCollected;
    [SerializeField]
    private string                      clueOwner1;
    [SerializeField]
    private string                      clueOwner2;
    
    [SerializeField]
    private string                      location;


    /*
	 * Properties to access those clue details.
	 */
    public int                          ID
    {
        get { return id; }
        set { id = value; }
    }

    public string 						ItemName
	{
		get { return (itemName); }

		set { itemName = value; }
	}

	public int 							Rating 
	{
		get { return (rating); }

		set { rating = value; }
	}

	public string 						Description
	{
		get { return (description); }

		set { description = value; }
	}

    public string Location
    {
        get { return location; }
        set { location = value; }
    }

	public int 							XMLIndex
	{
		get { return (xmlIndex); }

		set { xmlIndex = value; }
	}

	public int 							pairedItemXMLIndex
	{
		get { return (pairedItemXmlIndex); }

		set { pairedItemXmlIndex = value; }
	}

	public bool 						IsInspectable
	{
		get { return isInspectable; }
		set { isInspectable = value; }
	}

	public float                        RotateSpeed {
		get { return rotateSpeed; }
		set { rotateSpeed = value; }
	}

    public string ClueOwner1
    {
        get { return clueOwner1; }
        set { clueOwner1 = value; }
    }

    public string ClueOwner2
    {
        get { return clueOwner2; }
        set { clueOwner2 = value; }
    }

  
	public override string ToString ()
	{
		// returns a string with the clue's basic info.

		string messageFormat = "We hit: {0}\nName: {1}\nRating: {2}\nDescription: {3}";

		return string.Format(messageFormat, this.gameObject, name, rating, description);
	}
    


}


/*
 * There was no need to have the clue details (like name, desc, xml index) be public vars with 
 * public properties since the point of properties is to be able to control variable access without
 * always needing to keep them all private. 
 * 
 * Hence, I made the public vars into private backing fields, and I removed the capitalization
 * form the property names.
 * 
 */ 