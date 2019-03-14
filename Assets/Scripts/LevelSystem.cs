using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class LevelSystem : MonoBehaviour
{
    public bool is_nebula;
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
        this.CancelInvoke();
        InvokeRepeating("SpawnAsteroidIfNeeded", 0.5f, 1.0f / this.GetCurrentLevel.GetAsteroidRate);
        InvokeRepeating("SpawnLargeUFOIfNeeded", 0.5f, 1.0f / this.GetCurrentLevel.GetLargeUFORate);
        InvokeRepeating("SpawnSmallUFOIfNeeded", 0.5f, 1.0f / this.GetCurrentLevel.GetSmallUFORate);
        SpawnNebuli();
        if (this.GetCurrentLevel.GetLevelID == 6)
        {
            Camera.main.orthographicSize = 25;
            Debug.Log("INSANITY");
            for (int i = 0; i < 20; i++)
                SpawnNebuli();
        }
    }

    public void NextLevel()
    {
        uint current_level_id = this.GetCurrentLevel.GetLevelID;
        if(++current_level_id > this.GetMaxLevelID())
        {
            this.IsCompleted = true;
            return;
        }
        this.score_reader.gameObject.GetComponent<Damageable>().Heal(10000);
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
    }
	
	void Update ()
    {
        if (this.score_reader == null)
            return;
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

    private uint GetProcessingLargeUFOCount()
    {
        uint largeUFO_count = 0;
        foreach (GameObject game_object in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (game_object.tag == "Large UFO")
                largeUFO_count++;
        }
        largeUFO_count += this.asteroid_count_cache;
        return largeUFO_count;
    }

    private uint GetProcessingSmallUFOCount()
    {
        uint smallUFO_count = 0;
        foreach (GameObject game_object in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            if (game_object.tag == "Small UFO")
                smallUFO_count++;
        }
        smallUFO_count += this.asteroid_count_cache;
        return smallUFO_count;
    }

    private Vector3 RandomViewportPoint()
    {
        return new Vector3(UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.9f), UnityEngine.Random.Range(0.1f, 0.9f));
    }

    void SpawnNebuli()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject nebula = Instantiate(Resources.Load("Prefabs/Nebula")) as GameObject;
            nebula.transform.position = Camera.main.ViewportToWorldPoint(this.RandomViewportPoint());
            nebula.transform.parent = null;
        }
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

    void SpawnLargeUFOIfNeeded()
    {
        if (!this.IsCompleted && this.GetProcessingLargeUFOCount() < this.GetCurrentLevel.GetLargeUFOMax)
        {
            GameObject ufo = Instantiate(Resources.Load("Prefabs/LargeUFO")) as GameObject;
            Camera cam = Camera.main;
            ufo.transform.position = cam.ViewportToWorldPoint(this.RandomViewportPoint());
            ufo.transform.parent = null;
        }
    }

    void SpawnSmallUFOIfNeeded()
    {
        if (!this.IsCompleted && this.GetProcessingSmallUFOCount() < this.GetCurrentLevel.GetSmallUFOMax)
        {
            GameObject ufo = Instantiate(Resources.Load("Prefabs/SmallUFO")) as GameObject;
            Camera cam = Camera.main;
            ufo.transform.position = cam.ViewportToWorldPoint(this.RandomViewportPoint());
            ufo.transform.parent = null;
        }
    }
}
