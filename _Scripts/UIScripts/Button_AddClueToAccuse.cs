using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button_AddClueToAccuse : MonoBehaviour
{

    // Grab the information from the related clue attached to the ClueEntry's select button
    public void AddAccusationClue()
    {
        ClueInfo clueToAdd;

        Debug.LogWarning("BUTTON WAS PRESSED");
        // OLD: This is getting the name of the image related to the button that was pressed
        //string name = this.transform.parent.GetChild(0).GetComponent<Image>().sprite.name;

        //Checks if currentClue of ClueManager is set
        if (ClueManager.currentClue.clueName != null)
        {

            clueToAdd = ClueManager.currentClue;
            //Check if clueToAdd has already been added. Might want to replace this with a new field of ClueItem.
            foreach (ClueInfo collectedInfo in ClueManager._cluesToPresent)
            {
                if (collectedInfo.clueName == ClueManager.currentClue.clueName)
                {
                    UI_ButtonManager.S.addedClueAccuseNotif("This Clue has already been selected.");
                    return;
                }
            }

            // Continue to let the player choose clues to add to the _cluesToPresent List found in the ClueManager Singleton
            // as long as the cluesLeftToChoose variable in greater than 0

            if (AccusationManager.S.cluesLeftToChoose > 0)
            {
                //TODO: Comment this if statement
                print("Name: " + name);
                // Grabbing the correct index of the matching ClueInfo based on the name of the Clue ImageName in the ClueEntry menu of Jury_Scene
                //int clueIndex = ClueManager._dictClueNameToIndex[name];
                //We are adding the matching clue information based on which clue was selected in the Menus of clues built in Jury_Scene
                //ClueManager._cluesToPresent.Add(ClueManager._clueInfoOfCollected[clueIndex]);
                //print("ClueIndex is: " + clueIndex);

                print("Current Clue in ClueManager " + clueToAdd.clueName);

                ClueManager._cluesToPresent.Add(clueToAdd);
                UI_ButtonManager.S.AddButtonToAccusation(clueToAdd);

                // Once we grab the clue and add it to the _cluesToPresent List remove it from the available clues to choose from
                /* TODO: The button that we are clicking on is the child of the image that holds that clues description. So
             we are grabbing the parent element and setting its SetActive property to false so that it disables the entire object including the children.
            */
                //this.gameObject.transform.parent.gameObject.SetActive(false);


                // Decrease the amount of clues that the player can select to add to the list of clues for the Accusation of the crew member
                AccusationManager.S.cluesLeftToChoose--;

            }

            // If cluesLeftToChoose if 0 or less then make the FinalizeAccusationButton Appear
            if (AccusationManager.S.cluesLeftToChoose <= 0)
            {
                // Set the visibility of the finalizeAccusationButton to true so that we can go ahead with the accusation
                AccusationManager.S.finalizeAccusationButton.gameObject.SetActive(true);
            }
        }
        else
            UI_ButtonManager.S.addedClueAccuseNotif("Please Select a Clue to Add.");
    }
    
    //Deprecated
    /*
    public void CallBuildCollectedCluesMenu()
    {
        // Call the BuildCollectedCluesMenu function from ClueManager that is in charge of populating
        // a menu with all of the clues that have been found and collected so far
        //ClueManager.S.BuildCollectedCluesMenu ();

        // Set the visibility of the Text Objects that tell the player how many clues they have left to choose to active;
        AccusationManager.S.cluesLeftHeading.gameObject.SetActive(true);
        AccusationManager.S.cluesLeftText.gameObject.SetActive(true);

        // Disable the current textbox so that the clue selection menu is not obstructed
        GameObject TextBoxGO = this.transform.parent.transform.parent.gameObject;
        TextBoxGO.SetActive(false);

        // Display the CluesLeftToAccuse Text objects

    }
    */
}
