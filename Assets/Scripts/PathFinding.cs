//91
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class PathFinding : MonoBehaviour
{
    public Transform[] points;
    private int destpoint;
    private NavMeshAgent nav;

    public GameObject PlayerCar;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerCar = GameObject.FindGameObjectWithTag("Player");
        points[0] = PlayerCar.transform;
    }
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!nav.pathPending && nav.remainingDistance < 12.5f)
        {
            GoToNextPoint();
        }

    }
    void GoToNextPoint()
    {
        if (points.Length == 0)
        {
            return;
        }

        nav.destination = points[destpoint].position;
        destpoint = (destpoint + 1) % points.Length;

    }
}
