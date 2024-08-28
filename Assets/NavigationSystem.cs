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
}
