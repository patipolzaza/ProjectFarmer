using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerSort : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> _spriteRenderers = new List<SpriteRenderer>();
    [SerializeField] private float offsetY;

    // Update is called once per frame
    void Update()
    {
        UpdateLayerSort();
    }

    private void UpdateLayerSort()
    {
        float positionY = transform.position.y;

        foreach (var sr in _spriteRenderers)
        {
            sr.sortingOrder = -Mathf.RoundToInt((positionY - offsetY) * 100f);
        }
    }
}
