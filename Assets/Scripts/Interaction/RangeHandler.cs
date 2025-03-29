using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction
{
    public class RangeHandler : MonoBehaviour
    {
        public UnityEvent<Transform> onTriggerEnter;
        public UnityEvent<Transform> onTriggerExit;
        
        private void OnTriggerEnter(Collider other)
        {
            onTriggerEnter.Invoke(other.transform);
        }

        private void OnTriggerExit(Collider other)
        {
            onTriggerExit.Invoke(other.transform);
        }
    }
}
