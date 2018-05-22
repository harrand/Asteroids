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
    public float GetAsteroidRate;
    /// Maximum number of active large UFOs in the game for this level.
    public uint GetLargeUFOMax;
    /// Number of large UFOs spawned per second for this level.
    public float GetLargeUFORate;
    /// Maximum number of active small UFOs in the game for this level.
    public uint GetSmallUFOMax;
    /// Number of small UFOs spawned per second for this level.
    public float GetSmallUFORate;
    /// Score gained throughout this level until the level ends.
    public uint GetScoreLimit = 10000;
}