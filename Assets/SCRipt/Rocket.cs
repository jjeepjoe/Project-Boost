using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    //CONFIG PARAMS
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 40f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip crashDeath;
    [SerializeField] AudioClip landingWin;
    [SerializeField] float sceneDelay = 3f;
    int currentScene;
    //CACHE/ HANDLES
    Rigidbody myRB;
    AudioSource myAudioSource;
    //
    enum State {  Alive, Dying, Transcending }
    State myState = State.Alive;
    
    //connect to the components
    private void Start()
    {
        myRB = GetComponent<Rigidbody>();
        myAudioSource = GetComponent<AudioSource>();
    }
    //keep CODE clean.
    private void FixedUpdate()
    {
        if(myState != State.Dying)
        {
            RespondToThrustInput();
            RepondToRotateInput();
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
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    //
    private void StartSuccessSequence()
    {
        //Landing Pad
        //myAudioSource.Stop();
        myAudioSource.PlayOneShot(landingWin);
        myState = State.Transcending;
        Invoke("LoadNextScene", sceneDelay);
    }
    //
    private void StartDeathSequence()
    {
        //player death
        myState = State.Dying;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashDeath);
        Invoke("LoadCurrentScene", sceneDelay);
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
    private void RepondToRotateInput()
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
    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            myAudioSource.Stop();
        }
    }
    //
    private void ApplyThrust()
    {
        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(mainEngine);
        }
        myRB.AddRelativeForce(Vector3.up * mainThrust);
    }
}
