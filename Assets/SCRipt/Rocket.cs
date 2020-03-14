using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //CONFIG PARAMS
    
    //CACHE/ HANDLES
    Rigidbody myRB;
    AudioSource myAudioSource;
    
    //connect to the components
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
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
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.Play(0);
            }
            myRB.AddRelativeForce(Vector3.up);
        }
        else
        {
            myAudioSource.Stop();
        }
        //This will only allow a single press to be processed. The top is the boss if both pressed.
        if (Input.GetKey(KeyCode.A))
        {
            //we are moving about the Z axis
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //we are moving about the Z axis
            transform.Rotate(-Vector3.forward);
        }

    }
}
