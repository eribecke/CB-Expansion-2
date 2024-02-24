using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    //interface for all scripts with data to be saved to implement
    void SaveData(PlayerData data);


    void LoadData(PlayerData data);
}
