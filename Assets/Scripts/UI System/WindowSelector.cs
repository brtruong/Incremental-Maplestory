using UnityEngine;

namespace UISystem
{
    public class WindowSelector : MonoBehaviour
    {
        public static void Select (Transform window)
        {
            window.SetSiblingIndex(-1);
        } 
    }
}