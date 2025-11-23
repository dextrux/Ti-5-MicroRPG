using UnityEngine;
using System;

[Serializable]
public struct Page {
    public Sprite pageSprite;
    [TextArea(3, 10)]
    public string descriptionText;
}