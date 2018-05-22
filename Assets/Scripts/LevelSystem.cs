using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelSystem : MonoBehaviour
{
    public Scored score_reader;
    private uint previous_score;
    public bool IsCompleted { get; private set; }
    private uint asteroid_count_cache;

    private SortedList<uint, Level> levels;
    public Level GetCurrentLevel { get; private set; }

    public uint GetMaxLevelID()
    {
        if (this.levels.Count == 0)
            return 0;
        return this.levels.Keys[this.levels.Count - 1];
    }

    public void LoadLevel(int level_id)
    {
        this.GetCurrentLevel = this.levels.Values[level_id];
    }

    public void NextLevel()
    {
        uint current_level_id = this.GetCurrentLevel.GetLevelID;
        if(++current_level_id > this.GetMaxLevelID())
        {
            this.IsCompleted = true;
            return;
        }
        this.LoadLevel(Convert.ToInt32(current_level_id));
    }

	void Start ()
    {
        this.IsCompleted = false;
        this.asteroid_count_cache = 0;
        this.levels = new SortedList<uint, Level>();
		foreach(Transform child in this.transform)
        {
            GameObject child_object = child.gameObject;
            Level child_level = child_object.GetComponent<Level>();
            if (child_level != null)
            {
                this.levels.Add(child_level.GetLevelID, child_level);
            }
        }
        this.LoadLevel(0);

        InvokeRepeating("SpawnAsteroidIfNeeded", 0.5f, 1.0f / this.GetCurrentLevel.GetAsteroidRate);
    }
	
	void Update ()
    {
        uint score_delta = this.score_reader.score - this.previous_score;
        if (this.GetCurrentLevel == null || this.IsCompleted)
            return;
        if (score_delta >= this.GetCurrentLevel.GetScoreLimit && !this.IsCompleted)
        {
            this.previous_score = this.score_reader.score;
            this.NextLevel();
        }
	}

    private uint GetProcessingAsteroidCount()
    {
        uint asteroid_count = 0;
        foreach (GameObject game_object in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (game_object.tag == "Asteroid")
                asteroid_count++;
        }
        asteroid_count += this.asteroid_count_cache;
        return asteroid_count;
    }

    private Vector3 RandomViewportPoint()
    {
        return new Vector3(UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.9f));
    }

    void SpawnAsteroidIfNeeded()
    {
        if(!this.IsCompleted && this.GetProcessingAsteroidCount() < this.GetCurrentLevel.GetAsteroidMax)
        {
            GameObject asteroid = Instantiate(Resources.Load("Prefabs/Large Rock")) as GameObject;
            Camera cam = Camera.main;
            asteroid.transform.position = cam.ViewportToWorldPoint(this.RandomViewportPoint());
            asteroid.transform.parent = null;
        }
    }
}
