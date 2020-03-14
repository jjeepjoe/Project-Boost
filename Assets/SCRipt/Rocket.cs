using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //CONFIG PARAMS

    //CACHE/ HANDLEs
    Rigidbody myRB;

    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }
    //keep it clean
    private void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            print("Thrusting pressed");
            myRB.AddRelativeForce(Vector3.up);
        }
        //This will only allow a single press to be processed. The top is the boss if both pressed.
        if (Input.GetKey(KeyCode.A))
        {
            print("rotate left");
        }
        else if (Input.GetKey(KeyCode.D))
        {
            print("rotate right");
        }
    }
}
