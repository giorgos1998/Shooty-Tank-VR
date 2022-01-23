using UnityEngine;

// script moves player (camera) to follow the tank
public class MovePlayer : MonoBehaviour
{
    public GameObject player;
    public GameObject tank;

    // Update is called once per frame
    void Update()
    {
        player.transform.position = tank.transform.position;
    }
}
