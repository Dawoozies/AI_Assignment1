using UnityEngine;
using UnityEngine.AI;

public class RandomWalk : Navigator
{
    [SerializeField] float stepRadius;
    [SerializeField] Vector2 speedBounds;
    [SerializeField] Vector2 waitTimeBounds;
    float _waitTime;
    public override void Navigate()
    {
        if (_waitTime <= 0f)
        {
            Vector3 p = transform.position + Random.onUnitSphere * stepRadius;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(p, out hit, stepRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }

            _waitTime = Random.Range(waitTimeBounds.x, waitTimeBounds.y);
            agent.speed = Random.Range(speedBounds.x, speedBounds.y);
        }
    }
    public override void Update()
    {
        base.Update();
        if (_waitTime > 0f)
        {
            _waitTime -= Time.deltaTime;
        }
    }
}
