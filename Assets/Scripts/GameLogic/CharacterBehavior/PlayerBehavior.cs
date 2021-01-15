using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Player fish behavior
/// </summary>
public class PlayerBehavior : FishBehaviorBase
{
    public FixedJoystick Joystick;

    public FishEatenEvent FishEaten;
    public UnityEvent GameOver;
    public PowerUpPickedUpEvent PowerUpPickedUpEvent;

    private float _previousXPosition;
    private bool _godPowerUpActive;

    public override void Start()
    {
        base.Start();

        _godPowerUpActive = false;
        _previousXPosition = gameObject.transform.position.x;
    }

    /// <inheritdoc/>
    public override void HandleMovement()
    {
        var joyStickDirection = Vector3.up * Joystick.Vertical + Vector3.right * Joystick.Horizontal;

        // add force
        AddForceToRigidBody(joyStickDirection);

        // clamp position so player won't pass upper and lower bounds
        ClampPositionIfNeeded();

        var currentXPosition = gameObject.transform.position.x;

        // flip sprites if player changed direction of movement
        FlipSpriteIfNeeded(currentXPosition);

        // wrap x position if player passed left or right border
        WrapPositionIfNeeded(currentXPosition);

        _previousXPosition = _rigidBody.transform.position.x;
    }

    /// <summary>
    /// Handles powerup picked up event
    /// </summary>
    public void OnPowerUpPickedUp(PowerUpType powerUpType)
    {
        if (powerUpType == PowerUpType.God)
        {
            _godPowerUpActive = true;
        }
    }

    /// <summary>
    /// Handles power up duration finished
    /// </summary>
    public void OnPowerUpDurationFinished()
    {
        _godPowerUpActive = false;
        _spriteRenderer.color = Color.white;
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        if (_godPowerUpActive)
        {
            _spriteRenderer.color = Color.Lerp(Color.white, Color.yellow, _fishSpriteSwapTimeInterval / GameConstants.FISH_SPRITE_SWAP_TIME_INTERVAL);
        }
    }

    /// <summary>
    /// Wraps x position if needed
    /// </summary>
    private void WrapPositionIfNeeded(float currentXPosition)
    {
        // Wrap sprite around if player dragged past the left or right border
        var newPosition = _rigidBody.transform.position;

        if (currentXPosition >= GameConstants.GAME_X_RIGHT_BORDER - 0.2f)
        {
            newPosition.x = GameConstants.GAME_X_LEFT_BORDER + 0.5f;
        }
        else if (currentXPosition <= GameConstants.GAME_X_LEFT_BORDER + 0.2f)
        {
            newPosition.x = GameConstants.GAME_X_RIGHT_BORDER - 0.5f;
        }

        if (newPosition != _spriteRenderer.transform.position)
        {
            _rigidBody.transform.position = newPosition;
        }
    }

    /// <summary>
    /// Flips gameObject sprite if needed (when player changed direction of movement for instance)
    /// </summary>
    private void FlipSpriteIfNeeded(float currentXPosition)
    {
        // Flip rotation when
        //  sprite is facing right and the movement was to the left
        // OR
        //  sprite is facing left and the movement was to the right

        var currentRotation = gameObject.transform.rotation;

        if ((_previousXPosition > currentXPosition && currentRotation.y == 0) ||
            (_previousXPosition < currentXPosition && currentRotation.y != 0))
        {
            var rotation = Mathf.RoundToInt(currentRotation.eulerAngles.y + 180f) % 360;
            gameObject.transform.eulerAngles = new Vector3(0, rotation, 0);
        }
    }

    /// <summary>
    /// Clamp position so player won't pass upper and lower bounds
    /// </summary>
    private void ClampPositionIfNeeded()
    {
        // clamp position
        var updatedPosition = _rigidBody.transform.position;
        var clampedYPos = Mathf.Clamp(updatedPosition.y, GameConstants.GAME_Y_LOWER_BORDER, GameConstants.GAME_Y_UPPER_BORDER);
        updatedPosition.y = clampedYPos;

        _rigidBody.transform.position = updatedPosition;

        // stop applying force if hit one of the y borders
        if (updatedPosition.y == GameConstants.GAME_Y_LOWER_BORDER || updatedPosition.y == GameConstants.GAME_Y_UPPER_BORDER)
        {
            _rigidBody.velocity = Vector2.zero;
        }
    }

    /// <summary>
    /// Adds force to the rigidbody - performs the actual movement, takes direction from joystic
    /// </summary>
    /// <param name="joyStickDirection"></param>
    private void AddForceToRigidBody(Vector3 joyStickDirection)
    {
        // gradually reduce velocity if joystick is not moving in frame
        if (joyStickDirection == Vector3.zero)
        {
            _rigidBody.velocity *= 0.98f;
        }

        // else, add force
        else
        {
            _rigidBody.AddRelativeForce(new Vector2(joyStickDirection.x, joyStickDirection.y));
            var clampedXVelocity = Mathf.Clamp(_rigidBody.velocity.x, GameConstants.PLAYER_MIN_X_VELOCITY, GameConstants.PLAYER_MAX_X_VELOCITY);

            // clamp velocity
            _rigidBody.velocity = new Vector2(clampedXVelocity, _rigidBody.velocity.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyFish"))
        {
            var enemyFishScale = collision.transform.localScale;
            var playerScale = gameObject.transform.localScale;

            if (enemyFishScale.x >= playerScale.x)
            {
                if (!_godPowerUpActive)
                    // Invoke Game Over - only when god power up is not active
                    GameOver.Invoke();
            }
            else
            {
                // Eat (Destroy) other fish
                Destroy(collision.gameObject);

                // Get growth rate multiplier from configuration
                var growthRate = LevelManager.GetInstance().GetCurrentLevelConfigurations().PlayerGrowthRate;

                var growth = enemyFishScale * growthRate;

                // add to player's scale - but not too much as to overgrow the level
                gameObject.transform.localScale += growth * 0.8f;

                FishEaten.Invoke(enemyFishScale.x);
            }
        }
        // power up picked up
        else if (collision.gameObject.tag.StartsWith("PowerUp"))
        {
            var powerUpTypeString = collision.gameObject.tag.Split('.')[1];

            Enum.TryParse(powerUpTypeString, out PowerUpType powerUpType);

            PowerUpPickedUpEvent.Invoke(powerUpType);

            Destroy(collision.gameObject);
        }
    }
}
