using System;
using UnityEngine;
using UnityEngine.UI;
using CoreSystems;

namespace AudioSystem
{
    public class AudioSettingsUI : MonoBehaviour
    {
        // Memebers
        public static AudioSettingsUI Instance {get; private set;}

        [Header("Sliders")]
        [SerializeField] private Slider _MasterSlider;
        [SerializeField] private Slider _BGMSlider;
        [SerializeField] private Slider _SkillUseSlider;
        [SerializeField] private Slider _SkillHitSlider;
        [SerializeField] private Slider _MobHitSlider;

        [Header("Toggles")]
        [SerializeField] private Toggle _MasterToggle;
        [SerializeField] private Toggle _BGMToggle;
        [SerializeField] private Toggle _SkillUseToggle;
        [SerializeField] private Toggle _SkillHitToggle;
        [SerializeField] private Toggle _MobHitToggle;

        private const string FILE = "AudioSettings";
        private AudioSettings _AudioSettings;
        private OnAudioUpdateArgs _EventArgs;

        public event EventHandler<OnAudioUpdateArgs> OnAudioUpdate;

#region UnityFunctions
        private void Awake ()
        {
            if (!Instance) Instance = this;
            else Destroy(this);
        }

        private void Start ()
        {
            //_AudioSettings = SaveSystem.Load<AudioSettings>("", FILE);
            //if (_AudioSettings == null) _AudioSettings = new AudioSettings();
            _AudioSettings = new AudioSettings();
            
            _EventArgs = new OnAudioUpdateArgs {AudioSettings = _AudioSettings};

            Configure();
            OnAudioUpdate?.Invoke(this, _EventArgs);
        }
#endregion
#region Public Functions

        public async void LoadAudioSettings ()
        {
            _AudioSettings = await SaveSystem.Instance.CloudLoad<AudioSettings>(FILE);
            if (_AudioSettings == null) _AudioSettings = new AudioSettings();
            _EventArgs = new OnAudioUpdateArgs {AudioSettings = _AudioSettings};

            Configure();
            OnAudioUpdate?.Invoke(this, _EventArgs);
        }

        public void SaveAudioSettings () => SaveSystem.Save(_AudioSettings, "", FILE);
        public void UpdateAudioSetting () => OnAudioUpdate?.Invoke(this, _EventArgs);     
#endregion
#region Private Functions
        private void Configure ()
        {
            _MasterSlider.value = _AudioSettings.Master;
            _BGMSlider.value = _AudioSettings.BGM;
            _SkillUseSlider.value = _AudioSettings.SkillUseSFX;
            _SkillHitSlider.value = _AudioSettings.SkillHitSFX;
            _MobHitSlider.value = _AudioSettings.MobHitSFX;

            _MasterToggle.isOn = !_AudioSettings.MuteMaster;
            _BGMToggle.isOn = !_AudioSettings.MuteBGM;
            _SkillUseToggle.isOn = !_AudioSettings.MuteSkillUseSFX;
            _SkillHitToggle.isOn = !_AudioSettings.MuteSkillHitSFX;
            _MobHitToggle.isOn = !_AudioSettings.MuteMobHitSFX;

            _MasterSlider.onValueChanged.AddListener(delegate {_AudioSettings.Master = _MasterSlider.value;});
            _BGMSlider.onValueChanged.AddListener(delegate {_AudioSettings.BGM = _BGMSlider.value;});
            _SkillUseSlider.onValueChanged.AddListener(delegate {_AudioSettings.SkillUseSFX = _SkillUseSlider.value;});
            _SkillHitSlider.onValueChanged.AddListener(delegate {_AudioSettings.SkillHitSFX = _SkillHitSlider.value;});
            _MobHitSlider.onValueChanged.AddListener(delegate {_AudioSettings.MobHitSFX = _MobHitSlider.value;});

            _MasterToggle.onValueChanged.AddListener(delegate {_AudioSettings.MuteMaster = !_MasterToggle.isOn;});
            _BGMToggle.onValueChanged.AddListener(delegate {_AudioSettings.MuteBGM = !_BGMToggle.isOn;});
            _SkillUseToggle.onValueChanged.AddListener(delegate {_AudioSettings.MuteSkillUseSFX = !_SkillUseToggle.isOn;});
            _SkillHitToggle.onValueChanged.AddListener(delegate {_AudioSettings.MuteSkillHitSFX = !_SkillHitToggle.isOn;});
            _MobHitToggle.onValueChanged.AddListener(delegate {_AudioSettings.MuteMobHitSFX = !_MobHitToggle.isOn;});
        }
#endregion
    }
}
