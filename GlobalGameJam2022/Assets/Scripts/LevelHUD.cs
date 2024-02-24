using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelHUD : MonoBehaviour
{
    public TextMeshProUGUI GreenCounter;
    public TextMeshProUGUI OrangeCounter;
    public Animator OverlayAnimator;
    public DialogueHUDInfo DialogueHudInfo;
    public DialogueHUDInfo DialogueHudInfoOutro;
    public Button saveGame;
    private SaveManager saveManager;

    private CharacterController _characterController;

    private void Start()
    {
        //references SaveManager
        saveManager = FindObjectOfType<SaveManager>();
        Debug.Log("started");
        Debug.Log("save manager" +  saveManager);

        _characterController = FindObjectOfType<CharacterController>();

        //save button listener
        saveGame.onClick.AddListener(() => SaveGame());
    }

    public void SaveGame()
    {
        //calls SaveGameData method from SaveManager script after save button is clicked
        Debug.Log("Save Pressed");
        saveManager.SaveGameData();
    }

    public void SetupIntroText(string introText)
    {
        DialogueHudInfo.gameObject.SetActive(true);
        DialogueHudInfoOutro.gameObject.SetActive(false);
        DialogueHudInfo.PresentText(introText);
    }

    public void SetupOutroText(string outroText)
    {
        DialogueHudInfo.gameObject.SetActive(false);
        DialogueHudInfoOutro.gameObject.SetActive(true);
        DialogueHudInfoOutro.PresentText(outroText);
    }

    private void Update()
    {
        if (_characterController != null)
        {
            OrangeCounter.text = $"Orange: {_characterController.GetCharacterScore(WorldEnum.Orange)}";
            GreenCounter.text = $"Green: {_characterController.GetCharacterScore(WorldEnum.Green)}";
        }
    }

    public void FadeoutIntroText()
    {
        if (OverlayAnimator != null)
        {
            OverlayAnimator.Play("Fadeout", -1, 0f);
        }
    }

    public void FadeinOutroText()
    {
        if (OverlayAnimator != null)
        {
            OverlayAnimator.Play("Fadein", -1, 0f);
        }
    }
}
