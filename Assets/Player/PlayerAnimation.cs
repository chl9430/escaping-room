using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    Rigidbody rigid;
    TPSPlayerController controller;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        controller = GetComponent<TPSPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!controller.IsInAir)
        {
            animator.SetBool("isAscending", false);
            animator.SetBool("isDescending", false);

            if (controller.IsWalking)
            {
                animator.SetBool("isWalking", true);
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
        else
        {
            if (rigid.velocity.y > 0)
            {
                animator.SetBool("isAscending", true);
                animator.SetBool("isDescending", false);
            }

            if (rigid.velocity.y < 0)
            {
                animator.SetBool("isAscending", false);
                animator.SetBool("isDescending", true);
            }
        }
    }
}
