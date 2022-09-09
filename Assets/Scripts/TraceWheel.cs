using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraceWheel : MonoBehaviour
{
    private bool ToggleMove = false;
    private int direction = 1;
    public int DegreesPerSecond = 50;
    public float MinTime = 5.0f;
    public float MaxTime = 10.0f;
    private float timeToChangeOrientation;
    public int ChangeCount = 0;

    public void StartMove()
    {
        pickTimeToChangeOrientation();
        ToggleMove = true;
    }

    public void StopMove()
    {
        ToggleMove = false;
    }

    void Update()
    {
        if (ToggleMove)
        {
            HandleTime();
            Move();
        }
    }

    private void HandleTime()
    {
        timeToChangeOrientation -= Time.deltaTime;
        if (timeToChangeOrientation < 0)
        {
            changeDirection();
            pickTimeToChangeOrientation();
        }
    }

    private void changeDirection()
    {
        direction *= -1;
        ChangeCount++;
    }

    private void pickTimeToChangeOrientation()
    {
        timeToChangeOrientation = Random.Range(MinTime, MaxTime);
    }

    void Move()
    {
        transform.Rotate(new Vector3(0, 0, direction == 1 ? DegreesPerSecond : -DegreesPerSecond) * Time.deltaTime);
    }
}
