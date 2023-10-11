using UnityEngine;
using UnityEngine.UI;
using MesoSystem;

namespace UISystem
{
    public class ResourceUI : MonoBehaviour
    {
        // Members
        [SerializeField] private Text _TextMeso;
        [SerializeField] private Text _TextClickDMG;

#region Unity Functions
        private void Start ()
        {
            MesoManager.Instance.OnMesoUpdate += UpdateMesoText; 
        }
#endregion
#region Private Functions
        private void UpdateMesoText (object sender, OnMesoUpdateArgs e)
        {
            _TextMeso.text = string.Concat("Meso: ", e.Meso);
        }
#endregion
    }
}