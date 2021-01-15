using UnityEngine;

public class PowerUpBehavior : MonoBehaviour
{
    private void Awake()
    {
        var xInitialPosition = Random.Range(GameConstants.POWERUP_SPAWN_MIN_X_POS, GameConstants.POWERUP_SPAWN_MAX_X_POS);
        var yInitialPosition = GameConstants.POWERUP_SPAWN_Y_POS;

        gameObject.transform.position = new Vector3(xInitialPosition, yInitialPosition, GameConstants.FISH_Z_VALUE);

        gameObject.tag = gameObject.tag + ".Ingame";
    }

    private void LateUpdate()
    {
        // destroy powerup if it wasn't picked up
        if (gameObject.transform.position.y <= GameConstants.GAME_Y_LOWER_BORDER - 0.3f)
        {
            Destroy(gameObject);
        }
        // otherwise continue falling - using level fish speed to increase difficulty picking up power ups as level increases
        else
        {
            var speed = LevelManager.GetInstance().GetCurrentLevelConfigurations().MaxEnemySpeed * 0.75f;
            var currentPosition = gameObject.transform.position;
            
            gameObject.transform.position = new Vector3(currentPosition.x, currentPosition.y - (speed * Time.deltaTime * 20f), GameConstants.FISH_Z_VALUE);
        }
    }
}
