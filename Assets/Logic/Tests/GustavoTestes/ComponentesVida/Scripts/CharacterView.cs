using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Text healthText;
    [SerializeField] private Text actionPointsText;
    [SerializeField] private Text invulnerabilityText;
    [SerializeField] private Transform statesContainer;
    [SerializeField] private GameObject statePrefab;

    public void UpdateHealth(int health)
    {
        healthText.text = $"Health: {health}";
    }

    public void UpdateActionPoints(int points)
    {
        actionPointsText.text = $"AP: {points}";
    }

    public void UpdateStates(List<IStatusEffect> states)
    {
        foreach (Transform child in statesContainer)
            Destroy(child.gameObject);

        foreach (var state in states)
        {
            var obj = Instantiate(statePrefab, statesContainer);
            obj.GetComponent<Text>().text = state.Name;
        }
    }

    public void UpdateInvulnerability(bool isInvulnerable)
    {
        invulnerabilityText.text = isInvulnerable ? "Invulnerable" : "";
    }
}
