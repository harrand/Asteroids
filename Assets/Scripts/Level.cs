using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{

    /// ID of this level.
    public uint GetLevelID;
    /// Maximum number of active asteroids in the game for this level.
    public uint GetAsteroidMax;
    /// Number of asteroids spawned per second for this level.
    public uint GetAsteroidRate;
    /// Score gained throughout this level until the level ends.
    public uint GetScoreLimit = 10000;
}