using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using Firebase.Extensions;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public Button ice;
    DatabaseReference reference;
    private string currValue;
    void Start()
    {
        //initialize database reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //add listener for ice button
        ice.onClick.AddListener(() => IceClicked());
        //grab current saved value from database
        //removes the test child - > reference.Child("test").RemoveValueAsync();
        FirebaseDatabase.DefaultInstance.GetReference("test").Child("ice").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                currValue = snapshot.Value.ToString();
                Debug.Log(currValue + " initial string");

            }
        });

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IceClicked()
    {


        //update the value in the database
        reference.Child("test").Child("ice").SetValueAsync("ice" + currValue);
        //grab current saved value from database
        FirebaseDatabase.DefaultInstance.GetReference("test").Child("ice").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;


                currValue = snapshot.Value.ToString();
                Debug.Log(currValue + " updated");

            }
        });


    }
}
