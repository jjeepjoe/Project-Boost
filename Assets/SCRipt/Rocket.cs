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
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem crashDeathParticles;
    [SerializeField] ParticleSystem landingWinParticles;
    [SerializeField] float sceneDelay = 3f;
    bool areCollisionsDISABLED = false;
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
    private void Update()
    {
        if (myState != State.Dying)
        {
            RespondToThrustInput();
            RepondToRotateInput();
        }
        //todo: only if debug on
        if (Debug.isDebugBuild)
        {
            RespondToDebugKey();
        }
    }
    //
    private void RespondToDebugKey()
    {
        if (Input.GetKey(KeyCode.L))
        {
            LoadNextScene();
        }
        //toggle collisions
        if (Input.GetKeyDown(KeyCode.C))
        {            
            areCollisionsDISABLED = !areCollisionsDISABLED;
            Debug.LogFormat("PRESS C {0}", areCollisionsDISABLED);
        }
    }
    //
    private void OnCollisionEnter(Collision otherCollider)
    {
        if(myState != State.Alive || areCollisionsDISABLED) { return; } //ignore collision while dead or transending.
        //
        currentScene = SceneManager.GetActiveScene().buildIndex;
        switch (otherCollider.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Tag":
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
        //player win
        myState = State.Transcending;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(landingWin);
        landingWinParticles.Play();
        Invoke("LoadNextScene", sceneDelay);
    }
    //
    private void StartDeathSequence()
    {
        //player death
        myState = State.Dying;
        myAudioSource.Stop();
        myAudioSource.PlayOneShot(crashDeath);
        crashDeathParticles.Play();
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
        if (currentScene >= 6)
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
            //myAudioSource.Stop(); //THIS IS KEEPING MY OTHER AUDIO FROM PLAYING.
            mainEngineParticles.Stop();
        }
    }
    //
    private void ApplyThrust()
    {
        myRB.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        if (!myAudioSource.isPlaying)
        {
            myAudioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }
}
