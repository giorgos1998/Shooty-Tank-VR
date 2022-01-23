using UnityEngine;
using UnityEngine.UI;

// this script controls user input on the Menu screens
public class MenuClick : MonoBehaviour
{
    public Camera cam;
    // mask contains the layers that the Raycast can hit
    public LayerMask mask;
    public Image gazeCircleUI;
    public GameObject menuUI;

    private float timer = 0f;
    private GameObject activeTarget;
    private GameObject newTarget;
    private int rayChecks = 0;
    private bool selectEnabled = false;

    // when the Menu scene starts, initialize the menu UI to play the animation
    // also enable user input once the animation ends
    private void Start()
    {
        menuUI.SetActive(true);
        Invoke("ActivateSelect", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        // if user input is enabled
        if (selectEnabled)
        {
            // simple timer using the time between two frames
            timer += Time.deltaTime;
            if (timer >= 0.1)
            {
                // Raycast to find a new target every 0.1 seconds
                timer = 0f;
                newTarget = getTarget();

                // if player still looks at the same target and the target is clickable
                if (newTarget != null && newTarget == activeTarget && newTarget.tag == "Clickable")
                {
                    // raychecks stores how many rays in a row have hit the same target (it stores the time player gazes at the same thing)
                    rayChecks++;
                    // if the user looks for two seconds at the same target, click it (2.1 seconds for better animation)
                    if (rayChecks == 21)
                    {
                        // reset raychecks to 0 once the user clicked
                        rayChecks = 0;
                        ClickButton();
                    }
                }
                // if the user looks away from the active target or has no active target (active target = the last thing he aimed at)
                else
                {
                    // decrease rayChecks (don't set it to 0 as the user might miss the target for very little time involuntarily)
                    rayChecks--;
                    // don't leave rayChecks become negative (will take way more time to aim)
                    if (rayChecks < 0)
                    {
                        rayChecks = 0;
                    }
                    // set the new target as the active target (can be null)
                    activeTarget = newTarget;
                }
            }
            // fill the gaze circle with the correct amount each frame
            gazeCircleUI.fillAmount = rayChecks / 20f;
        }
    }

    // casts a Ray from the camera center and returns the first hit object or null if it didn't hit anything
    private GameObject getTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 50f, mask))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    // calls the onClick() method of the selected target
    private void ClickButton()
    {
        activeTarget.GetComponent<Button>().onClick.Invoke();
    }

    // enables user input
    private void ActivateSelect()
    {
        selectEnabled = true;
    }
}
