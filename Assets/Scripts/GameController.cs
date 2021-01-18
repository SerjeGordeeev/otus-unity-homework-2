using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

internal sealed class GameController : MonoBehaviour
{
    public Button attackButton;
    public Button menuButton;
    public CanvasGroup buttonPanel;
    public CanvasGroup menuCanvasGroup;
 

    [Header("Game Menu")]
    public CanvasGroup gameplayCanvasGroup;
    public Button continueButton;
    public Button restartLevelButton;
    public Button exitToMainMenuButton;

    [Header("Game Results")]
    public CanvasGroup resultsPanel;
    public Button resultsExitToMainMenuButton;
    public TextMeshProUGUI resultsText;

    [Header("Characters")]
    public Character[] playerCharacter;
    public Character[] enemyCharacter;
    Character currentTarget;
    bool waitingForInput;

    Character FirstAliveCharacter(Character[] characters)
    {
        return characters.FirstOrDefault(character => !character.IsDead());
    }

    void PlayerWon()
    {
        resultsText.text = "Player won!";
        Utility.SetCanvasGroupEnabled(resultsPanel, true);
    }

    void PlayerLost()
    {
        resultsText.text = "Player lost!";
        Utility.SetCanvasGroupEnabled(resultsPanel, true);
    }

    bool CheckEndGame()
    {
        if (FirstAliveCharacter(playerCharacter) == null) {
            PlayerLost();
            return true;
        }

        if (FirstAliveCharacter(enemyCharacter) == null) {
            PlayerWon();
            return true;
        }

        return false;
    }

    //[ContextMenu("Player Attack")]
    public void PlayerAttack()
    {
        waitingForInput = false;
    }

    //[ContextMenu("Next Target")]
    public void NextTarget()
    {
        int index = Array.IndexOf(enemyCharacter, currentTarget);
        for (int i = 1; i < enemyCharacter.Length; i++) {
            int next = (index + i) % enemyCharacter.Length;
            if (!enemyCharacter[next].IsDead()) {
                currentTarget.targetIndicator.gameObject.SetActive(false);
                currentTarget = enemyCharacter[next];
                currentTarget.targetIndicator.gameObject.SetActive(true);
                return;
            }
        }
    }

    IEnumerator GameLoop()
    {
        yield return null;
        while (!CheckEndGame()) {
            foreach (var player in playerCharacter)
            {
                if (player.IsDead())
                {
                    continue;
                }

                currentTarget = FirstAliveCharacter(enemyCharacter);
                if (currentTarget == null)
                    break;

                currentTarget.targetIndicator.gameObject.SetActive(true);
                Utility.SetCanvasGroupEnabled(buttonPanel, true);

                waitingForInput = true;
                while (waitingForInput)
                    yield return null;

                Utility.SetCanvasGroupEnabled(buttonPanel, false);
                currentTarget.targetIndicator.gameObject.SetActive(false);

                player.target = currentTarget.transform;
                player.AttackEnemy();

                while (!player.IsIdle())
                    yield return null;

                break;
            }

            foreach (var enemy in enemyCharacter)
            {
                if(enemy.IsDead())
                {
                    continue;
                }

                Character target = FirstAliveCharacter(playerCharacter);
                if (target == null)
                    break;

                enemy.target = target.transform;
                enemy.AttackEnemy();

                while (!enemy.IsIdle())
                    yield return null;

                break;
            }
        }
    }

    public void OpenMenu()
    {
        Utility.SetCanvasGroupEnabled(menuCanvasGroup, true);
        Utility.SetCanvasGroupEnabled(gameplayCanvasGroup, false);
    }

    public void ContinueGame()
    {
        Utility.SetCanvasGroupEnabled(menuCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(gameplayCanvasGroup, true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void Start()
    {
        attackButton.onClick.AddListener(PlayerAttack);

        continueButton.onClick.AddListener(ContinueGame);
        restartLevelButton.onClick.AddListener(RestartLevel);
        exitToMainMenuButton.onClick.AddListener(ExitToMainMenu);
        resultsExitToMainMenuButton.onClick.AddListener(ExitToMainMenu);

        Utility.SetCanvasGroupEnabled(buttonPanel, false);
        Utility.SetCanvasGroupEnabled(menuCanvasGroup, false);
        Utility.SetCanvasGroupEnabled(resultsPanel, false);
        StartCoroutine(GameLoop());
    }
}