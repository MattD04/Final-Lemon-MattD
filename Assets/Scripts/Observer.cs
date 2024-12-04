using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    public Transform player;
    public GameEnding gameEnding;
    bool m_IsPlayerInRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsPlayerInRange)
        {
            Vector3 direction = player.position - transform.position + Vector3.up;  //direction from the PointOfView GameObject to JohnLemon is JohnLemon’s position minus the PointOfView GameObject’s position
            Ray ray = new Ray(transform.position, direction); //defines a newray

            RaycastHit raycastHit;  //making a new variable to detect when a Raycast makes contact

            if(Physics.Raycast(ray, out raycastHit))  //if something hits the raycast as it outputs
            {
                if (raycastHit.collider.transform == player) //collides with player
                {
                    gameEnding.CaughtPlayer();
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            m_IsPlayerInRange = false;
        }
    }

}
