using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {

    [SerializeField] float mainThrust = 30f;
    [SerializeField] float rcsThrust = 250f;

    private Rigidbody rigidBody;
    private AudioSource audioSource;

    private enum State
    {
        Alive, Dying, Transcending
    }

    State state = State.Alive;

	// Use this for initialization
	void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (state.Equals(State.Alive))
        {
            Thrust();
            Rotate();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control

        var rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; // resume physics contoll of rotation
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        if (Input.GetKey(KeyCode.S))
        {
            rigidBody.AddRelativeForce(- Vector3.up * mainThrust / 2);
        }
    }

    void OnCollisionEnter(Collision collider)
    {
        if(state != State.Alive)
        {
            return;
        }

        switch(collider.gameObject.tag)
        {
            case "Finish":
                state = State.Transcending;
                Invoke("LoadTheNextLevel", 1f);
                break;
            case "Respawn":
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadTheNextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
