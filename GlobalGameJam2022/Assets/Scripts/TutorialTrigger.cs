using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public Animator Animator;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Character>() && Animator != null)
        {
            Animator.Play("FadeIn", -1, 0.0f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Character>() && Animator != null)
        {
            Animator.Play("FadeOut", -1, 0.0f);
        }
    }
}
