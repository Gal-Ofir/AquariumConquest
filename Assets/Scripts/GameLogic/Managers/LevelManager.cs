using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    public List<Transform> EnemyFishPrefabList;
    public List<Transform> ObstaclePrefabList;

    public Transform GodPowerUp;
    public Transform SlowTimePowerUp;

    public TutorialLevelUnityEvent TutorialLevelEvent;
    public UnityEvent LevelWon;

    private List<LevelConfiguration> _levelConfigurations;
    private GameObject _player;
    private Coroutine _spawnEnemiesCoroutine;
    private Coroutine _spawnPowerUpsCoroutine;
    private float _score;
    private int _currentLevel;

    private static LevelManager _instance;

    public float FishEnemySpeedModifier { get; private set; }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        FishEnemySpeedModifier = 1.0f;
    }

    private void Start()
    {
        // init configurations
        // in the future this will be loaded from JSON
        _levelConfigurations = new List<LevelConfiguration>()
        {
            new LevelConfiguration()
            {
                LevelId = 0,
                ScoreTarget = GameConstants.TUTORIAL_LEVEL_SCORE_TARGET,
                EnemySpawnRateMs = 1000,
                MaxEnemySpeed = 0.15f,
                MinEnemySpeed = 0.02f,
                PlayerGrowthRate = 0.4f,
                PowerUpList = new Dictionary<PowerUpType, int>()
                {
                    [PowerUpType.God] = 1,
                    [PowerUpType.SlowTime] = 1
                },
                ProbabilityOfSmallerEnemyFishSpawned = 0.6f
            },
            new LevelConfiguration()
            {
                LevelId = 1,
                ScoreTarget = 6f,
                EnemySpawnRateMs = 900,
                MaxEnemySpeed = 0.15f,
                MinEnemySpeed = 0.02f,
                PlayerGrowthRate = 0.4f,
                PowerUpList = new Dictionary<PowerUpType, int>()
                {
                    [PowerUpType.God] = 1,
                    [PowerUpType.SlowTime] = 1
                },
                ProbabilityOfSmallerEnemyFishSpawned = 0.8f,
                ObstacleConfigurations = new List<ObstacleConfiguration>()
                {
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(0, -12.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.zero
                    }
                }
            },
            new LevelConfiguration()
            {
                LevelId = 2,
                ScoreTarget = 12f,
                EnemySpawnRateMs = 900,
                MaxEnemySpeed = 0.18f,
                MinEnemySpeed = 0.1f,
                PlayerGrowthRate = 0.3f,
                PowerUpList = new Dictionary<PowerUpType, int>()
                {
                    [PowerUpType.God] = 3,
                    [PowerUpType.SlowTime] = 3
                },
                ProbabilityOfSmallerEnemyFishSpawned = 0.6f,
                ObstacleConfigurations = new List<ObstacleConfiguration>()
                {
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(8f, -14.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.zero
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_HORIZONTAL,
                        Position = new Vector3(-27f, 1.5f, 2),
                        Rotation = Vector3.zero
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_HORIZONTAL,
                        Position = new Vector3(27f, 1.5f, 2),
                        Rotation = Vector3.zero
                    }
                }
            },
            new LevelConfiguration()
            {
                LevelId = 3,
                ScoreTarget = 18f,
                EnemySpawnRateMs = 800,
                MaxEnemySpeed = 0.2f,
                MinEnemySpeed = 0.12f,
                PlayerGrowthRate = 0.2f,
                PowerUpList = new Dictionary<PowerUpType, int>()
                {
                    [PowerUpType.God] = 5,
                    [PowerUpType.SlowTime] = 4
                },
                ProbabilityOfSmallerEnemyFishSpawned = 0.6f,
                ObstacleConfigurations = new List<ObstacleConfiguration>()
                {
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(25, -12.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.zero
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(-25, -12.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.zero
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(-25f, 12.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.right * 180f
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(25f, 12.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.right * 180f
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_HORIZONTAL,
                        Position = new Vector3(-8f, -9.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.zero
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_HORIZONTAL,
                        Position = new Vector3(10.5f, 5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.forward * 90f
                    }
                }
            },
            new LevelConfiguration()
            {
                LevelId = 4,
                ScoreTarget = 40f,
                EnemySpawnRateMs = 800,
                MaxEnemySpeed = 0.3f,
                MinEnemySpeed = 0.2f,
                PlayerGrowthRate = 0.15f,
                PowerUpList = new Dictionary<PowerUpType, int>()
                {
                    [PowerUpType.God] = 8,
                    [PowerUpType.SlowTime] = 8
                },
                ProbabilityOfSmallerEnemyFishSpawned = 0.4f,
                ObstacleConfigurations = new List<ObstacleConfiguration>()
                {
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(2.7f, -12.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.forward * -27f
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(16.2f, -10f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.forward * -20.2f
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(-21.5f, -2.2f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.right * 180f + (Vector3.forward * -182.75f)
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_T_SHAPE,
                        Position = new Vector3(-25f, -3.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.right * 180f
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_HORIZONTAL,
                        Position = new Vector3(17f, -11.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.forward * 25.8f
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_HORIZONTAL,
                        Position = new Vector3(19.5f, 4.5f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.forward * 326.3f
                    },
                    new ObstacleConfiguration()
                    {
                        Obstacle = GameConstants.OBSTACLE_TYPE_HORIZONTAL,
                        Position = new Vector3(-26.7f, 14f, GameConstants.FISH_Z_VALUE),
                        Rotation = Vector3.forward * 351.8f
                    }
                }
            }
        };

        // sort by LevelId
        _levelConfigurations.Sort(new LevelConfigurationComparer());
        _score = 0f;
        _currentLevel = GameManager.GetInstance().GetActiveLevel();
    }

    /// <summary>
    /// Gets the current instance of the level manager
    /// </summary>
    public static LevelManager GetInstance()
    {
        return _instance;
    }

    /// <summary>
    /// Gets the current level configurations
    /// </summary>
    public LevelConfiguration GetCurrentLevelConfigurations()
    {
        return _levelConfigurations[_currentLevel];
    }

    /// <summary>
    /// Handle tutorial level events, according to which event happened previously
    /// </summary>
    public void TutorialLevelEventHandler(TutorialLevelState tutorialLevelState)
    {
        switch (tutorialLevelState)
        {
            case TutorialLevelState.SmallFishEaten:
                TutorialLevelEvent.Invoke(TutorialLevelState.BigFishSpawned);
                SpawnEnemyFish(0.1f, false, GameConstants.TUTORIAL_LEVEL_BIG_FISH_SCALE);
                SpawnEnemyFish(0.13f, true, GameConstants.TUTORIAL_LEVEL_MEDIUM_FISH_1_SCALE);
                break;
            case TutorialLevelState.MediumFish1Eaten:
                SpawnEnemyFish(0.1f, false, GameConstants.TUTORIAL_LEVEL_MEDIUM_FISH_2_SCALE);
                SpawnPowerUp(PowerUpType.God);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Handles fish eaten by player event - adds to current score
    /// </summary>
    public void OnFishEaten(float scoreToAdd)
    {
        var tutorialLevelSmallFishScoreValue = GameConstants.TUTORIAL_LEVEL_SMALL_FISH_SCALE.x;
        var tutorialLevelMediumFish1ScoreValue = GameConstants.TUTORIAL_LEVEL_MEDIUM_FISH_1_SCALE.x;
        var targetScore = GetCurrentLevelConfigurations().ScoreTarget;

        _score += scoreToAdd;

        if (_score >= targetScore)
        {
            HandleLevelWon();
        }

        // Tutorial specific logic
        if (_currentLevel == 0 && scoreToAdd == tutorialLevelSmallFishScoreValue)
        {
            TutorialLevelEvent.Invoke(TutorialLevelState.SmallFishEaten);
        }
        else if (_currentLevel == 0 && scoreToAdd == tutorialLevelMediumFish1ScoreValue)
        {
            TutorialLevelEvent.Invoke(TutorialLevelState.MediumFish1Eaten);
        }
    }

    /// <summary>
    /// Handles level start event
    /// </summary>
    [ContextMenu(nameof(OnLevelStarted))]
    public void OnLevelStarted()
    {
        // Reset level - destroy lingering resources and reset positions
        ResetLevel();

        _currentLevel = GameManager.GetInstance().GetActiveLevel();

        // specific logic for tutorial level
        var levelConfig = GetCurrentLevelConfigurations();

        if (levelConfig.LevelId == 0)
        {
            StartTutorialLevel();
        }
        else
        {
            // Start spawning enemies and powerups
            _spawnEnemiesCoroutine = StartCoroutine(nameof(SpawnEnemies));
            _spawnPowerUpsCoroutine = StartCoroutine(nameof(SpawnPowerUps));
        }

        // setup obstacles
        SetupObstacles();
    }

    /// <summary>
    /// Spawns power ups randomly according to level configuration
    /// </summary>
    public IEnumerator SpawnPowerUps()
    {
        var levelConfig = GetCurrentLevelConfigurations();
        var powerUpList = levelConfig.PowerUpList;

        var totalPowerUpsToSpawn = powerUpList.Values.Sum();

        Debug.Log("Started spawning power ups");

        for (int i = 0; i < totalPowerUpsToSpawn; i++)
        {
            // wait some sensible time between each power up spawn
            var waitTime = Random.Range(10f, 30f);
            Debug.Log($"Next power up in {waitTime} seconds ");

            yield return new WaitForSeconds(waitTime);

            // get which power up to spawn:
            // filter to fetch power ups still left to spawn
            var powerUpsToChooseFrom = powerUpList.Keys.Where(puType => 
            {
                return powerUpList[puType] > 0;
            });

            // choose one randomly
            var randomElement = Random.Range(0, powerUpsToChooseFrom.Count()); ;
            
            var powerUpType = powerUpsToChooseFrom.ElementAt(randomElement);

            SpawnPowerUp(powerUpType);
            Debug.Log($"PowerUp {powerUpType} spawned");

            // decrement one powerUp
            powerUpList[powerUpType]--;
        }
    }

    /// <summary>
    /// Spawn power up of given type <see cref="PowerUpType"/>
    /// </summary>
    public void SpawnPowerUp(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.God:
                Instantiate(GodPowerUp);
                break;

            case PowerUpType.SlowTime:
                Instantiate(SlowTimePowerUp);
                break;
        }
    }

    /// <summary>
    /// Debugging method: Spawn god powerup
    /// </summary>
    [ContextMenu(nameof(SpawnGodPowerUpTest))]
    public void SpawnGodPowerUpTest()
    {
        SpawnPowerUp(PowerUpType.God);
    }

    /// <summary>
    /// Debugging method: Spawn slow-time powerup
    /// </summary>
    [ContextMenu(nameof(SpawnSlowTimePowerUpTest))]
    public void SpawnSlowTimePowerUpTest()
    {
        SpawnPowerUp(PowerUpType.SlowTime);
    }

    /// <summary>
    /// Handles power up duration finished event
    /// </summary>
    public void OnPowerUpDurationFinished()
    {
        FishEnemySpeedModifier = 1.0f;
    }

    /// <summary>
    /// Handles level won
    /// </summary>
    private void HandleLevelWon()
    {
        GameManager.GetInstance().UpdateLevelPassed(_currentLevel + 1);

        LevelWon.Invoke();
    }

    /// <summary>
    /// Setup obstacles in the level according to level config
    /// </summary>
    private void SetupObstacles()
    {
        var levelConfig = GetCurrentLevelConfigurations();

        if (levelConfig.ObstacleConfigurations != null)
        {
            levelConfig.ObstacleConfigurations.ForEach(obstacleConfiguration => 
            {
                var obstacle = Instantiate(ObstaclePrefabList[obstacleConfiguration.Obstacle]);

                obstacle.transform.position = obstacleConfiguration.Position;
                obstacle.transform.eulerAngles = obstacleConfiguration.Rotation;
            });
        }
    }

    /// <summary>
    /// Handle game over event - stop spawning enemy fish and power ups
    /// </summary>
    public void OnGameOver()
    {
        StopEnemyFishSpawning();
        StopPowerupsSpawning();
    }

    /// <summary>
    /// Handles power up picked up event
    /// </summary>
    public void OnPowerUpPickedUp(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.SlowTime:
                FishEnemySpeedModifier = 0.3f;
                break;
        }
    }

    /// <summary>
    /// Stop spawning enemy fish
    /// </summary>
    private void StopEnemyFishSpawning()
    {
        // stop spawning enemy fish from previous level (if any)
        if (_spawnEnemiesCoroutine != null)
        {
            StopCoroutine(_spawnEnemiesCoroutine);
        }
    }

    /// <summary>
    /// Stop spawning power ups
    /// </summary>
    private void StopPowerupsSpawning()
    {
        // stop spawning powerups from previous level (if any)
        if (_spawnPowerUpsCoroutine != null)
        {
            StopCoroutine(_spawnPowerUpsCoroutine);
        }
    }

    /// <summary>
    /// Reset level to initial settings
    /// </summary>
    private void ResetLevel()
    {
        _score = 0f;

        StopEnemyFishSpawning();
        StopPowerupsSpawning();

        // reset player position
        _player.transform.position = new Vector3(0, 0, GameConstants.FISH_Z_VALUE);
        _player.transform.localScale = GameConstants.PLAYER_STARTING_SCALE;
        _player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        // find all enemy fish game objects and destroy them
        var enemyFish = GameObject.FindGameObjectsWithTag("EnemyFish").ToList();

        DestroyAll(enemyFish);

        // find all obstacle objects and destroy them
        var obstacles = GameObject.FindGameObjectsWithTag("Obstacle").ToList();

        DestroyAll(obstacles);

        // find all active powerups and destroy them
        var powerUps = GameObjectExtensions.FindAllIngameObjectsWithPrefix("PowerUp").ToList();

        DestroyAll(powerUps);

        // rest fish speed modifier
        FishEnemySpeedModifier = 1.0f;
    }

    /// <summary>
    /// Start the specific tutorial level logic and events
    /// </summary>
    private void StartTutorialLevel()
    {
        TutorialLevelEvent.Invoke(TutorialLevelState.Started);
        SpawnEnemyFish(0.1f, true, GameConstants.TUTORIAL_LEVEL_SMALL_FISH_SCALE);
    }

    /// <summary>
    /// Continuously spawn enemies
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            var levelConfig = GetCurrentLevelConfigurations();
            var enemySpeed = Random.Range(levelConfig.MinEnemySpeed, levelConfig.MaxEnemySpeed);
            var spawnFromLeft = Random.value >= 0.5;
            var enemyFishScale = GetEnemyFishScale();

            SpawnEnemyFish(enemySpeed, spawnFromLeft, enemyFishScale);

            yield return new WaitForSeconds(levelConfig.EnemySpawnRateMs / 1000);
        }
    }

    /// <summary>
    /// Spawn an enemy fish that swims at given speed, position and at given scale
    /// </summary>
    private void SpawnEnemyFish(float enemySpeed, bool spawnFromLeft, Vector3 enemyFishScale)
    {
        var enemyFishPrefabIndex = Random.Range(0, EnemyFishPrefabList.Count);
        var enemyFishPrefab = EnemyFishPrefabList[enemyFishPrefabIndex];

        var enemyFish = Instantiate(enemyFishPrefab);
        var enemyBehavior = enemyFish.gameObject.GetComponent<EnemyBehavior>();

        enemyFish.gameObject.transform.localScale = enemyFishScale;
        enemyBehavior.EnemySpeed = enemySpeed;
        enemyBehavior.SpawnFromLeft = spawnFromLeft;
    }

    /// <summary>
    /// Returns a random enemy fish scale, according to level config
    /// </summary>
    /// <returns></returns>
    private Vector3 GetEnemyFishScale()
    {
        var probabilityOfSmallerFish = GetCurrentLevelConfigurations().ProbabilityOfSmallerEnemyFishSpawned;

        var scaleShouldBeSmallerThanPlayerScale = Random.value <= probabilityOfSmallerFish;

        var zValue = GameConstants.FISH_Z_VALUE;
        float maxScaleValue;
        float minScaleValue;

        if (scaleShouldBeSmallerThanPlayerScale)
        {
            minScaleValue = GameConstants.SPAWNED_FISH_MIN_SCALE;
            maxScaleValue = _player.gameObject.transform.localScale.x - 0.0001f;
        }
        else
        {
            minScaleValue = _player.gameObject.transform.localScale.x + 0.0001f;
            maxScaleValue = GameConstants.SPAWNED_FISH_MAX_SCALE;
        }

        var fishScale = Random.Range(minScaleValue, maxScaleValue);

        return new Vector3(fishScale, fishScale, zValue);
    }

    /// <summary>
    /// Destroy all game objects
    /// </summary>
    private void DestroyAll(List<GameObject> gameObjects)
    {
        gameObjects.ForEach(go => Destroy(go));
    }
}
