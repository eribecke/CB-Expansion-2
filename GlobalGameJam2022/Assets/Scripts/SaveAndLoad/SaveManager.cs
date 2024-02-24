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
        //moves to each scene, starting from the Main Menu
        DontDestroyOnLoad(this.gameObject);
        
    }

    private void Start()
    {
        //initialize database reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        

    }

    public void SaveGameData()
    {
        //If no playerData object exists, create a new one
        if(playerData == null)
        {
            playerData = new PlayerData();
        }

        //grabs all scripts with ISaveable interface implemented and stores than in an IEnumerable, called the SaveData method on each item
        IEnumerable<ISaveable> saveScripts = FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>();
        foreach (ISaveable script in saveScripts)
        {
            script.SaveData(playerData);
        }

        //update the values in the database
        reference.Child("PlayerData").Child("Scene").SetValueAsync(playerData.currSceneName);
        reference.Child("PlayerData").Child("Positions").Child("Wolf1").Child("X").SetValueAsync(playerData.wolf1X);
        reference.Child("PlayerData").Child("Positions").Child("Wolf1").Child("Y").SetValueAsync(playerData.wolf1Y);
        reference.Child("PlayerData").Child("Positions").Child("Wolf1").Child("Z").SetValueAsync(playerData.wolf1Z);

        reference.Child("PlayerData").Child("Positions").Child("Wolf2").Child("X").SetValueAsync(playerData.wolf2X);
        reference.Child("PlayerData").Child("Positions").Child("Wolf2").Child("Y").SetValueAsync(playerData.wolf2Y);
        reference.Child("PlayerData").Child("Positions").Child("Wolf2").Child("Z").SetValueAsync(playerData.wolf2Z);

        //reference.Child("PlayerData").Child("Collectables").Child("GreenCollectables").SetValueAsync(playerData.gCollectables);

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

       FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Positions").Child("Wolf1").Child("X").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());

                //setting playerData values grabbed from Firebase
                playerData.wolf1X = float.Parse(snapshot.Value.ToString());
            
            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Positions").Child("Wolf1").Child("Y").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());

                //setting playerData values grabbed from Firebase
                playerData.wolf1Y = float.Parse(snapshot.Value.ToString());

            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Positions").Child("Wolf1").Child("Z").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());

                //setting playerData values grabbed from Firebase
                playerData.wolf1Z = float.Parse(snapshot.Value.ToString());

            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Positions").Child("Wolf2").Child("X").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());

                //setting playerData values grabbed from Firebase
                playerData.wolf2X = float.Parse(snapshot.Value.ToString());

            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Positions").Child("Wolf2").Child("Y").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());

                //setting playerData values grabbed from Firebase
                playerData.wolf2Y = float.Parse(snapshot.Value.ToString());

            }
        });

        FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Positions").Child("Wolf2").Child("Z").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                //Debug.Log(snapshot.Value.ToString());

                //setting playerData values grabbed from Firebase
                playerData.wolf2Z = float.Parse(snapshot.Value.ToString());

            }
        });






        FirebaseDatabase.DefaultInstance.GetReference("PlayerData").Child("Scene").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                Debug.Log("ggs");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                Debug.Log(snapshot.Value.ToString());

                //setting playerData values grabbed from Firebase
                playerData.currSceneName = snapshot.Value.ToString();
                Debug.Log(playerData.currSceneName + " scene name");

                //loads saved scene
                SceneManager.LoadScene(playerData.currSceneName);
               
               

            }
        });

       



    }
    
}
