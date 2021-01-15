using System;
using UnityEngine.Events;

/// <summary>
/// A unity event that gets invoked when a fish is eaten by the player
/// </summary>
[Serializable]
public class FishEatenEvent : UnityEvent<float>
{
}

