 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float speed = 10f;
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>(); // Initialize anim with the Animator component of this GameObject
    }

    void FixedUpdate()
    {
        // Check for continuous key presses using GetKey instead of GetKeyDown
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S))
        {
            anim.SetBool("Walk", true);
            // Make sure "Back" is false when moving forward or sideways
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
}
