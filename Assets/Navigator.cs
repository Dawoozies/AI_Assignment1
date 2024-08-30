using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{
    public NavMeshAgent agent;
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
}
