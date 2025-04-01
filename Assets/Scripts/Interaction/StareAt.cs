using System;
using UnityEngine;

namespace Interaction
{
    public class StareAt : MonoBehaviour
    {
        private Camera _mainCamera;

        private void Start()
        {
            // Cache the main camera once at the start.
            _mainCamera = Camera.main;
            
            if (_mainCamera == null) enabled = false; // Ensure the camera is set

        }

        private void LateUpdate()
        {

            // Make the object face the camera by calculating the direction
            Vector3 directionToCamera = _mainCamera.transform.position - transform.position;
            directionToCamera.y = 0; // Optional: Ignore vertical rotation (so it only rotates horizontally)
            transform.rotation = Quaternion.LookRotation(directionToCamera);
        }
    }
}