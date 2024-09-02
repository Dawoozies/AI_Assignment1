using UnityEngine;
using UnityEngine.AI;

public class PointNavigator : Navigator
{
    [SerializeField] Vector3 point;
    [SerializeField] float closeDistance;
    public bool nearPoint => Vector3.Distance(transform.position, point) <= closeDistance;
    public override void Navigate()
    {
        agent.speed = speed;
        Vector3 p = point;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(p, out hit, 10f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
    public override void SetPoint(Vector3 p)
    {
        point = p;
    }
}
