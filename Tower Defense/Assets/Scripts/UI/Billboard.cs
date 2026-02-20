using System;
using UnityEngine;

namespace UI
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private float refreshDuration;
        private Camera _cam;

        private void Start()
        {
            _cam = Camera.main;
            InvokeRepeating(nameof(LookAtCamera),0,refreshDuration);
        }

        void LookAtCamera()
        {
            transform.LookAt(_cam.transform);
        }
    }
}
