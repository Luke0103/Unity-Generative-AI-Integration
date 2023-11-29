using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    bool forward = false, backward = false, right = false, left = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            forward = true;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            forward = false;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            left = true;
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            left = false;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            backward = true;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            backward = false;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            right = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            right = false;
        }

        if (forward)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        if (backward)
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (right)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
        if (left)
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
    }
}
