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
    public GameObject timer;
    public Slider bossHpBar;
    public Slider playerHpBar;
    public TMP_Text bossHPText;

    [Header("Params")]
    public int playerMaxHp = 3;

    private int _playerHp;
    private int _bossHp;
    private float _timeLeft;
    private TMP_Text _timerText;
    private int _tasksSolved;
    private bool _isBattleActive;
    private MathTask _currentTask;
    private int stars;

    private void Awake()
    {
        if (SaveManager.Instance.playerData.LastSelectedLevel != null)
            levelConfig = SaveManager.Instance.playerData.LastSelectedLevel;
    }
    private void Start()
    {
        _timerText = timer.GetComponentInChildren<TMP_Text>();

        if (_timerText == null)
        {
            Debug.LogError("timerText not find!");
        }

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
        bossHPText.text = _bossHp.ToString();

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
        if (_timeLeft <= 0)
        {
            timer.SetActive(false);
        }
        else if (_timeLeft > 0)
        {
            timer.SetActive(true);
            StartCoroutine(TimerRoutine());
        }
    }

    private IEnumerator TimerRoutine()
    {
        while (_isBattleActive && _timeLeft > 0f)
        {
            _timeLeft -= Time.deltaTime;
            if (_timerText != null)
                _timerText.text = Mathf.CeilToInt(_timeLeft).ToString();
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
        {
            bossHpBar.value = _bossHp;
            bossHPText.text = _bossHp.ToString();
        }

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
        if (_playerHp == 3)
        {
            stars = 3;
        } else if (_playerHp == 2)
        {
            stars = 2;
        }
        else if (_playerHp == 2)
        {
            stars = 1;
        }

        if (GameManager.Instance != null && levelConfig != null)
        {
            GameManager.Instance.SetStars(levelConfig.levelId, stars);
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
