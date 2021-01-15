using UnityEngine;

/// <summary>
/// UI-Specific constants
/// </summary>
public class UIConstants
{
    #region Level Selection Constants
    public static float LEVEL_SELECT_PREFAB_START_X_POS = -210f;
    public static float LEVEL_SELECT_PREFAB_START_Y_POS = 95f;
    public static float LEVEL_SELECT_PREFAB_X_INTERVAL = 140f;
    public static float LEVEL_SELECT_PREFAB_Y_INTERVAL = -90f;
    public static int LEVEL_SELECT_PREFAB_COL_LENGTH = 4;

    public static Color LEVEL_SELECT_DISABLED_COLOR = new Color(0.58f, 0.58f, 0.58f, 0.78f);
    #endregion

    #region Healthbar constants
    public static float POWER_UP_HEALTHBAR_START_HEALTH = 100f;

    public static Vector3 POWER_UP_HEALTHBAR_POSITION = new Vector3(0f, 475f, 0f);
    public static Vector3 SCORE_HEALTHBAR_POSITION = new Vector3(0f, -500f, 0f);
    #endregion
}

