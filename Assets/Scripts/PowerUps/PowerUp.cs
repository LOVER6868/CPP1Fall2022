using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PickupType
    {
        Powerup =0,
        Life = 1,
        Score = 2
    }

    public PickupType currentPickup;
    public AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //PlayerController currPlayer = collision.gameObject.GetComponent<PlayerController>();

            switch(currentPickup)
            {
                case PickupType.Life:
                    GameManager.instance.lives++;
                    break;
                case PickupType.Score:
                    break;
                case PickupType.Powerup:
                    break;
            }
            if (pickupSound)
                collision.gameObject.GetComponent<AudioSourceManager>().PlayOneShot(pickupSound, false);

            Destroy(gameObject);
        }
    }
}
