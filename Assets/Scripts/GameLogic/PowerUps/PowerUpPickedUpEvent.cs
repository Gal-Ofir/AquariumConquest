using System;
using UnityEngine.Events;

/// <summary>
/// Unity event for when power ups are picked up
/// </summary>
[Serializable]
public class PowerUpPickedUpEvent : UnityEvent<PowerUpType>
{
}

