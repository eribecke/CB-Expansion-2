using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public List<Character> m_Characters = new List<Character>();

    private GameController _gameController;
    private LevelController _levelController;
    private bool _inputBlocked;

    private bool _ending;

    public bool InputBlocked
    {
        get => _inputBlocked;
        set => _inputBlocked = value;
    }

    public void Setup(GameController gameController, LevelController levelController)
    {
        _gameController = gameController;
        _levelController = levelController;
    }

    private void Start()
    {
        m_Characters = new List<Character>(GameObject.FindObjectsOfType<Character>());
        _ending = false;
    }

    void Update()
    {
        if (!_inputBlocked)
        {
            // get all inputs for controller first
            float translation = Input.GetAxis("Horizontal");
            bool tryJump = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);

            // apply all inputs to both characters
            bool warpable = true;
            bool exitable = true;
            foreach (var character in m_Characters)
            {
                character.ApplyMovement(translation);
                if (tryJump)
                {
                    character.TryJump();
                }

                if (!character.Warpable)
                {
                    warpable = false;
                }

                if (!character.Exitable)
                {
                    exitable = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (exitable)
                {
                    Debug.Log("EXITABLE!");
                    ExitLevel();
                }
                else if (warpable)
                {
                    Debug.Log("WARPABLE!");
                    SwapCharacters();
                }
            }
        }
    }

    private void ExitLevel()
    {
        if (_levelController != null)
        {
            _levelController.BeginFadeOut();
        }

        if (_gameController != null)
        {
            _gameController.CacheBalanceScore(GetBalanceScore());
        }
    }

    private void SwapCharacters()
    {
        if (m_Characters == null || m_Characters.Count < 2)
        {
            // something has gone wrong
            Debug.LogWarning("Trying to swap 0 or 1 wolves");
            return;
        }

        var temp = m_Characters[0].transform.position;

        m_Characters[0].transform.position = m_Characters[1].transform.position;
        m_Characters[1].transform.position = temp;
    }

    public int GetCharacterScore(WorldEnum type)
    {
        foreach (var character in m_Characters)
        {
            if (type == character.world)
            {
                return character.collectables;
            }
        }
        
        return -1;
    }

    public int GetBalanceScore()
    {
        if (_ending)
        {
            return 0;
        }
        
        return GetCharacterScore(WorldEnum.Orange) - GetCharacterScore(WorldEnum.Green);
    }
}
