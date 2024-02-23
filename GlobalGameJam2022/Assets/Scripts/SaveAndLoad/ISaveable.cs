using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaveable
{
    void SaveData(PlayerData data);


    void LoadData(PlayerData data);
}
