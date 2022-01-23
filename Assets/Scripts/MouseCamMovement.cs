using UnityEngine;

////////// WARNING: DISABLE SCRIPT TO USE GYROSCOPE ROTATION //////////

// script used for debugging, rotates the player camera using the mouse instead of the gyroscope
public class MouseCamMovement : MonoBehaviour
{
    public float sensitivity = 2f;

    private Vector2 currentRotation;

    // Update is called once per frame
    void Update()
    {
        // get rotation based on mouse movement
        currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
        currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
        // reset rotation to 0 if rotation is 360 degrees
        currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
        // clamp vertical rotation so that the camera doesn't turn upside down
        currentRotation.y = Mathf.Clamp(currentRotation.y, -80f, 80f);
        // set camera rotation
        transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0f);
    }
}
