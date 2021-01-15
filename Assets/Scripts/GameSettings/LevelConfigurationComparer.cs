using System.Collections.Generic;

/// <summary>
/// Sorts <see cref="LevelConfiguration"/> objects by <see cref="LevelConfiguration.LevelId"/> field
/// </summary>
public class LevelConfigurationComparer : IComparer<LevelConfiguration>
{
    public int Compare(LevelConfiguration x, LevelConfiguration y)
    {
        return x.LevelId - y.LevelId;
    }
}

