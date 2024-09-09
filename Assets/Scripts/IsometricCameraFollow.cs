using UnityEngine;

namespace IsometricPlayground
{
    public class IsometricCameraFollow : MonoBehaviour
    {
        [SerializeField] private IsometricPlayerController playerController;
        
        private void Update()
        {
            if (playerController)
            {
                transform.position = playerController.transform.position;
            }
        }
    }
}
