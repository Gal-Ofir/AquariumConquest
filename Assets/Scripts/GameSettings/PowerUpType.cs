using System;

/// <summary>
/// Defines different power up types
/// </summary>
[Serializable]
public enum PowerUpType
{
    /// <summary>
    /// Slows up enemy fish for a duration of time
    /// </summary>
    SlowTime,
    /// <summary>
    /// While active, player fish cannot be eaten
    /// </summary>
    God
}