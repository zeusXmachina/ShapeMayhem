using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZXM.Managers
    {

    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// Simple manager to hold Audio Sources for all classes that require access
        /// </summary>


        public static AudioManager Instance;


        public AudioSource mainMenuSound;
        public AudioSource gameStartSound;
        public AudioSource pauseGameSound;
        public AudioSource collectibleSoundA;
        public AudioSource collectibleSoundB;
        public AudioSource gameOverSound;
        public AudioSource completedSound;


        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                // DontDestroyOnLoad(gameObject);
            }


        }


    }
}
