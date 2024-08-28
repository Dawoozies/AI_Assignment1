using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healer : WorldObject
{
    public Transform nearestHealthDepleted;
    public Transform nearestVampire;
    public enum State
    {
        Fleeing,
        DispensingHealing,
    }
    public State state;
    public int health;
    void Update()
    {

    }
}
