using System;
using UnityEngine;

namespace Ldjam43
{
    public class AudioManager: MonoBehaviour
    {
        public AudioSource Source;
        public Ship Ship;
        public GameManager GameManager;

        [Header("Audio clips")]
        public AudioClip Thrust;
        public AudioClip Explosion;

        private void Start()
        {
            Ship.OnThurst += PlayThrust;
            Ship.OnThurstStop += StopSource;
            Ship.OnCollision += PlayExplosion;
            GameManager.OnWreckDestroyed += PlayExplosion;
            GameManager.OnPause += StopSource;
        }

        private void PlayExplosion()
        {
            Source.PlayOneShot(Explosion, 0.5f);
        }

        private void StopSource()
        {
            Source.Stop();
        }

        private void PlayThrust()
        {
            Source.clip = Thrust;
            Source.loop = true;
            Source.Play();
        }
    }
}
