using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    //CONFIG PARAMS
    //we set to an odd value to ensure we change or notice that it has not been changed.
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    private Vector3 startingPos;
    [Range(0,1)]
    [SerializeField] float movementmentFactor; // 00 = not moved 1 = fully moved.

    private void Start()
    {
        startingPos = transform.position;
    }
    /*
     * We get the Starting Point of our game object.
     * next, we do the vector math based on our manual inputs to the inspector.
     * Period : is the time for full sin wave
     * Cycle : is the number of Sin waves able to be made in the Period.
     * Tau : is Radian math for a complete circle or Sin wave.
     * rawSinwave : is the math conversion to our Sin Wave.
     * Last, we only want half of the Sin Wave so do the math and offset 
     * Then we update the game object.
     */
    private void Update()
    {
        //save divide by zero
        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2; //the radius in radians
        float rawSinWave = Mathf.Sin(cycles * tau); // look at the sin data
        movementmentFactor = rawSinWave / 2f + 0.5f; //to offset to half a wave 0- 1

        Vector3 offset = movementVector * movementmentFactor;
        transform.position = startingPos + offset;
    }
}
