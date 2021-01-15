using UnityEngine;

/// <summary>
/// Abstract class for fish behavior - defines default sprite swapping animation
/// </summary>
public abstract class FishBehaviorBase : MonoBehaviour
{
    public Sprite FirstSprite;
    public Sprite SecondSprite;

    internal bool _firstSpriteActive;
    internal SpriteRenderer _spriteRenderer;
    internal Rigidbody2D _rigidBody;
    internal float _fishSpriteSwapTimeInterval;

    private float _fishSpriteSwapTimeReductionValue = 150f;

    // Start is called before the first frame update
    public virtual void Start()
    {
        _firstSpriteActive = true;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody = gameObject.GetComponent<Rigidbody2D>();
        _fishSpriteSwapTimeInterval = GameConstants.FISH_SPRITE_SWAP_TIME_INTERVAL;
    }

    /// <summary>
    /// Handle fish movement
    /// </summary>
    public abstract void HandleMovement();

    public virtual void FixedUpdate()
    {
        HandleMovement();
    }

    /// <summary>
    /// In LateUpdate we take care of swapping between sprites, every time the time interval elapses
    /// </summary>
    public virtual void LateUpdate()
    {
        _fishSpriteSwapTimeInterval -= Time.deltaTime * _fishSpriteSwapTimeReductionValue;

        if (_fishSpriteSwapTimeInterval <= 0)
        {
            SwapSprites();
        }
    }

    /// <summary>
    /// Swap between the currently active sprite and the inactive one
    /// </summary>
    private void SwapSprites()
    {
        _spriteRenderer.sprite = !_firstSpriteActive ? FirstSprite : SecondSprite;
        _firstSpriteActive = !_firstSpriteActive;
        _fishSpriteSwapTimeInterval = GameConstants.FISH_SPRITE_SWAP_TIME_INTERVAL;
    }
}
