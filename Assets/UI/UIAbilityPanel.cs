using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAbilityPanel : MonoBehaviour
{
    [System.Serializable]
    public class AbilitySlot
    {
        public string abilityName = "Habilidade";
        public int cost = 3;
        public Button button;
        public TextMeshProUGUI label;
        public Image usedOverlay; 
        [HideInInspector] public bool used;
    }

    public AbilitySlot[] slots;

    public void Initialize(TurnoTatico turno)
    {
        if (slots == null) return;
        foreach (var slot in slots)
        {
            if (slot == null) continue;

            if (slot.label != null)
            {
                slot.label.text = $"{slot.abilityName} ({slot.cost})";
            }

            if (slot.button != null)
            {
                var captured = slot; 
                slot.button.onClick.RemoveAllListeners();
                slot.button.onClick.AddListener(() => TryUse(turno, captured));
            }

            UpdateUsedVisual(slot);
        }
    }

    private void TryUse(TurnoTatico turno, AbilitySlot slot)
    {
        if (turno == null || slot == null) return;
        if (slot.used) return;

        var ok = turno.UsarHabilidade(slot.cost);
        if (ok)
        {
            slot.used = true;
            UpdateUsedVisual(slot);
        }
    }

    public void ResetAll()
    {
        if (slots == null) return;
        foreach (var slot in slots)
        {
            if (slot == null) continue;
            slot.used = false;
            UpdateUsedVisual(slot);
        }
    }

    private void UpdateUsedVisual(AbilitySlot slot)
    {
        if (slot.usedOverlay != null)
        {
            slot.usedOverlay.enabled = slot.used;
        }
        if (slot.button != null)
        {
            slot.button.interactable = !slot.used;
        }
    }
}


