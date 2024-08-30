using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationSystem : MonoBehaviour
{
    public Navigator[] navigators;
    public int activeNavigator;
    private void Update()
    {
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
}
