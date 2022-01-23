using UnityEngine;

// this script contains the End Trigger behaviour (called when the player reaches the end of the level)
public class EndTrigger : MonoBehaviour
{
    public GameManager gameManager;

    // called when the tank touches the End Trigger
    private void OnTriggerEnter(Collider other)
    {
        // informs Game Manager that the player completed the level
        gameManager.CompleteLevel();
        Debug.Log("YOU WON");
    }
}
