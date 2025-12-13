using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class BattleManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Text _taskText;
    [SerializeField] private TMP_InputField _answerInput;
    [SerializeField] private GameObject _timer;
    [SerializeField] private Slider _bossHpBar;
    [SerializeField] private Slider _playerHpBar;
    [SerializeField] private TMP_Text _bossHPText;

    [Header("Params")]
    [SerializeField] private int _playerMaxHp = 3;

    private int _playerHp;
    private int _bossHp;
    private float _timeLeft;
    private TMP_Text _timerText;
    private int _tasksSolved;
    private bool _isBattleActive;
    private MathTask _currentTask;
    private int _stars;
    private LevelConfig _levelConfig;
    private IslandConfig _islandConfig;

    private void Start()
    {
        _timerText = _timer.GetComponentInChildren<TMP_Text>();

        if (_timerText == null)
        {
            Debug.LogError("timerText not find!");
        }
        _islandConfig = GameManager.Instance.GetLastIslandConfig();
        _levelConfig = GameManager.Instance.GetLastLevelConfig();
        ApplyLevelConfig();
        StartBattle();
    }

    private void ApplyLevelConfig()
    {
        if (_levelConfig == null)
        {
            Debug.LogError("BattleManager: LevelConfig not set!");
            enabled = false;
            return;
        }

        _bossHp = _levelConfig.levelKind == LevelKind.Boss
            ? _levelConfig.bossHp
            : _levelConfig.tasksCount;
        _bossHPText.text = _bossHp.ToString();

        _playerHp = _playerMaxHp;
        _timeLeft = _levelConfig.baseTime;

        if (_bossHpBar != null)
        {
            _bossHpBar.maxValue = _bossHp;
            _bossHpBar.value = _bossHp;
        }
        if (_playerHpBar != null)
        {
            _playerHpBar.maxValue = _playerMaxHp;
            _playerHpBar.value = _playerHp;
        }
    }

    private void StartBattle()
    {
        _isBattleActive = true;
        _tasksSolved = 0;
        NextTask();
        if (_timeLeft <= 0)
        {
            _timer.SetActive(false);
        }
        else if (_timeLeft > 0)
        {
            _timer.SetActive(true);
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
            _levelConfig.taskType,
            _levelConfig.minValue,
            _levelConfig.maxValue
        );

        if (_taskText != null)
            _taskText.text = $"{_currentTask.a} {_currentTask.op} {_currentTask.b} = ?";

        if (_answerInput != null)
        {
            _answerInput.text = "";
            _answerInput.ActivateInputField();
        }
    }

    public void OnSubmitAnswer()
    {
        if (!_isBattleActive) return;
        if (string.IsNullOrEmpty(_answerInput.text)) return;

        int parsed;
        if (!int.TryParse(_answerInput.text, out parsed))
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

        if (_bossHpBar != null)
        {
            _bossHpBar.value = _bossHp;
            _bossHPText.text = _bossHp.ToString();
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
        if (_playerHpBar != null)
            _playerHpBar.value = _playerHp;


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
            _stars = 3;
        } else if (_playerHp == 2)
        {
            _stars = 2;
        }
        else if (_playerHp == 2)
        {
            _stars = 1;
        }

        if (GameManager.Instance != null && _levelConfig != null)
        {
            GameManager.Instance.PlayerData.UpdateLevelProgress(_islandConfig.islandId, _levelConfig.levelId, _stars, 0f, _levelConfig.baseCoinsReward, _levelConfig.baseXpReward);
            SaveManager.Instance.Save(GameManager.Instance.PlayerData);
        }
        BattleResultHolder.IsWin = true;

        SceneController.Instance.LoadScene("Results");
    }

    private void OnBattleLose()
    {
        _isBattleActive = false;
        BattleResultHolder.IsWin = false;
        SceneController.Instance.LoadScene("Results");
    }
}
