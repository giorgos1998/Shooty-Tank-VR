using UnityEngine;

// script controls ingame background music (not on menus)
public class MusicControl : MonoBehaviour
{
    public Camera cam;

    private int playMusic = 1;

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
}
