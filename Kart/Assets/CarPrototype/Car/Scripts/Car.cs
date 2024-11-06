using UnityEngine;
using System.Collections.Generic;

public class Car : MonoBehaviour
{

    public Rigidbody rigidbody;
    public Tire frontRightTire;
    public Tire frontLeftTire;
    public Tire backRightTire;
    public Tire backLeftTire;

    List<Tire> tires;

    bool isAccelerating;

    void Start()
    {
        tires = new List<Tire>();
        tires.Add(frontRightTire);
        tires.Add(frontLeftTire);
        tires.Add(backRightTire);
        tires.Add(backLeftTire);
    }

    public void Accelerate(float gas)
    {
        foreach(Tire tire in tires)
            if (tire.traction) 
                tire.Accelerate(gas);
        
        isAccelerating = gas > 0;
        // frontLeftTire.Accelerate(gas);
        // frontRightTire.Accelerate(gas);
    }

    public void Steer(float steer)
    {
        // foreach(Tire tire in tires) tire.Steer(steer);
        frontLeftTire.Steer(steer);
        frontRightTire.Steer(steer);
    }

    public float GetSpeed()
    {
        return rigidbody.GetPointVelocity(transform.position).magnitude;
    }
    public bool IsAccelerating()
    {
        return isAccelerating;
    }
}
