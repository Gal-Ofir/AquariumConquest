using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameObjectExtensions
{
    /// <summary>
    /// An 'extension' to be able to find all ingame (active or inactive) objects that their tags start with the given prefix
    /// </summary>
    /// <param name="tagPrefix">tag prefix</param>
    /// <returns>Collection of GameObjects</returns>
    public static IEnumerable<GameObject> FindAllIngameObjectsWithTagPrefix(string tagPrefix)
    {
        var allActiveGameObjects = (Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[]);

        return allActiveGameObjects.Where(gameObject => gameObject != null &&
                                                        !string.IsNullOrWhiteSpace(gameObject.tag) &&
                                                        gameObject.tag.StartsWith(tagPrefix) &&
                                                        gameObject.tag.Contains("Ingame"));
    }
}

