using UnityEngine;

using CharacterSystem;
using InventorySystem;
using SkillSystem;
public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance {get; private set;}
    [field:SerializeField] public Database Database {get; private set;}

    public static BaseItem GetBaseItem (int id) => Instance?.Database.GetBaseItem[id];
    public static CharacterBase GetCharacterBase (int id) => Instance?.Database.GetCharacterBase[id];
    public static SkillBase GetSkillBase (int id) => Instance?.Database.GetSkillBase[id];

    private void Awake ()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }
}
