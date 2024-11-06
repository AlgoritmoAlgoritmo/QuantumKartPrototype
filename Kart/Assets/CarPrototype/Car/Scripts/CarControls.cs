using UnityEngine;

public class CarControls : MonoBehaviour
{
    public Car car;

    private float horizontalInput;
    private float verticalInput;

    void Update()
    {
        car.Accelerate(Input.GetAxis("Vertical"));
        car.Steer(Input.GetAxis("Horizontal"));
    }
}
