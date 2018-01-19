using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueDatabase : MonoBehaviour
{
    // contains the prefabs for all the clues in the whole game
    public static ClueDatabase S;
    public GameObject[] cluePrefabs;

    private void Awake()
    {
        S = this;
    }

    public ClueInfo GetClueInfo(string clueName)
    {
        ClueItem clueItem;

        foreach (GameObject clue in cluePrefabs)
        {
            clueItem = clue.GetComponent<ClueItem>();

            if (clueItem == null)
            {
                string messageFormat = "Clue prefab named {0} does not have a ClueItem component!";
                Debug.LogError(string.Format(messageFormat, clue.name));
            }
            else
            {
                return new ClueInfo(ref clueItem);
            }
        }

        throw new System.ArgumentException("No clue with the name " + clueName + " is in the clue database.");
    }
    
    public GameObject GetCluePrefab(string clueName)
    {
        foreach (GameObject cluePrefab in cluePrefabs)
        {
            if (cluePrefab.name == clueName)
                return cluePrefab;
        }

        Debug.LogError("Clue Prefab with name " + clueName + " was not found.");
        return null;
    }

}
