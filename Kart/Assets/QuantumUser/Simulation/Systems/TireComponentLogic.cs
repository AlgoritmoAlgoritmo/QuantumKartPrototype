using Photon.Deterministic;
using Quantum;

public unsafe partial struct TireComponent {
	public void Accelerate( PhysicsBody3D* carPhysicsBody,
								FP acceleration,
								FPVector3 direction )
	{
		carPhysicsBody->AddForce( direction * acceleration * 50 );
	}
}