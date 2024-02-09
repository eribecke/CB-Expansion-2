using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfBarrier : MonoBehaviour
{
    public WorldEnum world;
    public Material greenMaterial;
    public Material orangeMaterial;

    void Start()
    {

        GetComponent<Renderer>().material = world == WorldEnum.Green
            ? greenMaterial
            : orangeMaterial;

        this.gameObject.layer = world == WorldEnum.Green
            ? LayerMask.NameToLayer("OrangeBlocker")
            : LayerMask.NameToLayer("GreenBlocker");
    }
}
