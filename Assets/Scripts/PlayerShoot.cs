using UnityEngine;
using UnityEngine.UI;

// this script controls player aiming and shooting
public class PlayerShoot : MonoBehaviour
{
    [Header("Unity Reference Fields")]

    public Camera cam;
    // mask contains the layers that the Raycast can hit
    public LayerMask mask;
    public Transform turret;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public GameObject muzzleFlashEffect;
    public Sprite crosshairGreen;
    public Sprite crosshairRed;
    public Image gazeCircleUI;
    public Image crosshairUI;

    [Header("Gun/Turret Attributes")]

    public float gunRange = 100f;
    public float turretRotationSpeed = 10f;
    public float fireRate = 0.2f;
    

    private float timer = 0f;
    private GameObject activeTarget;
    private GameObject newTarget;
    private int rayChecks = 0;
    private float reloadCountdown = 0f;

    // Update is called once per frame
    private void Update()
    {
        // if the gun is reloaded
        if (reloadCountdown <= 0f)
        {
            // if the crosshair is red (gun not loaded), change it to green (gun loaded)
            if (crosshairUI.sprite == crosshairRed)
            {
                crosshairUI.sprite = crosshairGreen;
            }

            // simple timer using the time between two frames
            timer += Time.deltaTime;
            if (timer >= 0.1)
            {
                // Raycast to find a new target every 0.1 seconds
                timer = 0f;
                newTarget = getTarget();

                // if player still looks at the same target and the target is breakable
                if (newTarget != null && newTarget == activeTarget && newTarget.tag == "Breakable")
                {
                    // raychecks stores how many rays in a row have hit the same target (it stores the time player gazes at the same thing)
                    rayChecks++;
                    // if the user looks for two seconds at the same target, shoot it (2.1 seconds for better animation)
                    if (rayChecks == 21)
                    {
                        // reset raychecks to 0 once the user shot and reset the reload timer
                        rayChecks = 0;
                        Shoot();
                        reloadCountdown = 1f / fireRate;
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
        }
        // decrease reload timer
        reloadCountdown -= Time.deltaTime;
        // fill the gaze circle with the correct amount each frame
        gazeCircleUI.fillAmount = rayChecks / 20f;

        // track active target with turret
        if (activeTarget != null)
        {
            // keep starting rotation
            Vector3 startRotation = transform.rotation.eulerAngles;
            // find direction vector starting from the tank position and ending at the target
            Vector3 dir = activeTarget.transform.position - transform.position;
            // get the rotation needed to face the target
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            // get a smoother rotation towards the target (we don't want the turret to snap to the target)
            Vector3 rotation = Quaternion.Lerp(turret.rotation, lookRotation, Time.deltaTime * turretRotationSpeed).eulerAngles;
            // rotate the turret
            turret.rotation = Quaternion.Euler(startRotation.x, rotation.y, startRotation.z);
        }
    }

    // casts a Ray from the camera center and returns the first hit object or null if it didn't hit anything
    private GameObject getTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, gunRange, mask))
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    // called when the player has selected a target
    private void Shoot()
    {
        // makes the crosshair red to indicate that the gun is reloading
        crosshairUI.sprite = crosshairRed;

        // creates a new bullet at the tip of the gun and passes the target info (using Seek() method)
        GameObject bulletGO = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        BulletBehaviour bulletScript = bulletGO.GetComponent<BulletBehaviour>();
        bulletScript.Seek(activeTarget.transform);

        // creates an muzzle flash effect at the tip of the gun and destroys it after 5 seconds
        GameObject muzzleInstance = (GameObject)Instantiate(muzzleFlashEffect, firePoint.position, firePoint.rotation);
        Destroy(muzzleInstance, 5f);
        // plays a shooting sound effect at the tip of the gun
        firePoint.gameObject.GetComponent<AudioSource>().Play();
    }
}
