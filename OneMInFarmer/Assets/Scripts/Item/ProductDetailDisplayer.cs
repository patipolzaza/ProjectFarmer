using UnityEngine;

public class ProductDetailDisplayer : WindowUIBase
{
    public static ProductDetailDisplayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}
