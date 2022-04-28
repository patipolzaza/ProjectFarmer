using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerSort : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private Dictionary<SpriteRenderer, int> spriteRenderersSortingOrderPairs = new Dictionary<SpriteRenderer, int>();

    [SerializeField] private float offsetY;

    private void Awake()
    {
        if (obj.GetComponent<SpriteRenderer>())
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            int defaultLayerOrder = sr.sortingOrder;

            spriteRenderersSortingOrderPairs.Add(sr, defaultLayerOrder);
        }
        else if (obj.GetComponent<SpriteRenderers>())
        {
            SpriteRenderers srs = obj.GetComponent<SpriteRenderers>();
            var spriteRenderers = srs.GetSpriteRenderers;
            foreach (var sr in spriteRenderers)
            {
                int defaultLayerOrder = sr.sortingOrder;
                spriteRenderersSortingOrderPairs.Add(sr, defaultLayerOrder);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateLayerSort();
    }

    private void UpdateLayerSort()
    {
        float positionY = transform.position.y;

        foreach (var keyValuePair in spriteRenderersSortingOrderPairs)
        {
            SpriteRenderer sr = keyValuePair.Key;
            int defaultSortingOrder = keyValuePair.Value;

            int newSortingOrder = defaultSortingOrder + (-(Mathf.RoundToInt((positionY - offsetY) * 100f)));
            sr.sortingOrder = newSortingOrder;
        }
    }
}
