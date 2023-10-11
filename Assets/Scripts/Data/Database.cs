using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using InventorySystem;
using CharacterSystem;
using SkillSystem;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Database")]
public class Database : ScriptableObject, ISerializationCallbackReceiver
{
    [SerializeField] private BaseItem[] _BaseItems;
    public Dictionary<BaseItem, int> GetBaseItemID = new Dictionary<BaseItem, int>();
    public Dictionary<int, BaseItem> GetBaseItem = new Dictionary<int, BaseItem>();

    [SerializeField] private CharacterBase[] _CharacterBases;
    public Dictionary<CharacterBase, int> GetCharacterBaseID = new Dictionary<CharacterBase, int>();
    public Dictionary<int, CharacterBase> GetCharacterBase = new Dictionary<int, CharacterBase>();
    
    [SerializeField] private SkillBase[] _SkillBases;
    public Dictionary<SkillBase, int> GetSkillBaseID = new Dictionary<SkillBase, int>();
    public Dictionary<int, SkillBase> GetSkillBase = new Dictionary<int, SkillBase>();

    [ContextMenu("Update")]
    public void OnAfterDeserialize ()
    {
        GetBaseItemID = new Dictionary<BaseItem, int>();
        GetBaseItem = new Dictionary<int, BaseItem>();

        GetCharacterBaseID = new Dictionary<CharacterBase, int>();
        GetCharacterBase = new Dictionary<int, CharacterBase>();

        GetSkillBaseID = new Dictionary<SkillBase, int>();
        GetSkillBase = new Dictionary<int, SkillBase>();

        GetBaseItem.Add(0, null);

        for (int i = 1; i < _BaseItems.Length; i++)
        {
            _BaseItems[i].ChangeID(i);
            GetBaseItemID.Add(_BaseItems[i], i);
            GetBaseItem.Add(i, _BaseItems[i]);
        }

        for (int i = 0; i < _CharacterBases.Length; i++)
        {
            _CharacterBases[i].ChangeID(i);
            GetCharacterBaseID.Add(_CharacterBases[i], i);
            GetCharacterBase.Add(i, _CharacterBases[i]);
        }

        for (int i = 0; i < _SkillBases.Length; i++)
        {
            _SkillBases[i].ChangeID(i);
            GetSkillBaseID.Add(_SkillBases[i], i);
            GetSkillBase.Add(i, _SkillBases[i]);
        }
    }

    public void OnBeforeSerialize (){}
}