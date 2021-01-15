using System;
using UnityEngine;

/// <summary>
/// Defines obstacle configurations
/// </summary>
[Serializable]
public class ObstacleConfiguration
{
    public int Obstacle { get; set; }

    public Vector3 Position { get; set; }
    
    public Vector3 Rotation { get; set; }
}

