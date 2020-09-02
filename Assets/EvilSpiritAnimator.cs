using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EvilSpiritAnimator : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown("c"))
        {
            animator.SetTrigger("toggleChomping");
        }
    }
}
