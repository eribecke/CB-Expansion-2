using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    public WorldEnum world;
    public Material greenMaterial;
    public Material orangeMaterial;
    //added key
    public int key;

    void Start()
    {
        var collider = gameObject.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;

        GetComponent<Renderer>().material = world == WorldEnum.Green
            ? greenMaterial
            : orangeMaterial;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var character = other.gameObject.GetComponent<Character>();
        if (character)
        {
            if (character.world == this.world)
            {
                SamWorldePick(character);
            }
        }
    }


    void SamWorldePick(Character character)
    {
        character.getCollectable();
        Destroy(this.gameObject);
    }
}
