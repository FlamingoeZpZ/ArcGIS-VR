using System;
using UnityEngine;

namespace Interaction
{
    public class StareAt : MonoBehaviour
    {
        [SerializeField] private Transform target;

        private void Awake()
        {
            if(!target)
                if (Camera.main != null)
                    target = Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(target);
        }
    }
}
