using UnityEngine;

namespace DamageSystem
{
    [CreateAssetMenu(fileName = "DamageSkin", menuName = "Incremental Maplestory/DamageSkin")]
    public class DamageSkin : ScriptableObject
    {
        [SerializeField] private GameObject[] _Numbers, _NumbersCrit;
        [SerializeField] private Sprite[] _Units;
        [SerializeField] private Sprite[] _Status;

        private float[] _NumberSizes;
        private float[] _NumberCritSizes;

        public (GameObject, float) GetNumber (int number, bool crit)   
        {
            switch (number)
            {
                case 0:
                    if (crit) return (_NumbersCrit[0], _NumberCritSizes[0]);
                    else return (_Numbers[0], _NumberSizes[0]);
                
                case 1:
                    if (crit) return (_NumbersCrit[1], _NumberCritSizes[1]);
                    else return (_Numbers[1], _NumberSizes[1]);

                case 2:
                    if (crit) return (_NumbersCrit[2], _NumberCritSizes[2]);
                    else return (_Numbers[2], _NumberSizes[2]);

                case 3:
                    if (crit) return (_NumbersCrit[3], _NumberCritSizes[3]);
                    else return (_Numbers[3], _NumberSizes[3]);

                case 4:
                    if (crit) return (_NumbersCrit[4], _NumberCritSizes[4]);
                    else return (_Numbers[4], _NumberSizes[4]);

                case 5:
                    if (crit) return (_NumbersCrit[5], _NumberCritSizes[5]);
                    else return (_Numbers[5], _NumberSizes[5]);
                
                case 6:
                    if (crit) return (_NumbersCrit[6], _NumberCritSizes[6]);
                    else return (_Numbers[6], _NumberSizes[6]);
                
                case 7:
                    if (crit) return (_NumbersCrit[7], _NumberCritSizes[7]);
                    else return (_Numbers[7], _NumberSizes[7]);
                
                case 8:
                    if (crit) return (_NumbersCrit[8], _NumberCritSizes[8]);
                    else return (_Numbers[8], _NumberSizes[8]);
                
                case 9:
                    if (crit) return (_NumbersCrit[9], _NumberCritSizes[9]);
                    else return (_Numbers[9], _NumberSizes[9]);

                default:
                    return (null, -1);
            }
        }

        public (GameObject, float) GetCritEffect ()
        {
            return (_NumbersCrit[10], _NumberCritSizes[10]); 
        }

        [ContextMenu("Generate Number Sizes")]
        public void GenerateNumberSizes ()
        {
            _NumberSizes = new float[_Numbers.Length];

            for (int i = 0; i < _Numbers.Length; i++)
                _NumberSizes[i] = _Numbers[i].GetComponent<SpriteRenderer>().sprite.bounds.size.x;

            _NumberCritSizes = new float[_NumbersCrit.Length];
            for (int i = 0; i < _NumbersCrit.Length; i++)
                _NumberCritSizes[i] = _NumbersCrit[i].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }

        private void OnEnable ()
        {
            GenerateNumberSizes();
        }
    }
}