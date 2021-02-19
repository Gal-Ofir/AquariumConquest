using UnityEngine;

/// <summary>
/// EnemyBehavior script
/// </summary>
public class EnemyBehavior : FishBehaviorBase
{
    public float EnemySpeed;
    public bool SpawnFromLeft;

    private void Awake()
    {
        // default values
        EnemySpeed = 0;
        SpawnFromLeft = true;
    }

    /// <inheritdoc/>
    public override void HandleMovement()
    {
        // move fish according to the position spawned (left or right)
        var xDirection = SpawnFromLeft ? 1 : -1;
        var enemyFishSpeedModifier = LevelManager.GetInstance().FishEnemySpeedModifier;
        _rigidBody.transform.position += new Vector3(xDirection * EnemySpeed * enemyFishSpeedModifier, 0, 0);

        DestroyIfNeeded();
    }

    public override void Start()
    {
        base.Start();

        Vector3 startPosition = Vector3.forward * 2;

        startPosition.x = SpawnFromLeft ? GameConstants.GAME_X_LEFT_BORDER : GameConstants.GAME_X_RIGHT_BORDER;
        startPosition.y = Random.Range(GameConstants.GAME_Y_LOWER_BORDER, GameConstants.GAME_Y_UPPER_BORDER);

        gameObject.transform.position = startPosition;

        gameObject.transform.eulerAngles = SpawnFromLeft ? Vector3.zero : new Vector3(0f, 180f, 0f);
    }

    /// <summary>
    /// Destroy the enemy fish gameObject when we reach the edge of the level
    /// </summary>
    private void DestroyIfNeeded()
    {
        // if we reach an edge
        if ((SpawnFromLeft && _rigidBody.transform.position.x >= GameConstants.GAME_X_RIGHT_BORDER + 1f) ||
           (!SpawnFromLeft && _rigidBody.transform.position.x <= GameConstants.GAME_X_LEFT_BORDER -1f))
        {
            // tutorial level specific logic: 'respawn' 
            if (LevelManager.GetInstance().GetCurrentLevelConfigurations().LevelId == 0)
            {
                Vector3 startPosition = Vector3.forward * 2;

                startPosition.x = SpawnFromLeft ? GameConstants.GAME_X_LEFT_BORDER : GameConstants.GAME_X_RIGHT_BORDER;
                startPosition.y = Random.Range(GameConstants.GAME_Y_LOWER_BORDER, GameConstants.GAME_Y_UPPER_BORDER);

                gameObject.transform.position = startPosition;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
