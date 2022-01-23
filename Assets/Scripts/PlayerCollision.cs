using UnityEngine;

// script handles player collision with obstacles
public class PlayerCollision : MonoBehaviour
{
    // called when the tank collides with something
    private void OnCollisionEnter(Collision collision)
    {
        // if the tank collided with an 'Obstacle' or a 'Breakable' obstacle, inform the Game Manager that the player lost
        if (collision.collider.tag == "Obstacle" || collision.collider.tag == "Breakable")
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
