using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreenController : MonoBehaviour
{
    public float WaitTime = 1.0f;

    private float _countdown;
    private GameController _gameController;
    
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _countdown = WaitTime;
    }
    
    void Update()
    {
        if (_countdown > 0.0f)
        {
            _countdown -= Time.deltaTime;
        }

        if (_countdown <= 0.0f)
        {
            if (Input.anyKeyDown)
            {
                _gameController.StartGame();
            }
        }
    }
}
