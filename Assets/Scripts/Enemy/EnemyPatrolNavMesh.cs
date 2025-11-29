using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolNavMesh : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTimeAtPoint = 1.5f;

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private float waitTimer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogError("Asigná waypoints en el inspector.");
            enabled = false;
            return;
        }
    }

    void OnEnable()
    {
        GoToCurrentWaypoint();
    }

    void Update()
    {
        if (agent.pathPending) return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTimeAtPoint)
            {
                NextWaypoint();
                GoToCurrentWaypoint();
                waitTimer = 0f;
            }
        }
    }

    void NextWaypoint()
    {
        currentIndex = (currentIndex + 1) % waypoints.Length;
    }

    void GoToCurrentWaypoint()
    {
        agent.SetDestination(waypoints[currentIndex].position);
    }
}

