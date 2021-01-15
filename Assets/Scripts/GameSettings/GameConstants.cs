
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A class for defining game related constants
/// </summary>
public class GameConstants
{
    #region General
    public static int NUMBER_OF_LEVELS = 12;
    public static float FISH_Z_VALUE = 2f;
    #endregion

    #region Player Constants
    public static float PLAYER_MOVE_SPEED = 25f;
    public static float PLAYER_MAX_X_VELOCITY = 30f;
    public static float PLAYER_MIN_X_VELOCITY = -30f;
    public static Vector3 PLAYER_STARTING_SCALE = new Vector3(1.5f, 1.5f, 0f);
    #endregion

    #region Game Dimensions
    public static float GAME_X_LEFT_BORDER = -36f;
    public static float GAME_X_RIGHT_BORDER = 36f;
    public static float GAME_Y_UPPER_BORDER = 17.5f;
    public static float GAME_Y_LOWER_BORDER = -17.5f;
    #endregion

    #region Fish Constants
    public static float FISH_SPRITE_SWAP_TIME_INTERVAL = 45f;

    public static float SPAWNED_FISH_MIN_SCALE = 0.1f;
    public static float SPAWNED_FISH_MAX_SCALE = 6f;
    #endregion

    #region Tutorial Level Constants
    public static Vector3 TUTORIAL_LEVEL_SMALL_FISH_SCALE = new Vector3(1f, 1f, 0f);
    public static Vector3 TUTORIAL_LEVEL_MEDIUM_FISH_1_SCALE = new Vector3(1.3f, 1.3f, 0f);
    public static Vector3 TUTORIAL_LEVEL_MEDIUM_FISH_2_SCALE = new Vector3(1.5f, 1.5f, 0f);
    public static Vector3 TUTORIAL_LEVEL_BIG_FISH_SCALE = new Vector3(4f, 4f, 0f);
    public static float TUTORIAL_LEVEL_SCORE_TARGET = 3.8f;
    #endregion

    #region Obstacle constants
    public static int OBSTACLE_TYPE_T_SHAPE = 0;
    public static int OBSTACLE_TYPE_HORIZONTAL = 1;
    #endregion

    #region Powerup constants
    public static float POWERUP_SPAWN_MIN_X_POS = -32f;
    public static float POWERUP_SPAWN_MAX_X_POS = 32f;
    public static float POWERUP_SPAWN_Y_POS = 21f;

    public static float POWERUP_DURATION_MS = 10000f;
    #endregion
}
