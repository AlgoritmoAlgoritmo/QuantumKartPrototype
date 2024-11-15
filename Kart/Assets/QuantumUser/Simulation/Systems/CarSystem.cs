using UnityEngine;
using UnityEngine.Scripting;
using Photon.Deterministic;


namespace Quantum {
    [Preserve]
    public unsafe class CarSystem : SystemMainThreadFilter<CarSystem.Filter>,
                                    ISignalOnPlayerAdded
    {
        #region Methods
        private string tiresFolder = "QuantumUser/Prefabs/SeparateEntities/";
        private string frontLeftTirePrototypeName = "FrontLeft_QuantumTireEntityPrototype";
        private string frontRightTirePrototypeName = "FrontRight_QuantumTireEntityPrototype";
        private string backLeftTirePrototypeName = "BackLeft_QuantumTireEntityPrototype";
        private string backRightTirePrototypeName = "BackRight_QuantumTireEntityPrototype";

        public struct Filter
        {
            public EntityRef Entity;
            public PhysicsBody3D* PhysicsBody3D;
            public Transform3D* Transform;
            public Quantum.CarComponent CarComp;
        }
        #endregion


        #region Public methods
        public void OnPlayerAdded( Frame f, PlayerRef player, bool firstTime )
        {
            var entity = f.Create( f.GetPlayerData( player ).PlayerAvatar );

            if( f.Unsafe.TryGetPointer<CarComponent>( entity, out var link ) )
            {
                link->Player = player;

                var tireEntityAsset = f.FindAsset<EntityPrototype>( 
                                        tiresFolder + frontLeftTirePrototypeName );
                var tireEntity = f.Create(tireEntityAsset);

                link->TireEntity = tireEntity;
            }


            if( f.Unsafe.TryGetPointer<Transform3D>( entity, out var transform ) ) 
            {
                transform->Position = new FPVector3( player * 2, 2, 0 );
            }
        }


        public override void Update(Frame f, ref Filter filter)
        {
            var input = f.GetPlayerInput(filter.CarComp->Player);
            var direction = filter.Transform->Up;

            Debug.Log( "filter.CarComponent->TireEntity " + filter.CarComp->TireEntity );

            if( f.Unsafe.TryGetPointer<TireComponent>( filter.CarComp->TireEntity, out var tire ) ) {
                Debug.Log( "Accelerating" );
                UpdateTire( tire, filter, input, direction, f );
            }
        }
        #endregion


        #region Private methods
        private void UpdateTire( TireComponent* tire, Filter filter, Quantum.Input* input, FPVector3 direction, Frame frame ) {
            tire->Accelerate( filter.PhysicsBody3D, input->Vertical, direction );
            frame.Unsafe.TryGetPointer<Transform3D>( filter.CarComp->TireEntity, out var transform );
            transform->Position = filter.Transform->Position + tire->PositionOffset;
        }
        #endregion
    }
}