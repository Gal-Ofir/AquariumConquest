using System;
using System.Collections.Generic;

/// <summary>
/// Defines the level configuration data object model
/// </summary>
[Serializable]
public class LevelConfiguration
{
    public int LevelId { get; set; }

    public float EnemySpawnRateMs { get; set; }

    public float MinEnemySpeed { get; set; }

    public float MaxEnemySpeed { get; set; }

    public float ProbabilityOfSmallerEnemyFishSpawned { get; set; }

    public float ScoreTarget { get; set; }

    public float PlayerGrowthRate { get; set; }

    public Dictionary<PowerUpType, int> PowerUpList { get; set; }

    public List<ObstacleConfiguration> ObstacleConfigurations { get; set; }
}

