using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// UI Manager script
/// </summary>
public class UIManager : MonoBehaviour
{
    public FixedJoystick Joystick;

    public GameObject InGameUI;
    public TextMeshProUGUI GameOverText;
    public TextMeshProUGUI StartLevelText;
    public TextMeshProUGUI LevelWonText;

    public TextMeshProUGUI TutorialLevelFirstText;
    public TextMeshProUGUI TutorialLevelSecondText;
    public TextMeshProUGUI TutorialLevelThirdText;
    public Button TutorialLevelOkButton;

    public Transform LevelSelectPrefab;

    public UnityEvent StartLevel;

    private GameObject _mainGameMenuContainer;
    private GameObject _mainMenu;
    private Transform _levelSelectionContainer;
    private Transform _levelSelectionMenu;
    private List<Transform> _levelSelectionList;

    private static UIManager _instance;

    private void Awake()
    {
        if (_instance == null )
        {
            _instance = this;
        }

        _mainGameMenuContainer = GameObject.FindGameObjectWithTag("MenuContainer");
        _mainMenu = GameObject.FindGameObjectWithTag("MainMenu");

        _levelSelectionContainer = GetComponentsInChildren<Transform>(includeInactive: true)
                        .First(go => go.CompareTag("LevelSelectionContainer"));
        _levelSelectionMenu = GetComponentsInChildren<Transform>(includeInactive: true)
                                .First(go => go.CompareTag("LevelSelectionMenu"));

        TutorialLevelFirstText.enabled = false;
        TutorialLevelSecondText.enabled = false;
        TutorialLevelOkButton.gameObject.SetActive(false);

        _levelSelectionList = new List<Transform>();
    }

    /// <summary>
    /// Handles level started button clicked
    /// </summary>
    public void OnStartLevelClicked()
    {
        // enable joystick and start current level by emitting StartLevel event
        Joystick.Direction = Vector2.zero;
        Joystick.gameObject.SetActive(true);
        StartLevel.Invoke();
        // disable the menu
        _mainGameMenuContainer.SetActive(false);

        // disable game over text if it was active
        GameOverText.enabled = false;

        // enable ingame UI
        InGameUI.SetActive(true);

        DestroyLevelSelectionMenu();

        Time.timeScale = 1f;
    }

    /// <summary>
    /// Handles 'Select level' button clicked
    /// </summary>
    public void OnSelectLevelClicked()
    {
        SetupLevelSelectionMenu();

        // Disable main menu
        _mainMenu.SetActive(false);
        LevelWonText.enabled = false;
        GameOverText.enabled = false;

        // enable level selection menu
        _levelSelectionMenu.gameObject.SetActive(true);
    }

    /// <summary>
    /// Handles 'Back' button clicked
    /// </summary>
    public void OnBackToMainMenuClicked()
    {
        // Enable main menu
        _mainMenu.SetActive(true);

        // Disable level selection menu and destroy the level selection buttons
        _levelSelectionMenu.gameObject.SetActive(false);
        DestroyLevelSelectionMenu();
    }

    /// <summary>
    /// Handles level won event
    /// </summary>
    public void OnLevelWon()
    {
        ResetUI();

        StartLevelText.text = "Next";
        LevelWonText.enabled = true;
        GameOverText.enabled = false;

        // update active level
        GameManager.GetInstance().UpdateActiveLevel(GameManager.GetInstance().GetActiveLevel() + 1);
    }

    /// <summary>
    /// Handles game over event
    /// </summary>
    public void OnGameOver()
    {
        ResetUI();

        StartLevelText.text = "Retry";
        LevelWonText.enabled = false;
        GameOverText.enabled = true;
    }

