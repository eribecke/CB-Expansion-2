using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProximitySkill : MonoBehaviour
{
    public bool canCativate = false;

    protected WorldEnum world;
    private bool skillPerformed = false;

    private float distanceOffset = 1.5f;
    private Character character;
    private bool canActivateIn = false;

    void Start()
    {
        OnStart();
        character = new List<Character>(
            GameObject.FindObjectsOfType<Character>()
        ).Find(character => character.world == world);
    }


    private void Update()
    {
        if (!skillPerformed)
        {
            CheckProximity();
        }
    }

    private void CheckProximity()
    {
        if (character && !skillPerformed)
        {
            var size = GetComponent<Collider2D>().bounds.size;
            Vector3 source = transform.position;
            Vector3 target = character.transform.position;
            var targetTop = new Vector3(target.x, (target.y + size.y / 2), target.z);
            var targetBottom = new Vector3(target.x, (target.y - size.y / 2), target.z);

            if (character.world == WorldEnum.Green)
            {
                canCativate = Vector3.Distance(source, targetBottom) < distanceOffset;
            } else {
                canCativate =  Vector3.Distance(source, targetTop) < distanceOffset
                    || Vector3.Distance(source, targetBottom) < distanceOffset;
            }
        }

        if (canCativate && !canActivateIn)
        {
            canActivateIn = true;
            OnCanActivateIn();

        }
        if (!canCativate && canActivateIn)
        {
            canActivateIn = false;
            OnCanActivateOut();
        }

        if (Input.GetKeyDown("space") && canCativate)
        {
            skillPerformed = true;
            PerformSkill();

            if (character)
            {
                character.TryHowl();
            }
        }
    }

    private void PerformSkill()
    {
        OnPerformHability();
    }

    public abstract void OnPerformHability();
    public virtual void OnStart() { }
    public virtual void OnCanActivateIn() { }
    public virtual void OnCanActivateOut() { }
}
