using UnityEngine;

// this script controls tank movement and rotation using the gyroscope or the keyboard (for debugging)
public class TankMovement : MonoBehaviour
{
    public GameObject tank;
    public float forwardForce = 500f;
    public float sideForce = 200f;
    public AudioSource engineStart;
    public AudioSource engineRun;

    private Rigidbody tankRB;
    private bool startedEngine = false;

    // called when the tank spawns
    private void Start()
    {
        // get tank's rigidbody (for movement) and play the start engine sound effect
        tankRB = tank.GetComponent<Rigidbody>();
        engineStart.Play();
    }

    private void FixedUpdate()
    {
        // if the start engine sound effect ended and the tank movement is disabled (startedEngine flag)
        if (!engineStart.isPlaying && !startedEngine)
        {
            // play the running engine sound effect and enable tank movement
            engineRun.Play();
            startedEngine = true;
        }

        // if tank movement is enabled
        if (startedEngine)
        {
            // add a forward force at the direction the tank is facing
            tankRB.AddRelativeForce(0, 0, forwardForce * Time.deltaTime);

            // rotate the tank based on the accelerometer tilt
            tank.transform.Rotate(0, Input.acceleration.x * sideForce * Time.deltaTime, 0);

            // get 'd' and 'a' keyboard keys and rotate the tank (for debugging)
            if (Input.GetKey("d"))
            {
                tank.transform.Rotate(0, sideForce * Time.deltaTime, 0);
            }

            if (Input.GetKey("a"))
            {
                tank.transform.Rotate(0, -sideForce * Time.deltaTime, 0);
            }
        }
        
        // if the tank falls under the platform, inform the Game Manager that the player lost
        if (tankRB.position.y < -1f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
