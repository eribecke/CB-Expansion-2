using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateBlock : ProximitySkill
{
    Transform topObject;
    public Material preSkillMaterial;
    public Material canActivateMaterial;
    public Material postSkillMaterial;
    private Animator animator;
    // Start is called before the first frame update
    public override void OnStart()
    {
        world = WorldEnum.Green;
        topObject = this.transform.Find("top");
        topObject.GetComponent<BoxCollider2D>().enabled = false;
        OnCanActivateOut();
        GetComponent<Renderer>().enabled = false;

        var animationObject = this.transform.Find("animation");
        animator = animationObject.GetComponent<Animator>();

    }

    public override void OnCanActivateIn()
    {
        animator.Play("GrowBlock_CanActivate");
        topObject.GetComponent<Renderer>().material = canActivateMaterial;
    }

    public override void OnCanActivateOut()
    {
        topObject.GetComponent<Renderer>().material = preSkillMaterial;
        
    }

    public override void OnPerformHability()
    {
        topObject.GetComponent<BoxCollider2D>().enabled = true;
        topObject.GetComponent<Renderer>().material = postSkillMaterial;
        GrowSeed();
    }

    public void GrowSeed()
    {
        animator.Play("GrowBlock_Growing");
        topObject.GetComponent<Transform>().localPosition = new Vector3(
            0,
            0.41f,
            0
        );
    }
}
