using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AudioSystem
{    
    public class AudioManager : MonoBehaviour
    {
        // Members
        public static AudioManager Instance {get; private set;}

        [Header("Settings")]
        [SerializeField] private Logger _Logger;
        [SerializeField] private AudioSettings _AudioSettings;

        // An Audio Track is a list of audio sounds that should not be played concurrently
        [Header("Audio Tracks")]
        [SerializeField] private AudioTrack[] _AudioTracks;  

        private Dictionary<AudioClip, AudioTrack> _AudioTable; // AudioClip get the AudioTrack that it belongs to 

#region Unity Functions
        private void Awake ()
        {
            _AudioSettings = null;
            Configure();
        }

        private void Start ()
        {
            if (AudioSettingsUI.Instance != null)
                AudioSettingsUI.Instance.OnAudioUpdate += UpdateAudio;
        }
#endregion
#region Public Functions
        public void PlayAudio (AudioClip clip)
        {
            if (!clip) return;
            
            if (_AudioTable[clip] == null)
            {
                _Logger.Log(gameObject, clip.name + " does not exist");
                return;
            }

            if (_AudioTable[clip].Type != AudioType.BGM)
            {
                _AudioTable[clip].Source.PlayOneShot(clip);
            }
            else
            {
                _AudioTable[clip].Source.clip = clip;
                _AudioTable[clip].Source.Play();
            }
            
            _Logger.Log(gameObject, "Playing " + clip.name);
        }
        
        public void PauseAudio ()
        {

        }

        public void StopAudio ()
        {

        }

        public void UpdateAudio (object sender, OnAudioUpdateArgs e)
        {
            _Logger.Log(gameObject, "Updating Audio");

            _AudioSettings = e.AudioSettings;
            _AudioSettings.ToString();

            foreach (AudioTrack track in _AudioTracks)
            {
                switch (track.Name)
                {
                    case "BGM":
                        track.Source.volume = track.BaseVolume * _AudioSettings.Master * _AudioSettings.BGM;
                        track.Source.mute = _AudioSettings.MuteMaster || _AudioSettings.MuteBGM;
                        break;

                    case "SFX Skill Use":
                        track.Source.volume = track.BaseVolume * _AudioSettings.Master * _AudioSettings.SkillUseSFX;
                        track.Source.mute = _AudioSettings.MuteMaster || _AudioSettings.MuteSkillUseSFX;
                        break;
                        
                    case "SFX Skill Hit":
                        track.Source.volume = track.BaseVolume * _AudioSettings.Master * _AudioSettings.SkillHitSFX;
                        track.Source.mute = _AudioSettings.MuteMaster || _AudioSettings.MuteSkillHitSFX;
                        break;
                    
                    case "SFX Mob Hit":
                        track.Source.volume = track.BaseVolume * _AudioSettings.Master * _AudioSettings.MobHitSFX;
                        track.Source.mute = _AudioSettings.MuteMaster || _AudioSettings.MuteMobHitSFX;
                        break;
                }
            }
        }

#endregion
#region  Private Functions
        private void Configure ()
        {
            if (Instance)
                Destroy(gameObject);
            else
                Instance = this;

            // Generate AudioTable
            _AudioTable = new Dictionary<AudioClip, AudioTrack>();

            foreach (AudioTrack audioTrack in _AudioTracks)
                foreach (AudioClip clip in audioTrack.Clips)
                    _AudioTable.Add(clip, audioTrack);
        }
#endregion
#region Audio Track
        public enum AudioType 
        {
            BGM, SFX
        }

        [System.Serializable]
        public class AudioTrack
        {
            public string Name;
            public AudioType Type;
            public float BaseVolume;
            public AudioSource Source;
            public AudioClip[] Clips;
        }
#endregion
    }
}