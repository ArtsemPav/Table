using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("Config")]
    public LevelConfig levelConfig;

    [Header("UI")]
    public TMP_Text taskText;
    public TMP_InputField answerInput;
    public TMP_Text timerText;
    public Slider bossHpBar;
    public Slider playerHpBar;

    [Header("Params")]
    public int playerMaxHp = 3;

    private int _playerHp;
    private int _bossHp;
    private float _timeLeft;
    private int _tasksSolved;
    private bool _isBattleActive;
    private MathTask _currentTask;

    private void Awake()
    {
        if (SelectedLevelHolder.SelectedLevel != null)
            levelConfig = SelectedLevelHolder.SelectedLevel;
    }
    private void Start()
    {
        ApplyLevelConfig();
        StartBattle();
    }

    private void ApplyLevelConfig()
    {
        if (levelConfig == null)
        {
            Debug.LogError("BattleManager: LevelConfig not set!");
            enabled = false;
            return;
        }

        _bossHp = levelConfig.levelKind == LevelKind.Boss
            ? levelConfig.bossHp
            : levelConfig.tasksCount;

        _playerHp = playerMaxHp;
        _timeLeft = levelConfig.baseTime;

        if (bossHpBar != null)
        {
            bossHpBar.maxValue = _bossHp;
            bossHpBar.value = _bossHp;
        }
        if (playerHpBar != null)
        {
            playerHpBar.maxValue = playerMaxHp;
            playerHpBar.value = _playerHp;
        }
    }

    private void StartBattle()
    {
        _isBattleActive = true;
        _tasksSolved = 0;
        NextTask();
        StartCoroutine(TimerRoutine());
    }

    private IEnumerator TimerRoutine()
    {
        while (_isBattleActive && _timeLeft > 0f)
        {
            _timeLeft -= Time.deltaTime;
            if (timerText != null)
                timerText.text = Mathf.CeilToInt(_timeLeft).ToString();
            yield return null;
        }

        if (_isBattleActive && _timeLeft <= 0f)
        {
            OnBattleLose();
        }
    }

    private void NextTask()
    {
        _currentTask = MathTaskGenerator.Instance.Generate(
            levelConfig.taskType,
            levelConfig.minValue,
            levelConfig.maxValue
        );

        if (taskText != null)
            taskText.text = $"{_currentTask.a} {_currentTask.op} {_currentTask.b} = ?";

        if (answerInput != null)
        {
            answerInput.text = "";
            answerInput.ActivateInputField();
        }
    }

    public void OnSubmitAnswer()
    {
        if (!_isBattleActive) return;
        if (string.IsNullOrEmpty(answerInput.text)) return;

        int parsed;
        if (!int.TryParse(answerInput.text, out parsed))
        {
            WrongAnswer();
            return;
        }

        if (parsed == _currentTask.answer)
            CorrectAnswer();
        else
            WrongAnswer();
    }

    private void CorrectAnswer()
    {
        _tasksSolved++;
        _bossHp--;

        if (bossHpBar != null)
            bossHpBar.value = _bossHp;

        if (_bossHp <= 0)
        {
            OnBattleWin();
        }
        else
        {
            NextTask();
        }
    }

    private void WrongAnswer()
    {
        _playerHp--;
        if (playerHpBar != null)
            playerHpBar.value = _playerHp;

        if (_playerHp <= 0)
        {
            OnBattleLose();
        }
        else
        {
            NextTask();
        }
    }

    private void OnBattleWin()
    {
        _isBattleActive = false;

        int stars = 3; // можешь потом рассчитывать динамически

        if (GameManager.Instance != null && levelConfig != null)
        {
            GameManager.Instance.SetStars(levelConfig.levelId, stars);
            GameManager.Instance.AddCoins(levelConfig.baseCoinsReward);
            GameManager.Instance.AddXP(levelConfig.baseXpReward);
        }

        BattleResultHolder.IsWin = true;
        BattleResultHolder.Level = levelConfig;
        BattleResultHolder.CoinsReward = levelConfig.baseCoinsReward;
        BattleResultHolder.XpReward = levelConfig.baseXpReward;
        BattleResultHolder.StarsEarned = stars;

        SceneController.Instance.LoadScene("Results");
    }

    private void OnBattleLose()
    {
        _isBattleActive = false;

        BattleResultHolder.IsWin = false;
        BattleResultHolder.Level = levelConfig;
        BattleResultHolder.CoinsReward = 0;
        BattleResultHolder.XpReward = 0;
        BattleResultHolder.StarsEarned = 0;

        SceneController.Instance.LoadScene("Results");
    }
}
