using UnityEngine;

public class Tire : MonoBehaviour
{
    // Game Objects
    public Transform tireTransform;
    public Transform carTransform;
    public Rigidbody carRigidBody;
    public Transform tireInnerTransform;
    public Car car;

    // Debug
    public LineHelper xAxis;
    public LineHelper yAxis;
    public LineHelper zAxis;

    // constants
    public bool traction = true;
    public float torque = 1.0f;
    public float springRestDistance = 1.0f;
    public float springStrenght = 1.0f;
    public float springDamping = 1.0f;
    public float tireMass = 1.0f;
    public float carTopSpeed = 1.0f;
    public float steerAngle = 20.0f;

    public AnimationCurve powerCurve;
    public AnimationCurve slideCurve;
    public AnimationCurve frictionCurve;
    // visual tire suspension
    public float visualTireSmooth = 0.3f;
    
    float steer = 0;
    float acceleration = 0;

    float oldTireDistance;
    
    float tireDistance;

    // void Start()
    // {
    //     tireInnerTransform.gameObject.SetActive(false);
    // }

    public void Accelerate(float gas)
    {
        acceleration = gas;
    }

    public void Steer(float steer)
    {
        this.steer = steer;

        tireTransform.localRotation = Quaternion.AngleAxis(steer * steerAngle, Vector3.up);
        // tireInnerTransform.localRotation = Quaternion.AngleAxis(90.0f, Vector3.forward) * Quaternion.AngleAxis(steer * steerAngle, Vector3.right);
    }

    void Update()
    {
        // tireInnerTransform.localPosition = tireDistance * Vector3.up;
        // Vector3 velocity = Vector3.zero;
        // Vector3.SmoothDamp(tireInnerTransform.localPosition, tireDistance * Vector3.up, ref velocity, visualTireSmooth);
        float velocity = 0.0f;
        float smoothedDistance = Mathf.SmoothDamp(oldTireDistance, tireDistance, ref velocity, visualTireSmooth);
        tireInnerTransform.localPosition = smoothedDistance * Vector3.up;
        oldTireDistance = tireDistance;
    }

    void FixedUpdate()
    {
        float offsetDistance = 0;
        if (Physics.Raycast(tireTransform.position, Vector3.down, out RaycastHit hit, 10))
        {
            Debug.DrawLine(tireTransform.position, tireTransform.position + 10f * Vector3.down, Color.white);
            offsetDistance = hit.distance;
        }

        // physics
        Vector3 tireWorldVel = carRigidBody.GetPointVelocity(tireTransform.position);

        // spring
        Vector3 springDir = tireTransform.up;
        float offset = springRestDistance - offsetDistance;
        float velocity = Vector3.Dot(springDir, tireWorldVel);
        float force = offset * springStrenght - velocity * springDamping;
        
        if(force > 0)
        {
            Vector3 yForce = springDir * force;
            carRigidBody.AddForceAtPosition(yForce, tireTransform.position);
            Debug.DrawLine(tireTransform.position, tireTransform.position + yForce, Color.green);
            tireDistance = -0.5f * offsetDistance; 
        }


        if(offsetDistance <= springRestDistance)
        {
            // acceleration / braking
            Vector3 accelDir = tireTransform.forward;
            float carSpeed = Vector3.Dot(carTransform.forward, carRigidBody.linearVelocity);
            float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);
            
            if(acceleration != 0)
            {
                float availableTorque = powerCurve.Evaluate(normalizedSpeed) * acceleration * torque;
                Vector3 zForce = accelDir * availableTorque;

                carRigidBody.AddForceAtPosition(zForce, tireTransform.position);
                
                Debug.DrawLine(tireTransform.position, tireTransform.position + zForce, Color.blue);        
            }
            else if(!car.IsAccelerating())
            {
                // friction
                Vector3 frictionDir = tireTransform.forward;
                float frictionVel = Vector3.Dot(frictionDir, tireWorldVel);
                float tireFirction = frictionCurve.Evaluate(normalizedSpeed);
                float desiredFrictionVelChange = -frictionVel * tireFirction;
                float desiredFrictionAccel = desiredFrictionVelChange / Time.fixedDeltaTime;
                Vector3 zFrictionForce = frictionDir * tireMass * desiredFrictionAccel;
                
                carRigidBody.AddForceAtPosition(zFrictionForce, tireTransform.position);
                
                Debug.DrawLine(tireTransform.position, tireTransform.position + zFrictionForce, Color.yellow);        

            }

            // steering
            Vector3 steeringDir = tireTransform.right;
            float steerringVel = Vector3.Dot(steeringDir, tireWorldVel);
            float tireGripFactor = slideCurve.Evaluate(normalizedSpeed);
            float desiredVelChange = -steerringVel * tireGripFactor;
            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
            Vector3 xForce = steeringDir * tireMass * desiredAccel;

            carRigidBody.AddForceAtPosition(xForce, tireTransform.position);

            Debug.DrawLine(tireTransform.position, tireTransform.position + xForce, Color.red);
        }
    }
}
