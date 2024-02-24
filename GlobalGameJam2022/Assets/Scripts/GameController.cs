using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour, ISaveable
{
    public string SplashScreen = "TestScene";
    
    public List<string> LevelList = new List<string>();

    public string EndScene;

    private SaveManager saveManager;

    [Header("Music Controls")] 
    public int MaxImbalance;

    public AudioSource DarkAudio;
    public AudioSource LightAudio;

    public float VolumeSpeed;

    private int _activeLevelNum = 0;

    private LevelController _currentLevelController;

    public void SaveData(PlayerData data)
    {
        data.currSceneName = SceneManager.GetActiveScene().name;
    }

    public void LoadData(PlayerData data)
    {
        Debug.Log("load called from game controller");
        SceneManager.LoadScene(data.currSceneName);
    }

    public LevelController CurrentLevelController
    {
        get => _currentLevelController;
        set => _currentLevelController = value;
    }

    // balance 0 = perfectly balanced.
    // -ve = dark balanced.
    // +ve = light balanced
    private int _overallBalanceScore = 0;
    
    private int _cachedBalanceScore = -1;
    
    // audio variables
    private float _current;
    private float _target;
    private bool _volumeUp;

    public int CurrentBalanceScore()
    {
        int retval = _overallBalanceScore;
        if (_currentLevelController != null)
        {
            retval += _currentLevelController.GetLevelBalanceScore();
        }

        return retval;
    }
    
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);   
        saveManager = FindObjectOfType<SaveManager>();
    }

    private void Start()
    {
        if(saveManager.playerData != null)
        {
            LoadData(saveManager.playerData);
        }
        _overallBalanceScore = 0;
        _cachedBalanceScore = 0;
        _current = 0.5f;
        _target = 0.5f;

        DarkAudio.volume = 1.0f - _current;
        LightAudio.volume = _current;
        
        //SceneManager.LoadScene(LevelOne);
        _activeLevelNum = 0;
        SceneManager.LoadScene(SplashScreen);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetScene();
        }

        var currBalanceScore = CurrentBalanceScore();
        if (currBalanceScore != _cachedBalanceScore)
        {
            _volumeUp = currBalanceScore > _cachedBalanceScore;
            _cachedBalanceScore = currBalanceScore;

            UpdateMusicBalance();
        }
        
        AudioEasing();
    }

    private void AudioEasing()
    {
        if (Mathf.Approximately(_current,_target))
        {
            return;
        }
        
        _current += VolumeSpeed * Time.deltaTime * ((_volumeUp) ? 1.0f : -1.0f);

        if (_volumeUp)
        {
            if (_current > _target)
            {
                _current = _target;
            }
        }
        else
        {
            if (_current < _target)
            {
                _current = _target;
            }
        }
        
        DarkAudio.volume = 1.0f - _current;
        LightAudio.volume = _current;
    }

    void UpdateMusicBalance()
    {
        // clamp imbalance between maximums
        var value = Mathf.Clamp(_cachedBalanceScore,-MaxImbalance,MaxImbalance);

        var normValue = Mathf.InverseLerp(-MaxImbalance, MaxImbalance, value);

        _target = normValue;
    }

    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        _overallBalanceScore = 0;
        _cachedBalanceScore = 0;
        _current = 0.5f;
        _target = 0.5f;

        DarkAudio.volume = 1.0f - _current;
        LightAudio.volume = _current;
        _activeLevelNum = 0;
        SceneManager.LoadScene(LevelList[_activeLevelNum]);
    }

    public void ResetToSplashScreen()
    {
        _overallBalanceScore = 0;
        _cachedBalanceScore = 0;
        _current = 0.5f;
        _target = 0.5f;
        
        _activeLevelNum = 0;
        SceneManager.LoadScene(SplashScreen);
    }

    public bool TryAdvanceScene()
    {
        _activeLevelNum++;

        if (_activeLevelNum >= LevelList.Count)
        {
            SceneManager.LoadScene(EndScene);
            // has reached the end of the level
            return false;
        }

        SceneManager.LoadScene(LevelList[_activeLevelNum]);
        return true;
    }

    public void CacheBalanceScore(int delta)
    {
        _overallBalanceScore += delta;
    }
}
