using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using System.Linq.Expressions;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerData playerData;
    DatabaseReference reference;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
    }

    private void Start()
    {
        //initialize database reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void SaveGameData()
    {
        if(playerData == null)
        {
            playerData = new PlayerData();
        }

        IEnumerable<ISaveable> saveScripts = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        foreach (ISaveable script in saveScripts)
        {
            script.SaveData(playerData);
        }

        //update the value in the database
        reference.Child("PlayerData").Child("Scene").SetValueAsync(playerData.currSceneName);
        //grab current saved value from database
        /*FirebaseDatabase.DefaultInstance.GetReference("test").Child("ice").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;


                currValue = snapshot.Value.ToString();
                Debug.Log(currValue + " updated");

            }
        });*/
    }

    public void LoadGameData()
    {
        //grab current saved value from database
        //removes the test child - > reference.Child("test").RemoveValueAsync();
        Debug.Log("load called");
        if (playerData == null)
        {
            playerData = new PlayerData();
        }
        FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Scene").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Value.ToString());
                playerData.currSceneName = snapshot.Value.ToString();
                Debug.Log(playerData.currSceneName + " scene name");
                SceneManager.LoadScene(playerData.currSceneName);
               
               

            }
        });

        IEnumerable<ISaveable> saveScripts = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        Debug.Log(saveScripts.ToArray().Length);
        foreach (ISaveable script in saveScripts)
        {
            script.LoadData(playerData);
            
        }
        
       // 
    }
    
}
