using UnityEngine;
using UnityEngine.UI;
using MapSystem;

namespace UISystem
{
    public class StageUI : MonoBehaviour
    {
        // Members
        [SerializeField] private Text _TextLevel, _TextCount;

#region Unity Functions
        private void Start ()
        {
            if (StageManager.Instance != null)
                StageManager.Instance.OnStageUpdate += UpdateText;
        }
#endregion
#region Private Functions
        private void UpdateText (object sender, OnStageUpdateArgs e)
        {
            _TextLevel.text = string.Concat("Stage: " + e.Level);
            _TextCount.text = string.Concat(e.KillCount + " / " + e.KillRequirement);
        }
#endregion
    }
}