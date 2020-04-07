using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]float rcsThrust = 500f;
    [SerializeField]float mainThrust = 1250f;
    Rigidbody rigidBody;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }
    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case ("Friendly"):
                //do nothing
                print("OK"); //remove
                break;
            case ("Fuel"):
                print("FUEL");
                break;
            default:
                print("DEAD");
                break;


        }
    }
    private void Thrust()
    {
        float mainThrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrustThisFrame);
            if (!audioSource.isPlaying) //anti layer
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
    private void Rotate()
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
