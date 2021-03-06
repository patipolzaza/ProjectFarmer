using System;

public interface IUsable
{
    /// <summary>
    /// Use item to interactable target
    /// </summary>
    /// <param name="targetToUse">Target to use this usable.</param>
    /// <returns>Boolean that is true if it out</returns>
    public bool Use(Interactable targetToUse);
    public void AddTargetType(Type targetType);
}
