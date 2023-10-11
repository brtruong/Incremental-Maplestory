using UnityEngine;
using AudioSystem;

namespace UISystem
{
    public class TitleUI : MonoBehaviour
    {
        [SerializeField] private AudioClip _BGM;
        private void OnEnable()
        {
            AudioManager.Instance?.PlayAudio(_BGM);    
        }    
    }    
}