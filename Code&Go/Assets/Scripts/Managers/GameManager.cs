﻿using AssetPackage;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private Category[] categories;
    [SerializeField] private bool loadSave = true;

    [SerializeField] private Category category;
    public int levelIndex;
    private bool gameLoaded = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SaveManager.Init();
        }
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        LoadGame();
        TrackerAsset.Instance.Completable.Initialized("articoding", CompletableTracker.Completable.Game);
        TrackerAsset.Instance.Completable.Progressed("articoding", CompletableTracker.Completable.Game, ProgressManager.Instance.GetGameProgress());
    }

    public void LoadGame()
    {
        if (loadSave && !gameLoaded)
        {
            SaveManager.Load();
            gameLoaded = true;
        }
    }

    public bool IsGameLoaded()
    {
        return gameLoaded;
    }

    public Category GetCurrentCategory()
    {
        return category;
    }

    public bool InCreatedLevel()
    {
        return category == categories[categories.Length - 1];
    }

    public int GetCurrentLevelIndex()
    {
        return levelIndex;
    }

    public void SetCurrentLevel(int levelIndex)
    {
        this.levelIndex = levelIndex;
    }

    public void SetCurrentCategory(Category category)
    {
        this.category = category;
    }

    // Esto habra que moverlo al MenuManager o algo asi
    public void LoadLevel(Category category, int levelIndex)
    {
        this.category = category;
        this.levelIndex = levelIndex;

        ProgressManager.Instance.LevelStarted(category, levelIndex);
        if (loadSave)
            SaveManager.Save();

        if (LoadManager.Instance == null)
        {
            SceneManager.LoadScene("LevelScene");
            return;
        }

        LoadManager.Instance.LoadScene("LevelScene");
    }

    public void LoadLevelCreator()
    {
        if (LoadManager.Instance == null)
        {
            SceneManager.LoadScene("BoardCreation");
            return;
        }
        LoadManager.Instance.LoadScene("BoardCreation");
    }

    public void LoadScene(string name)
    {
        if (LoadManager.Instance == null)
        {
            SceneManager.LoadScene(name);
            return;
        }
        LoadManager.Instance.LoadScene(name);
    }

    public void OnDestroy()
    {
        if (Instance == this && loadSave)
            SaveManager.Save();
    }

    public void OnApplicationQuit()
    {
        if (loadSave)
            SaveManager.Save();
    }

    public string GetCurrentLevelName()
    {
        // TODO: nombre distinto si estas en el creador

        Category category = GetCurrentCategory();
        int levelIndex = GetCurrentLevelIndex();
        string levelName = category.levels[levelIndex].levelName;

        return levelName;
    }

    public Category[] GetCategories()
    {
        return categories;
    }
}
