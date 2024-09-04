using LitMotion;
using UnityEngine;
public class DoorMoving : MonoBehaviour
{
    public Vector3 start;
    public Vector3 end;
    public Ease easing;
    public float time;
    float _time;
    bool dir;
    void Start()
    {
        DoMotion();
    }
    void Update()
    {
        if (_time < time)
        {
            _time += Time.deltaTime;
        }
        else
        {
            _time = 0;
            DoMotion();
        }
    }
    void DoMotion()
    {
        Vector3 a;
        Vector3 b;
        if (dir)
        {
            a = start;
            b = end;
        }
        else
        {
            a = end;
            b = start;
        }
        LMotion.Create(a, b, time / 4f)
            .WithEase(easing)
            .Bind(x => transform.localPosition = x);
        dir = !dir;
    }
}