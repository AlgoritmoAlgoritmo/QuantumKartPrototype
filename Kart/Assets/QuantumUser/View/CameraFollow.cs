using UnityEngine;


namespace Quantum {
    public class CameraFollow : QuantumEntityViewComponent<CustomViewContext>
    {
        #region Variables
        [Header( "Target Settings" )]
        public float distance = 5f;  // Distance behind the target
        public float height = 2f;    // Height above the target

        [Header( "Smooth Damping Settings" )]
        public float smoothTime = 0.3f; // Time to smoothly reach the target position

        private Vector3 velocity = Vector3.zero;
        private bool isLocal;
        #endregion


        #region Public methods
        public override void OnActivate( Frame frame ) 
        {
            var link = frame.Get<CarLink>(EntityRef);
            isLocal = Game.PlayerIsLocal(link.Player);
        }

        public override void OnLateUpdateView()
        {
            if( !isLocal )
                return;

            // Calculate the desired position behind the target
            Vector3 targetPosition = transform.position - transform.forward * distance + Vector3.up * height;

            // Smoothly move the camera to the target position
            ViewContext.MyCamera.transform.position = Vector3.SmoothDamp(
                                                        ViewContext.MyCamera.transform.position, 
                                                        targetPosition,
                                                        ref velocity,
                                                        smoothTime );

            // Make the camera look at the target
            ViewContext.MyCamera.transform.LookAt( transform.position + Vector3.up * height * 0.5f );
        }
        #endregion
    }
}