    /// <summary>
    /// Handles tutorial level state changed event
    /// </summary>
    public void TutorialLevelEventHandler(TutorialLevelState tutorialLevelState)
    {
        switch (tutorialLevelState)
        {
            case TutorialLevelState.Started:
                // activate text
                TutorialLevelFirstText.enabled = true;
                break;
            case TutorialLevelState.SmallFishEaten:
                TutorialLevelFirstText.enabled = false;
                break;
            case TutorialLevelState.BigFishSpawned:
                // activate text
                TutorialLevelSecondText.enabled = true;
                break;
            case TutorialLevelState.MediumFish1Eaten:
                TutorialLevelSecondText.enabled = false;
                TutorialLevelThirdText.enabled = true;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Set up level selection menu, according to how many levels player has passed
    /// </summary>
    private void SetupLevelSelectionMenu()
    {
        var numOfLevels = GameConstants.NUMBER_OF_LEVELS;

        var levelPassed = GameManager.GetInstance().GetLevelPassed();

        var xPosition = UIConstants.LEVEL_SELECT_PREFAB_START_X_POS;
        var yPosition = UIConstants.LEVEL_SELECT_PREFAB_START_Y_POS;
        var currentLevel = 0;

        for (int i = 0; i < numOfLevels; i += UIConstants.LEVEL_SELECT_PREFAB_COL_LENGTH)
        {
            for (int j = 0; j < UIConstants.LEVEL_SELECT_PREFAB_COL_LENGTH; j++)
            {
                var levelSelection = Instantiate(LevelSelectPrefab, _levelSelectionContainer);
                var levelSelectionText = levelSelection.GetComponentInChildren<TextMeshProUGUI>();
                
                // Set level text
                if (currentLevel == 0)
                {
                    levelSelectionText.text = "Tutorial";
                }
                else
                {
                    levelSelectionText.text = $"Level {currentLevel}";
                }

                // Set level color
                if (levelPassed < currentLevel)
                {
                    levelSelection.GetComponent<Image>().color = UIConstants.LEVEL_SELECT_DISABLED_COLOR;
                    levelSelection.GetComponent<Button>().interactable = false;
                }

                levelSelection.localPosition = new Vector3(xPosition, yPosition, 0);

                xPosition += UIConstants.LEVEL_SELECT_PREFAB_X_INTERVAL;

                currentLevel++;
                _levelSelectionList.Add(levelSelection);
            }

            xPosition = UIConstants.LEVEL_SELECT_PREFAB_START_X_POS;
            yPosition += UIConstants.LEVEL_SELECT_PREFAB_Y_INTERVAL;
        }

        _levelSelectionList.ForEach(levelSelection =>
        {
            // add event listener to button
            levelSelection.GetComponent<Button>().onClick.AddListener(() =>
            {
                OnLevelSelectedClick(_levelSelectionList.IndexOf(levelSelection));
            });
        });              
    }

    /// <summary>
    /// Handles a level selected button clicked
    /// </summary>
    /// <param name="levelSelected"></param>
    private void OnLevelSelectedClick(int levelSelected)
    {
        GameManager.GetInstance().UpdateActiveLevel(levelSelected);
    }

    /// <summary>
    /// Destroys the level selection menu.
    /// Increases performance and lets the selection menu be updated each time a new level was passed
    /// </summary>
    private void DestroyLevelSelectionMenu()
    {
        // destroy all level selection buttons (memory save + update if level was passed)
        _levelSelectionList.ForEach(levelSelect => { Destroy(levelSelect.gameObject); });
        _levelSelectionList = new List<Transform>();
    }

    /// <summary>
    /// Reset the UI
    /// </summary>
    private void ResetUI()
    {
        Time.timeScale = 0f;
        Joystick.input = Vector2.zero;
        Joystick.gameObject.SetActive(false);
        GameOverText.enabled = true;
        _mainGameMenuContainer.SetActive(true);
        _mainMenu.SetActive(true);
        _levelSelectionMenu.gameObject.SetActive(false);
        TutorialLevelFirstText.enabled = false;
        TutorialLevelSecondText.enabled = false;
        TutorialLevelThirdText.enabled = false;
        InGameUI.SetActive(false);
        DestroyLevelSelectionMenu();
    }
}
