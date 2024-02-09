using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelHUD : MonoBehaviour
{
    public TextMeshProUGUI GreenCounter;
    public TextMeshProUGUI OrangeCounter;
    public Animator OverlayAnimator;
    public DialogueHUDInfo DialogueHudInfo;
    public DialogueHUDInfo DialogueHudInfoOutro;

    private CharacterController _characterController;

    private void Start()
    {
        _characterController = FindObjectOfType<CharacterController>();
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
