using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClueItemInspector : MonoBehaviour
{
    public static ClueItemInspector S;			// Singleton of this class;

    public ClueItem currentClue;
    public GameObject cloneClue;
    public ClueItem cloneClueItem;

    public GameObject clueUIWindow;
    public Text itemNameDisplay;
    public Text itemDescripDisplay;

    public GameObject clueListInspector;
    public Text clueListInspectorName;
	public Text clueListInspectorDescription;
    public Image clueImageDisplay;

    public bool inspectingItem;

    // components
    private Collider cloneClueCol;
    //public Vector3 cloneCluePos = new Vector3(2f,2f,-4); 
	public Vector3 cloneCluePos;

    //Vector3 cluePrevPos;
    //Quaternion cluePrevRot;

    Vector3 centerOfClone { get { return cloneClueCol.bounds.center; } }
    // ^ better to have this as a property, so we don't have to update a var constantly in Update.
    // Can reduce how many calls on the collider are done, too, so potential performance boost!

    [SerializeField]
    private bool touchControlled;


    // Set the LayerMask _clueLayer to the appropriate layer.

    private LayerMask _clueLayer;

    private Camera mainCam;
	private Void voidCamControl; 

    public float rayCastDistance = 20000;

    private Vector3 _lightOffset = new Vector3(0, 2, -3);
    GameObject inspectionLight = null; // cache, so we can destroy and recreate it as needed

    // methods

    void Awake()
    {
        S = this;
        // better to set up component refs in Awake, since it executes before Start
        mainCam = Camera.main;
		DontDestroyOnLoad (mainCam);

		voidCamControl = mainCam.GetComponent<Void>();

        _clueLayer = 1 << LayerMask.NameToLayer("Clue");

        
        //Attempt to find UI Text by name if not plugged in already
        if (itemNameDisplay == null)
        {
            itemNameDisplay = GameObject.Find("ItemName").GetComponent<Text>();
        }

        if (itemDescripDisplay == null)
        {
            itemDescripDisplay = GameObject.Find("ItemDescription").GetComponent<Text>();
        }
    }

    void Update()
    {

		//Alex Code
		if (clueUIWindow.activeSelf) {
			Camera.main.GetComponent<Void> ().enabled = false;
			Camera.main.GetComponent<Drag_And_Zoom> ().enabled = false;
			Camera.main.GetComponent<Mouse_Drag> ().enabled = false;
		} else {
			Camera.main.GetComponent<Void> ().enabled = true;
			Camera.main.GetComponent<Drag_And_Zoom> ().enabled = true;
			Camera.main.GetComponent<Mouse_Drag> ().enabled = true;
		}

        if (!GameController.S.gamePaused)
        {
            ClickItem();
            //Does not bother calling HandleRotation if the currentClue has already been set
            if (cloneClueItem != null)
                HandleRotation(ref cloneClueItem);
        }
    }


    /********************************************
	* HELPER FUNCTIONS							*
	*********************************************/
    void ClickItem()
    {
        //TODO Look into making this mobile compatible
		if (Input.GetMouseButtonUp(0) && voidCamControl.IsZoomed)
        {
            print("Mouse Button Released");
            Ray pos = mainCam.ScreenPointToRay(Input.mousePosition);
            print("Position: " + pos);
            RaycastHit objectHit;
            Debug.DrawRay(pos.origin, pos.direction * rayCastDistance, Color.red, 5f);
            //TODO Put clue's on a specific layer and have the Raycast search only that layer. That way our raycasting is more efficient.
            if (Physics.Raycast(pos.origin, pos.direction, out objectHit, rayCastDistance))
            {
                if (objectHit.collider.CompareTag("Clue") || objectHit.collider.CompareTag("Prop"))
                {
                    Debug.Log("You have selected the " + objectHit.transform.GetComponent<ClueItem>().ItemName);

                    // Open Clue UI Window before spawning the clone 
                    clueUIWindow.SetActive(true);            

                    // Set clueItem to the object that we just hit with the Raycast
                    // cache the ClueItem script for a performance boost.
                    SetCurrentClue(objectHit.collider.gameObject);
                    // If the object we hit is tagged as a Clue or Prop, then bring it up for inspection
                    HandleClueViewing(ref currentClue);
                }
            }
        }
    }
    //Should change back to ref if it makes sense
    public void SetCurrentClue(GameObject objectHit)
    {
        if (currentClue == null)
        {
            //Separate ClueItem script management from clone manipulation

            currentClue = objectHit.GetComponent<ClueItem>();
            print("current clue:" + currentClue.ItemName);

            itemNameDisplay.text = currentClue.ItemName;
            itemDescripDisplay.text = currentClue.Description;

			//Alex code  - Set current clue position to cam position
			Vector3 desiredViewingLocation = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, -1f);

            //Vector3 location to spawn clone
            //Vector3 desiredViewingLocation = cloneCluePos;

            cloneClue = Instantiate(objectHit, desiredViewingLocation, Quaternion.Euler(0, 0, 0));
            cloneClue.transform.localScale *= currentClue.cloneScale;
            cloneClue.transform.rotation = Quaternion.Euler(currentClue.cloneRot.x, currentClue.cloneRot.y, currentClue.cloneRot.z);

            CanvasClueObject.S.SetCloneClue(ref cloneClue);

            cloneClueCol = cloneClue.GetComponent<Collider>();

            cloneClueItem = cloneClue.GetComponent<ClueItem>();
            //cloneClueItem.isCollected = true;

            cloneClueCol.enabled = false;
            cloneClueItem.enabled = false;

            inspectingItem = true;
        }


        //Needs to be replaced by clicking out
        /*
        else if (objectHit.collider.GetComponent<ClueItem>() != currentClue)
        {
            ResetCurrentClue();
        }
        */
    }

    public void StoreCurrentClue()
    {
        if (currentClue != null)
            ClueManager.S.AddClue(ref currentClue);
    }

    //
    public void ResetCurrentClue()
    {
        //currentClue.transform.position = cluePrevPos;
        //currentClue.transform.rotation = cluePrevRot;
        currentClue.isInspectable = false;
        if (cloneClue != null)
            Destroy(cloneClue);

        currentClue = null;
        cloneClue = null;
        cloneClueItem = null;

        //when we're not inspecting anything, we won't need the light
        if (inspectionLight != null)
            Destroy(inspectionLight);

        inspectingItem = false;
    }

    GameObject CreateLight(Vector3 position)
    {
        GameObject lightObject = new GameObject("Inspection Light");
        Light lightComponent = lightObject.AddComponent<Light>();
        lightComponent.type = LightType.Spot;
        lightComponent.intensity = 15f;
        lightComponent.range = 5f;
        lightComponent.spotAngle = 50f;
        lightComponent.color = Color.white;
        lightObject.transform.position = position;
        lightObject.transform.rotation = Quaternion.Euler(35, 0, 0);

        return lightObject;
    }

    void HandleClueViewing(ref ClueItem currentClue)
    {

        // Return the ClueItem information stored in the Clue Item we just clicked on 
        // and log it to the console.
        print(currentClue.ToString()); // overrode its ToString method. 

        if (cloneClueItem != false)
        cloneClueItem.isInspectable = true;

        // Set up position to set up the light for inspecting the clueItem
        Vector3 clueLightLocation = new Vector3 (0,0,0);

        if (cloneClue != null)
        {
            clueLightLocation = cloneClue.transform.position + _lightOffset;

            // Create a light to view inspectable clueItem
            if (inspectionLight == null)
                inspectionLight = CreateLight(clueLightLocation);
        }

    }

    // helper methods
    //TODO This needs to work with mobile as well. Look into Unity's touch class to get an idea of how we can implement this with a touch screen.
    void HandleRotation(ref ClueItem currentClue)
    {

        // If the item is inspectable we handle input to rotate the object. 
        // W and S rotate it around the x-axis while A and D rotate around the y-axis
        if (currentClue.isInspectable)
        {
            // Rotate object based on the button pressed
            // TODO Objects not rotating as preferred.

            //			// The following handles code for inspecting (rotating) ClueItems on PC
            //            if (Input.GetKey(KeyCode.W))
            //            {
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.right, currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.W))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }
            //            else if (Input.GetKey(KeyCode.S))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.right, -currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.S))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }
            //            else if (Input.GetKey(KeyCode.A))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.up, currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.A))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }
            //            else if (Input.GetKey(KeyCode.D))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            //                currentClue.transform.RotateAround(centerOfItem, Vector3.up, -currentClue.rotateSpeed * Time.deltaTime);
            //            }
            //            else if (Input.GetKeyUp(KeyCode.D))
            //            {
            //                //rigidbody.constraints = RigidbodyConstraints.None;
            //            }

            //This is just to text touch drag functionality on Macs/PCs
            float x = 0;
            float y = 0;
			
            //if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
            if (!touchControlled)
            {
                //VERY GHETTO, DOES NOT FULLY WORK
                if (Input.GetMouseButton(0))
                {
                    Vector3 pos = 
                    new Vector3 (0, 0, 0);
                    //new Vector3(Screen.width/2,Screen.height/2, 0);


                    pos.x = cloneClueCol.bounds.center.x + Screen.width/2 - Input.mousePosition.x;
                    /*
                    float xPos = Screen.width / 2;
                    float xRange = Screen.width / 6;
                    float yPos = Screen.height / 2;
                    float yRange = Screen.height / 8;
                    
                    if (pos.x > xPos + xRange)
                        pos.x = xPos + xRange;
                    else if (pos.x < xPos - xRange)
                        pos.x = xPos - xRange;

                    if (pos.y > yPos + yRange)
                        pos.y = yPos + yRange;
                    else if (pos.y < yPos - yRange)
                        pos.y = yPos - yRange;
                        */
                    print(pos);

                    //print(pos.x);
                    //print(pos.y);

                    x = pos.x/100;
                    y = pos.y/100;
                }
            }

            //else
            if (touchControlled)
            {
                // The following handles code for inspecting (rotating) clues on Mobile
                Touch myTouch = Input.GetTouch(0);

                x = myTouch.deltaPosition.x;
                y = myTouch.deltaPosition.y;
            }

			if (Mathf.Abs(y) > Mathf.Abs(x))
			{
				// We are using the y variable in AngleAxis because moving our finger up and down would 
				// make the object rotate on it's x (right) axis

				cloneClue.transform.RotateAround (centerOfClone, Vector3.right, y * currentClue.rotateSpeed * Time.deltaTime);
//				rb.constraints = RigidbodyConstraints.FreezePosition;
//				rb.constraints = RigidbodyConstraints.FreezeRotationY;
//				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			}

			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
                // We are using the x variable in AngleAxis because moving our finger left and right would 
                // make the object rotate on it's y (world up) axis

                cloneClue.transform.RotateAround (centerOfClone, Vector3.up, x * currentClue.rotateSpeed * Time.deltaTime);
//				rb.constraints = RigidbodyConstraints.FreezePosition;
//				rb.constraints = RigidbodyConstraints.FreezeRotationX;
//				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			}
        }
    }
}
