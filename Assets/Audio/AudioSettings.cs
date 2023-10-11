using System;
using UnityEngine;

namespace AudioSystem
{
    [System.Serializable]
    public class AudioSettings
    {
        public float Master;
        public bool MuteMaster;
        
        public float BGM;
        public bool MuteBGM;

        public float SkillUseSFX;
        public bool MuteSkillUseSFX;

        public float SkillHitSFX;
        public bool MuteSkillHitSFX;

        public float MobHitSFX;
        public bool MuteMobHitSFX;

        public AudioSettings ()
        {
            Master = BGM = SkillUseSFX = SkillHitSFX = MobHitSFX = 1f;
            MuteMaster = MuteBGM = MuteSkillUseSFX = MuteSkillHitSFX = MuteMobHitSFX = false;
        }
    }

    public class OnAudioUpdateArgs : EventArgs
    {
        public AudioSettings AudioSettings;
    }
}