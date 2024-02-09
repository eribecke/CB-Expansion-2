using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroSceneController : MonoBehaviour
{
    public string HarmonyText;
    public string DarkBalanceText;
    public string LightBalanceText;
    public int BalanceTolerance = 3;
    public DialogueHUDInfo DialogueHudInfo;

    public Animator LightAnimator;
    public Animator DarkAnimator;
    public Animator MergeAnimator;

    private GameController _gameController;
    
    public float WaitTime = 10.0f;

    private float _countdown;
    
    void Start()
    {
        _gameController = FindObjectOfType<GameController>();
        _countdown = WaitTime;

        var outroTextToShow = HarmonyText;

        if (_gameController)
        {
            var balance = _gameController.CurrentBalanceScore();

            if (balance > BalanceTolerance)
            {
                // LIGHT BALANCE
                outroTextToShow = LightBalanceText;
                
                LightAnimator.gameObject.SetActive(true);
                LightAnimator.SetBool("Grounded", true);
                DarkAnimator.gameObject.SetActive(false);
                MergeAnimator.gameObject.SetActive(false);
            }
            else if (balance < -BalanceTolerance)
            {
                // DARK BALANCE
                outroTextToShow = DarkBalanceText;
                
                LightAnimator.gameObject.SetActive(false);
                DarkAnimator.gameObject.SetActive(true);
                DarkAnimator.SetBool("Grounded", true);
                MergeAnimator.gameObject.SetActive(false);
            }
            else
            {
                // BALANCED!
                outroTextToShow = HarmonyText;
                
                LightAnimator.gameObject.SetActive(false);
                DarkAnimator.gameObject.SetActive(false);
                MergeAnimator.gameObject.SetActive(true);
            }
        }

        DialogueHudInfo.gameObject.SetActive(true);
        DialogueHudInfo.PresentText(outroTextToShow);
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
                _gameController.ResetToSplashScreen();
            }
        }
    }
}
