using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovableBlock : ProximitySkill
{
    public Material normalMaterial;
    public Material canActivateMaterial;

    public override void OnStart()
    {
        world = WorldEnum.Orange;
    }

    public override void OnPerformHability()
    {
        Destroy(this.gameObject);
    }

    public override void OnCanActivateIn()
    {
        GetComponent<Renderer>().material = canActivateMaterial;
    }

    public override void OnCanActivateOut()
    {
        GetComponent<Renderer>().material = normalMaterial;
    }
}
