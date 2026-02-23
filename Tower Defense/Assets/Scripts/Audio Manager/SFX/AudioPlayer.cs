using System;
using UnityEngine;

namespace Audio_Manager.SFX
{
    public class AudioPlayer : MonoBehaviour
    {
        public Sound sound;
        private void OnEnable()
        {
            AudioManager.Instance.Play(sound);
        }
    }
}