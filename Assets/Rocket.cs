using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 500f;
    [SerializeField]float mainThrust = 500f;
    [SerializeField] AudioClip mainEngine;
    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending}
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return;  //ignoring collisions when dead
        }
        switch (collision.gameObject.tag)
        {
            case ("Friendly"):
                break;
            case ("Finish"):
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //parameterise time 
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f); //parameterise as well
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //alow for more then 2 levels
    }

    private void RespondToThrustInput()
    {
        float mainThrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            Thrusting();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Thrusting()
    {
        float mainThrustThisFrame = mainThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(Vector3.up * mainThrustThisFrame);
        if (!audioSource.isPlaying) //anti layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; //take manual control

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //physics takes over
    }

   
}
