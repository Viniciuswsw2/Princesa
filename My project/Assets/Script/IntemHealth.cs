using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntemHealth : MonoBehaviour
{
    public int healthvalue;
    
    private void OnCollisionEnter2D(Collision2D collison)
       {
          if (collison.gameObject.tag == "Player")
          {
             collison.gameObject.GetComponent<Player>().IncreaseLife(healthvalue);
             Destroy(gameObject);
          }
       }
}
