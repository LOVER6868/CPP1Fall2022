using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float lifetime;
    public int floorHits;

    [HideInInspector]
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (lifetime <= 0)
            lifetime = 2.0f;

        if (floorHits <= 0)
            floorHits = 2;

        GetComponent<Rigidbody2D>().velocity = new Vector2(speed, 0);
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
            floorHits--;

        if (floorHits < 0)
            Destroy(gameObject);

        if (gameObject.CompareTag("PlayerProjectile"))
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                GameManager.instance.lives--;
                Destroy(gameObject);
            }
        }
    }
}
