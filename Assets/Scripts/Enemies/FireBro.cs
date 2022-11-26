using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBro : Enemy
{
    public float projectileFireRate;
    float timeSinceLastFire;

    [HideInInspector]
    public bool active = false;

    Shoot shootScript;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        shootScript = GetComponent<Shoot>();
        shootScript.OnProjectileSpawned.AddListener(UpdateTimeSinceLastFire);
    }

    public void OnDisable()
    {
        shootScript.OnProjectileSpawned.RemoveListener(UpdateTimeSinceLastFire);
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            AnimatorClipInfo[] currentClips = anim.GetCurrentAnimatorClipInfo(0);

            if (currentClips[0].clip.name != "Fire")
            {
                if (Time.time >= timeSinceLastFire + projectileFireRate)
                {
                    anim.SetTrigger("Fire");
                }
            }
        }
    }

    public override void Death()
    {
        Destroy(gameObject);
    }

    void UpdateTimeSinceLastFire()
    {
        timeSinceLastFire = Time.time;
    }
}
