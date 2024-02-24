using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour, ISaveable
{

    private Collectable[] collectables;

    private void Awake()
    {
        //moves to each scene, starting from the Main Menu
        DontDestroyOnLoad(this.gameObject);

    }
    public void SaveData(PlayerData data)
    {
        //fetch collectables from scene
       /* collectables = FindObjectsOfType<Collectable>();
        Dictionary<int, string> remainingGreen = new Dictionary<int, string>();
        
        //adding non-collected collectables to the dictionary
        foreach (Collectable gColl in collectables)
        {
            if(gColl.tag == "GCollectable")
            {
                remainingGreen.Add(gColl.key, gColl.key.ToString());
            }
            
        }

        //updating PlayerData
        data.gCollectables = remainingGreen; */
       
    }

    //implements LoadData from ISaveable interface
    public void LoadData(PlayerData data)
    {
       
    }

}
