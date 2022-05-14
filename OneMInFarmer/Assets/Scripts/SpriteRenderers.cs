using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRenderers : MonoBehaviour
{
    [SerializeField] private SpriteRenderer[] _spriteRenderers;
    [SerializeField] private Color _defaultColor = new Color32(255, 255, 255, 255);

    public SpriteRenderer[] GetSpriteRenderers => _spriteRenderers;
    public Color GetDefaultColor => _defaultColor;
}
