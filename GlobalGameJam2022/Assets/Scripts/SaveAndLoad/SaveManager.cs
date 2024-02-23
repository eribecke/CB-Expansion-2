using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerData PlayerData;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
    
}
