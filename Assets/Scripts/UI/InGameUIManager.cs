using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Manages the Ingame ui components
/// </summary>
public class InGameUIManager : MonoBehaviour
{
    public Transform ScoreHealthbarPrefab;
    public Transform PowerUpHealthBarPrefab;

    public UnityEvent PowerUpDurationFinished;

    private float _powerUpHealthBarActivationTime;

    private Transform _scoreHealthBarTransform;
    private Transform _powerUpHealthBarTransform;

    /// <summary>
    /// Handles level started event
    /// </summary>
    public void OnLevelStarted()
    {
        DestroyHealthBars();

        // Create a new healthbar and set this gameObject as parent
        _scoreHealthBarTransform = Instantiate(ScoreHealthbarPrefab);

        var scoreHealthBar = _scoreHealthBarTransform.gameObject.GetComponent<Healthbar>();

        _scoreHealthBarTransform.transform.parent = transform;
        _scoreHealthBarTransform.transform.localPosition = UIConstants.SCORE_HEALTHBAR_POSITION;

        var scoreHealthBarSlider = scoreHealthBar.GetComponent<Slider>();

        // Set the score healthbar as 0 and define max, low and high health settings
        scoreHealthBar.regenerateHealth = false;
        scoreHealthBar.SetHealth(0f);
        scoreHealthBar.maximumHealth = LevelManager.GetInstance().GetCurrentLevelConfigurations().ScoreTarget;
        scoreHealthBar.highHealth = (int) (scoreHealthBar.maximumHealth * 0.66f);
        scoreHealthBar.lowHealth = (int)(scoreHealthBar.maximumHealth * 0.33f);

        scoreHealthBarSlider.maxValue = LevelManager.GetInstance().GetCurrentLevelConfigurations().ScoreTarget;
    }

    /// <summary>
    /// Handles game over - destroys lingering healthbars
    /// </summary>
    public void OnGameOver()
    {
        DestroyHealthBars();
    }

    /// <summary>
    /// Handles fish eaten by player event
    /// </summary>
    /// <parm name="scoreToAdd"></param>
    public void OnFishEaten(float scoreToAdd)
    {
        var scoreHealthBar = _scoreHealthBarTransform.gameObject.GetComponent<Healthbar>();

        scoreHealthBar.GainHealth(scoreToAdd);
    }

    /// <summary>
    /// Handles power up picked up event - starts the powerup health bar
    /// </summary>
    public void OnPowerUpPickedUp(PowerUpType powerUpType)
    {
        if (_powerUpHealthBarTransform == null)
        {
            _powerUpHealthBarTransform = Instantiate(PowerUpHealthBarPrefab);
            _powerUpHealthBarTransform.transform.parent = transform;
            _powerUpHealthBarTransform.transform.localPosition = UIConstants.POWER_UP_HEALTHBAR_POSITION;

            var powerUpHealthBar = _powerUpHealthBarTransform.gameObject.GetComponent<Healthbar>();

            powerUpHealthBar.regenerateHealth = false;
            powerUpHealthBar.SetHealth(UIConstants.POWER_UP_HEALTHBAR_START_HEALTH);

            powerUpHealthBar.highHealth = (int)(powerUpHealthBar.maximumHealth * 0.66f);
            powerUpHealthBar.lowHealth = (int)(powerUpHealthBar.maximumHealth * 0.33f);

            _powerUpHealthBarActivationTime = Time.time;

            var powerUpHealthBarSlider = _powerUpHealthBarTransform.GetComponent<Slider>();
            powerUpHealthBarSlider.maxValue = UIConstants.POWER_UP_HEALTHBAR_START_HEALTH;
            powerUpHealthBarSlider.minValue = 0f;
        }
    }

    private void FixedUpdate()
    {
        if (_powerUpHealthBarTransform != null && _powerUpHealthBarTransform.gameObject != null && _powerUpHealthBarTransform.gameObject.activeSelf)
        {
            var powerUpHealthBar = _powerUpHealthBarTransform.gameObject.GetComponent<Healthbar>();

            var currentTimeSinceActivation = Time.time - _powerUpHealthBarActivationTime;
            var powerUpHealthBarPercentageFilled = currentTimeSinceActivation / (GameConstants.POWERUP_DURATION_MS / 1000f);

            var powerUpValue = UIConstants.POWER_UP_HEALTHBAR_START_HEALTH - Mathf.Lerp(UIConstants.POWER_UP_HEALTHBAR_START_HEALTH, 0f, powerUpHealthBarPercentageFilled);

            powerUpHealthBar.SetHealth(powerUpValue);

            if (powerUpHealthBar.health >= UIConstants.POWER_UP_HEALTHBAR_START_HEALTH)
            {
                Destroy(_powerUpHealthBarTransform.gameObject);
                _powerUpHealthBarTransform = null;

                PowerUpDurationFinished.Invoke();
            }
        }
    }

    /// <summary>
    /// Destroy the healthbars 
    /// </summary>
    private void DestroyHealthBars()
    {
        if (_powerUpHealthBarTransform != null)
        {
            Destroy(_powerUpHealthBarTransform.gameObject);
        }

        if (_scoreHealthBarTransform != null)
        {
            Destroy(_scoreHealthBarTransform.gameObject);
        }
    }
}
