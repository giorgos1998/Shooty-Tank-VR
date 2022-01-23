using UnityEngine;

// script that controls the falling rocks event
public class Rockfall : MonoBehaviour
{
    public GameObject rockfallWarningUI;
    public GameObject rock;
    public Transform startingPos;
    public Transform endingPos;
    public Transform spawnPoint;
    public float fallHeight = 20f;
    public int rockCount = 10;
    public float timeBetweenRocks = 1f;

    private bool spawnRocks = false;
    private float randomXPos;
    private float randomZPos;
    float timer = 0f;

    // triggers when the tank enters the Rockfall Trigger
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Tank")
        {
            // get a random X and Y position between the Starting and Ending position GameObjects
            randomXPos = Random.Range(startingPos.position.x, endingPos.position.x);
            randomZPos = Random.Range(startingPos.position.z, endingPos.position.z);
            // move the rock's spawn point at the random position
            spawnPoint.position = new Vector3(randomXPos, fallHeight, randomZPos);
            // scale down the spawn point (rocks are spawn point's children and inherit its size), or else the rocks are huge (size of the trigger)
            spawnPoint.localScale = new Vector3(1f / 15f, 1f / 5f, 1f / 5f);
            // activate the warning UI
            rockfallWarningUI.SetActive(true);
            // enable rock spawning one second after the warning
            Invoke("BeginRockfall", 1f);
        }
    }

    // enables rock spawning
    private void BeginRockfall()
    {
        spawnRocks = true;
    }

    // Update is called once per frame
    private void Update()
    {
        // if rock spawning is enabled
        if (spawnRocks)
        {
            // simple timer using the time between two frames
            timer += Time.deltaTime;
            if (timer >= timeBetweenRocks)
            {
                // drop a rock with interval timeBetweenRocks
                timer = 0f;

                // get a random X and Y position between the Starting and Ending position GameObjects
                randomXPos = Random.Range(startingPos.position.x, endingPos.position.x);
                randomZPos = Random.Range(startingPos.position.z, endingPos.position.z);
                // create a rock with the same transform as the spawn point
                GameObject createdRock = (GameObject)GameObject.Instantiate(rock, spawnPoint);
                // move the rock at the random position
                createdRock.transform.position = new Vector3(randomXPos, fallHeight, randomZPos);

                // decrease the remaining rocks count
                rockCount--;
                // if we have spawned all the rocks, disable rock spawning
                if (rockCount == 0)
                {
                    spawnRocks = false;
                }
            }
        }
    }
}
