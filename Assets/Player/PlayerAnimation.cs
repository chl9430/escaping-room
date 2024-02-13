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

            if (controller.CurrentInput.x < 0f)
            {
                animator.SetBool("isLeft", true);
            }

            if (controller.CurrentInput.x > 0f)
            {
                animator.SetBool("isRight", true);
            }

            if (controller.CurrentInput.y < 0f)
            {
                animator.SetFloat("ZValue", -1.0f);
                animator.SetBool("isVertical", true);
            }

            if (controller.CurrentInput.y > 0f)
            {
                animator.SetFloat("ZValue", 1.0f);
                animator.SetBool("isVertical", true);
            }

            if (controller.CurrentInput.x == 0f)
            {
                animator.SetBool("isLeft", false);
                animator.SetBool("isRight", false);
            }

            if (controller.CurrentInput.y == 0f)
            {
                animator.SetFloat("ZValue", 0f);
                animator.SetBool("isVertical", false);
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

        if (controller.IsShooting)
        {
            animator.SetBool("isShooting", true);
        }
        else
        {
            animator.SetBool("isShooting", false);
        }
    }
}
