using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class WaypointPatrol : MonoBehaviour
{   
    //array transform for waypoints
    public Transform[] waypoints;
    int m_CurrentWaypointIndex;

    public NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        //starts at zero index
        navMeshAgent.SetDestination(waypoints[0].position);
    }

    // Update is called once per frame
    void Update()
    {
        //checks remaining distance to the destination is less than the stopping distance set in the Inspector
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            //updates index
            m_CurrentWaypointIndex = (m_CurrentWaypointIndex + 1) % waypoints.Length; //Add one to the current index, but if that increment puts the index equal to the number of elements in the waypoints array then instead set it to zero.”
            navMeshAgent.SetDestination(waypoints[m_CurrentWaypointIndex].position); //same as update, but uses whatever index Ghost is at
        }
    }
}
