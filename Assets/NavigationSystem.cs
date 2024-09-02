using UnityEngine;
using UnityEngine.AI;

public class NavigationSystem : MonoBehaviour
{
    public Navigator[] navigators;
    public int activeNavigator;
    NavMeshAgent agent;
    bool disabled;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (disabled)
        {
            agent.velocity = Vector3.zero;
            return;
        }
        navigators[activeNavigator].Navigate();
    }
    public void ChangeActiveNavigator(int index)
    {
        activeNavigator = index;
    }
    public void ChangePoint(Vector3 p)
    {
        navigators[activeNavigator].SetPoint(p);
    }
    public void Disable(bool value)
    {
        disabled = value;
    }
    public void SetSpeed(float value)
    {
        foreach (var item in navigators)
        {
            item.SetSpeed(value);
        }
    }
    public void SetDefaultSpeeds()
    {
        foreach (var item in navigators)
        {
            item.SetDefaultSpeed();
        }
    }
}
