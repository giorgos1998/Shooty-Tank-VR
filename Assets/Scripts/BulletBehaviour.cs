using UnityEngine;

// this script controls the bullet's behaviour once it is fired from the tank
public class BulletBehaviour : MonoBehaviour
{
    public float bulletSpeed = 70f;
    public GameObject impactEffect;
    public GameObject explosionEffect;
    public AudioClip explosionSound;
    
    private Transform target;

    // Seek() is used to pass the target parameter from PlayerShoot to the created bullet
    public void Seek(Transform _target)
    {
        target = _target;
    }

    // Update is called once per frame
    void Update()
    {
        // if for some reason we don't have a target, the bullet gets destroyed
        if (target == null)
        {
            Destroy(gameObject);
            // return is needed in order to stop the rest of the Update() method from executing
            // destruction of a GameObject takes some time
            return;
        }

        // find direction vector starting from the bullet position and ending at the target
        Vector3 dir = target.position - transform.position;
        // calculate the distance the bullet will travel in this frame towards the target
        float frameTravel = bulletSpeed * Time.deltaTime;

        // if distance from target is equal/smaller than the distance bullet travels in this frame, we've hit the target
        if (dir.magnitude <= frameTravel)
        {
            HitTarget();
            // return again stops the rest of the Update() method from executing
            return;
        }

        // move the bullet towards the target
        // we normalize the direction vector so that the travel speed is irrelevant of the distance from the target
        transform.Translate(dir.normalized * frameTravel, Space.World);
        // rotate the bullet towards the target (e.g. if we are shooting at an object on the left of the tank)
        transform.LookAt(target);
    }

    private void HitTarget()
    {
        Vector3 pos = target.position;

        // create an impact effect on the spot the bullet hits the target and destroy it after 3 seconds
        // also destroys the bullet instantly
        GameObject impactEffectInstance = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactEffectInstance, 3f);
        Destroy(gameObject);

        // create an explosion effect on the target and destroy it after 5 seconds
        // also destroys the target instantly
        GameObject explosionInstance = (GameObject)Instantiate(explosionEffect, target.position, target.rotation);
        Destroy(explosionInstance, 5f);
        Destroy(target.gameObject);

        // play an explosion sound at the destroyed target's position
        AudioSource.PlayClipAtPoint(explosionSound, pos);
    }
}
