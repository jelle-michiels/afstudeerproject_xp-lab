using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Animator: " + animator);
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool("isWalking");
        bool pressed = Input.GetKey("z") || Input.GetKey("q") || Input.GetKey("s") || Input.GetKey("d");

        if (!isWalking && pressed)
        {
            animator.SetBool("isWalking", true);
        }

        if (!pressed)
        {
            animator.SetBool("isWalking", false);
        }
    }
}
