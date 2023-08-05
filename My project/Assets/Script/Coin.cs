using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int scoreValue;
    private AudioSource sound;

    void Awake()
    {
        sound = GetComponent<AudioSource>();
    }
    
    private void OnTriggerEnter2D(Collider2D collison)
    {
        if (collison.gameObject.tag == "Player")
        {
            sound.Play();
            GameController.instance.UpdateScore(scoreValue);
            Destroy(gameObject, 0.1f);
        }
    }
}
