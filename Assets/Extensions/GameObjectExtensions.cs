using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// An 'extension' to be able to find all ingame (active or inactive) objects that start with the given prefix
    /// </summary>
    /// <param name="prefix">tag prefix</param>
    /// <returns>Collection of GameObjects</returns>
    public static IEnumerable<GameObject> FindAllIngameObjectsWithPrefix(string prefix)
    {
        var allActiveGameObjects = (Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]);

        return allActiveGameObjects.Where(gameObject => gameObject != null &&
                                                        !string.IsNullOrWhiteSpace(gameObject.tag) &&
                                                        gameObject.tag.StartsWith(prefix) &&
                                                        gameObject.tag.Contains("Ingame"));
    }
}

