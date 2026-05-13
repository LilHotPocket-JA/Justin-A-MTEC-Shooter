using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.XR;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI.Table;

public class Enemy : MonoBehaviour

{
    private CharacterController characterController;

    public Transform player;
    private Vector3 tP;
    private Vector3 dirToPlayer;

    public float speed =  5;


    public float viewAng = 120;
    public float viewRang = 5;
    public float detectR = 0.5f;

    public LayerMask playerLayer;

    private Vector3 currMovement;
    float g = 9.8f;

    public float rotSpd = 5;

    private NavMeshAgent agent;

    public Transform[] waypoints;
    private Transform targetWaypoint;
    private int waypointIndex = 0;

    private bool patrolling = true;
    private bool playerFound = false;

    private Vector3 lastKnownPosition;
    public float alertDuration = 5;
    private float tSA = 0;

    public float timeWaist = 0;
    public float timeUration = 5;
    private bool isWaiting;

    private Animator anim;
    void Start()
    {

        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        SetNextTargetWaypoint(true);
        anim = GetComponent<Animator>();
    }

    private void SetNextTargetWaypoint(bool start = false)
    {
        if (!start)
        {
          waypointIndex++;
        }
        
        if (waypointIndex >= waypoints.Length) 
        { 
            waypointIndex = 0;
        }

        targetWaypoint = waypoints[waypointIndex];
        agent.SetDestination(targetWaypoint.position);
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("Velocity", agent.desiredVelocity.magnitude);
        //targetWaypoint = waypoints[waypointIndex];

        dirToPlayer = (player.position - transform.position).normalized;
        Quaternion rota =  Quaternion.LookRotation(dirToPlayer);
        tP = new Vector3(player.position.x, player.position.y, player.position.z);

        //characterController.Move(currMovement * Time.deltaTime);
       // currMovement.y -= g * Time.deltaTime;

        if (patrolling)
        {
            Patrol();
        }


        /*if (PlayerDetected())
        {
            //transform.localRotation = Quaternion.RotateTowards(transform.localRotation, rota, rotSpd * Time.deltaTime);
            patrolling = false;
            playerFound  =true;

            tSA = 0;
            lastKnownPosition = player.position;
            //Debug.Log("Player found!");
            agent.SetDestination(lastKnownPosition);
        }*/

        if (playerFound)
        {
            if (tSA < alertDuration)
            {
                //Debug.Log("Looking for player");
                tSA += Time.deltaTime;
            }
            else
            {
                //Debug.Log("Returning to patrol");
                playerFound = false;
                tSA = 0;
                patrolling = true;
                SetNextTargetWaypoint(true);
            }

        }


        //characterController.Move(transform.forward * speed * Time.deltaTime);

    }

    private void Patrol()
    {
        float dist = Vector3.Distance(transform.position, targetWaypoint.position);
        float buffer = 0.25f;

        if(dist > buffer && !isWaiting)
        {
            isWaiting = true;
        }

        if (isWaiting)
        {
            if(timeWaist < timeUration)
            {
                timeWaist += Time.deltaTime;
            }

            else
            {
                timeWaist = 0;
                isWaiting = false;
                SetNextTargetWaypoint();
            }
        }


    }

    private bool PlayerDetected()
    {
        bool result = false;
        float angle = Vector3.Angle(transform.forward, dirToPlayer);

        if (angle < viewAng / 2)
        {
            if (Physics.Raycast(transform.position, dirToPlayer, viewRang, playerLayer))
            {
                result = true;
            }
        }

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= detectR)
        {
            result = true;
        }

        return result;
    }

}
