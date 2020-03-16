using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 40f;
    int currentScene;
    //CACHE/ HANDLES
    Rigidbody myRB;
    AudioSource myAudioSource;
    enum State {  Alive, Dying, Transcending }
    State myState = State.Alive;
    
    //connect to the components
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }
    //keep CODE clean.
    private void Update()
    {
        if(myState != State.Dying)
        {
            Thrust();
            Rotate();
        }
    }
    //
    private void OnCollisionEnter(Collision otherCollider)
    {
        if(myState != State.Alive) { return; } //ignore collision while dead.
        //
        currentScene = SceneManager.GetActiveScene().buildIndex;
        switch (otherCollider.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                //Landing Pad
                myState = State.Transcending;
                Invoke("LoadNextScene", 1f); //parameterise time
                break;
            default:
                //player death
                myState = State.Dying;
                Invoke("LoadCurrentScene", 1f); //parameterise time
                break;
        }
    }
    //If Dead restart the same level.
    private void LoadCurrentScene()
    {
        SceneManager.LoadScene(currentScene);
    }
    //TODO: work for more levels
    private void LoadNextScene()
    {
        currentScene += 1;
        if (currentScene > 1)
        {
            currentScene = 0;
        }
        SceneManager.LoadScene(currentScene);
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
