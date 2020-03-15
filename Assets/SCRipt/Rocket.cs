using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 40f;
    //CACHE/ HANDLES
    Rigidbody myRB;
    AudioSource myAudioSource;
    
    //connect to the components
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }
    //keep CODE clean.
    private void Update()
    {
        Thrust();
        Rotate();
    }
    //
    private void OnCollisionEnter(Collision otherCollider)
    {
        switch (otherCollider.gameObject.tag)
        {
            case "Friendly":
                print("nice guy");
                break;
            case "Fuel":
                print("gas baby");
                break;
            default:
                //player death
                print("last resort");
                break;
        }
    }
    //refactored our the parts.
    private void Rotate()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        //Locks from the rotation in Unity
        myRB.freezeRotation = true;
        //This will only allow a single press to be processed. The top is the boss if both pressed.
        if (Input.GetKey(KeyCode.A))
        {
            //we are moving about the Z axis anti-clockwise
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //we are moving about the Z axis
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        //un-Locks from the rotation in Unity
        myRB.freezeRotation = false;
    }
    //Vertical movement, and Audio handling.
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (!myAudioSource.isPlaying)
            {
                myAudioSource.Play(0);
            }
            myRB.AddRelativeForce(Vector3.up * mainThrust);
        }
        else
        {
            myAudioSource.Stop();
        }
    }
}
