using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{
    public NavMeshAgent agent;
    public float speed;
    public float defaultSpeed;
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public virtual void Navigate()
    {
    }
    public virtual void Update()
    {
    }
    public virtual void SetPoint(Vector3 p)
    {
    }
    public virtual void SetSpeed(float value)
    {
        speed = value;
    }
    public virtual void SetDefaultSpeed()
    {
        SetSpeed(defaultSpeed);
    }
}
