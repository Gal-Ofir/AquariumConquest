using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager script that handles the layer above <see cref="LevelManager"/> logic
/// </summary>
public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public List<Transform> BackgroundPrefabList;
    public GameObject FishTank;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }

        PlayerPrefs.SetInt("ActiveLevel", 0);
    }

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }

    /// <summary>
    /// Get the current instance of the game manager
    /// </summary>
    public static GameManager GetInstance()
    {
        return _instance;
    }

    /// <summary>
    /// Get what is the highest level that the player passed
    /// </summary>
    public int GetLevelPassed()
    {
        return PlayerPrefs.GetInt("LevelsPassed", 0);
    }

    /// <summary>
    /// Get the current active level
    /// </summary>
    public int GetActiveLevel()
    {
        return PlayerPrefs.GetInt("ActiveLevel", 0);
    }

    /// <summary>
    /// Update the highest level passed
    /// </summary>
    public void UpdateLevelPassed(int level)
    {
        PlayerPrefs.SetInt("LevelsPassed", Mathf.Max(GetLevelPassed(), level));
    }

    /// <summary>
    /// Set the current active level
    /// </summary>
    public void UpdateActiveLevel(int level)
    {
        PlayerPrefs.SetInt("ActiveLevel", level);
    }

    /// <summary>
    /// Handles level started event - switch backgrounds
    /// </summary>
    public void OnLevelStarted()
    {
        var activeLevel = GetActiveLevel();

        if (activeLevel < BackgroundPrefabList.Count)
        {
            var background = GameObject.FindGameObjectWithTag("Background");
            Destroy(background);

            var newBackground = Instantiate(BackgroundPrefabList[activeLevel]);

            // set as child of FishTank gameobject
            newBackground.transform.parent = FishTank.transform;
        }
    }
}
