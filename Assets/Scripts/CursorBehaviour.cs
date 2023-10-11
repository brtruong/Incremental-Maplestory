using UnityEngine;
using UnityEngine.UI;

using CoreSystems;
using CharacterSystem;
using DamageSystem;
using MesoSystem;

public class CursorBehaviour : MonoBehaviour
{

    [SerializeField] private int clickDMG;

    private void Update ()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 10f, GameSettings.InteractableLayers);

        if (hit.collider == null) return;

        if (hit.collider.tag == "Meso")
        {
            hit.transform.GetComponent<MesoBehaviour>().CollectMeso();
        }

        if (Input.GetMouseButtonDown(0) && hit.collider.tag == "Enemy")
        {
            DamageLines d = new DamageLines();
            d.Add(clickDMG, false);
            hit.transform.GetComponentInParent<IDamageable>().Damage(d, "No Effect");
        }

        if (Input.GetMouseButtonDown(1) && hit.collider.tag == "Character")
        {
            GameObject obj = hit.transform.gameObject;
            if (CharacterManager.Instance != null)
                CharacterManager.Instance.SetActiveCharacter(obj);
        }
    }
}