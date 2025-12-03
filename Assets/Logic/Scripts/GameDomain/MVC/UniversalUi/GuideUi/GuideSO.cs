using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Guide", menuName = "Guide System/Guide", order = 1)]
public class GuideSO : ScriptableObject {
    public string guideTitle = "New Guide Title";
    [SerializeField] private List<Page> pages = new List<Page>();
    public IReadOnlyList<Page> Pages => pages;
}