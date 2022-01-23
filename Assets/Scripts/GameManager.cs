using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// manages general game behaviour (e.g. when player wins or loses, load the next level)
public class GameManager : MonoBehaviour
{
    public float restartDelay = 3f;
    public Text scoreUI;
    public Text scoreDefUI;
    public Transform tankTF;

    public GameObject completeLevelUI;
    public GameObject levelLostUI;
    public TankMovement movementScript;

    private bool gameEnded = false;
    private float oldScore;
    private float totalScore;

    // when a game scene that contains a GameManager is initialized, load saved score from previous levels
    private void OnEnable()
    {
        oldScore = PlayerPrefs.GetFloat("score");
    }

    // this method is called when the player loses
    public void EndGame()
    {
        // gameEnded is a flag used to run the EndGame methon only once per level (scene reload sets it again to false)
        if (gameEnded == false)
        {
            // stop the tank from moving
            movementScript.enabled = false;
            // game ended, wait for scene reload to call it again
            gameEnded = true;
            Debug.Log("GAME OVER");
            // calculate and display total score so far
            totalScore = oldScore + tankTF.position.z;
            levelLostUI.SetActive(true);
            scoreDefUI.text = totalScore.ToString("0");
            // call method to stop tank engine sound since it stopped moving
            this.StopAudio();
            // reload the scene after some delay
            Invoke("Restart", restartDelay);
        }
    }

    // reloads the scene
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // called when player reaches the end of the level
    public void CompleteLevel()
    {
        // stop the tank from moving
        movementScript.enabled = false;
        // calculate and display total score so far
        totalScore = oldScore + tankTF.position.z;
        completeLevelUI.SetActive(true);
        scoreUI.text = totalScore.ToString("0");
        // call method to stop tank engine sound since it stopped moving
        this.StopAudio();
        // save total score to use on the next level
        PlayerPrefs.SetFloat("score", totalScore);
        // load next level after some time
        Invoke("NextLevel", 3f);
    }

    // stops tank engine sound
    private void StopAudio()
    {
        AudioSource[] soundEffects = tankTF.gameObject.GetComponents<AudioSource>();
        foreach (var effect in soundEffects)
        {
            effect.Stop();
        }
    }

    // loads next level (scene)
    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // checks if the user pressed the exit button (top left X)
    private void Update()
    {
        // exits the app if exit button is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
