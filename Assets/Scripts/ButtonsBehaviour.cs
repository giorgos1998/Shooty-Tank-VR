using UnityEngine;
using UnityEngine.SceneManagement;

// this script controls the behaviour of the Main menu and the End Credits menu
public class ButtonsBehaviour : MonoBehaviour
{
    public Camera cam;
    public GameObject mainMenu;
    public GameObject helpMenu;
    
    private int playMusic = 1;

    // called when player selects the Start button
    public void StartClick()
    {
        // loads 1st level
        Debug.Log("Pressed Start");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // called when player selects the Help button
    public void HelpClick()
    {
        // disables Main menu and enables Help menu
        Debug.Log("Pressed Help");
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    // called when player selects the Music On button
    public void MusicOnClick()
    {
        Debug.Log("Pressed Music ON");

        // sets music flag to true (1)
        playMusic = 1;
        // gets background music player
        AudioSource music = cam.GetComponent<AudioSource>();
        // if background music isn't playing already, start it
        if (!music.isPlaying)
        {
            music.Play();
        }
    }

    // called when player selects the Music Off button
    public void MusicOffClick()
    {
        Debug.Log("Pressed Music OFF");
        // sets music flag to false (0)
        playMusic = 0;
        // gets background music player
        AudioSource music = cam.GetComponent<AudioSource>();
        // if background music is playing, stop it
        if (music.isPlaying)
        {
            music.Stop();
        }
    }

    // called when player selects the Back to menu button
    public void BackToMenuClick()
    {
        // disables Help menu and enables Main menu
        Debug.Log("Pressed Back to menu");
        helpMenu.SetActive(false);
        mainMenu.SetActive(true);
        
    }

    // called when player selects the Quit button
    public void QuitClick()
    {
        // exits the app
        Debug.Log("Pressed Quit");
        Application.Quit();
    }

    // called when player selects the Back to menu button from the End Credits
    public void CreditsBackToMenuClick()
    {
        // loads the first Scene (Main menu)
        Debug.Log("Pressed Back to menu");
        SceneManager.LoadScene(0);
    }

    // store music setting when scene closes and reset score
    private void OnDisable()
    {
        // with PlayerPrefs the values stay stored even if we exit the app
        PlayerPrefs.SetInt("music", playMusic);
        PlayerPrefs.SetFloat("score", 0f);
    }

    // load music setting when we start the scene
    private void OnEnable()
    {
        playMusic = PlayerPrefs.GetInt("music");
        // if music flag is true (1) start the music player
        if (playMusic == 1)
        {
            AudioSource music = cam.GetComponent<AudioSource>();
            music.Play();
        }
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
