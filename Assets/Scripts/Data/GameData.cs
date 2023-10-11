using System.Collections.Generic;
using UnityEngine;

using CharacterSystem;
using InventorySystem;
using MapSystem;
using MesoSystem;

namespace CoreSystems
{
    [System.Serializable]
    public class GameData
    {
        [SerializeField] private Meso _Meso;
        [SerializeField] private Stage _Stage;
        [SerializeField] private List<Character> _Characters;

        public Stage Stage => _Stage;
        public Meso Meso => _Meso;
        public List<Character> Characters => _Characters;

        public GameData ()
        {
            _Meso = new Meso(GameSettings.MesoStartAmount);
            _Stage = new Stage();
            _Characters = new List<Character>();
        }
        
        public void SaveCharacters ()
        {
            foreach (Character c in _Characters)
                c.SavePosition();
        }
    }
}