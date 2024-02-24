using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using UnityEngine.UI;
using Firebase.Extensions;
using UnityEngine.SceneManagement;

public class MainMenuTest : MonoBehaviour
{
    // Start is called before the first frame update
    public Button loadGame;
    public Button startGame;
    public SaveManager saveManager;
    DatabaseReference reference;
    private string currValue;
    void Start()
    {
        //initialize database reference
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //add listener for ice button
        loadGame.onClick.AddListener(() => LoadGameClicked());
        startGame.onClick.AddListener(() => StartGame());
        
    

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        //loads first level
        SceneManager.LoadScene(0);
    }

    public void LoadGameClicked()
    {
        Debug.Log("load clicked");

        //calls LoadGameData() from SaveManager
        
        saveManager.LoadGameData();
        

    }
}
