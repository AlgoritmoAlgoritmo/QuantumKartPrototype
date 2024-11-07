namespace Quantum
{
    using Photon.Deterministic;
    using UnityEngine.Scripting;
    using UnityEngine;

    [Preserve]
    public unsafe class CarSystem : SystemMainThreadFilter<CarSystem.Filter>,
                                    ISignalOnPlayerAdded
    {
        public void OnPlayerAdded( Frame f, PlayerRef player, bool firstTime )
        {
            var entity = f.Create( f.GetPlayerData( player ).PlayerAvatar );
            var link = new CarLink() {
                Player = player
            };

            f.Add( entity, link );

            if( f.Unsafe.TryGetPointer<Transform3D>( entity, out var transform ) ) 
            {
                transform->Position = new FPVector3( player * 2, 2, 0 );
            }
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.Link->Player);

            Debug.Log( "Horizontal: " + input->Horizontal );
            Debug.Log( "Vertical: " + input->Vertical );
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody3D* PhysicsBody3D;
            public CarLink* Link;
        }
    }
}
