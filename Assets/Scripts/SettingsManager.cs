using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameTMP;
    [SerializeField] private Toggle addToogle;
    [SerializeField] private TextMeshProUGUI addMaxFactorTMP1;
    [SerializeField] private TextMeshProUGUI addMaxFactorTMP2;
    [SerializeField] private Toggle subToogle;
    [SerializeField] private TextMeshProUGUI subMaxFactorTMP1;
    [SerializeField] private TextMeshProUGUI subMaxFactorTMP2;
    [SerializeField] private Toggle multiplyToogle;
    [SerializeField] private TextMeshProUGUI multyMaxFactorTMP1;
    [SerializeField] private TextMeshProUGUI multyMaxFactorTMP2;
    [SerializeField] private Toggle divToogle;
    [SerializeField] private TextMeshProUGUI divMaxFactorTMP1;
    [SerializeField] private TextMeshProUGUI divMaxFactorTMP2;
    [SerializeField] private PopupManager popupManager;
    [SerializeField] private TMP_InputField tmpInputField;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private Slider musicSlider;

    private int addMaxFactor1 = 10;
    private int addMaxFactor2 = 10;
    private int subMaxFactor1 = 20;
    private int subMaxFactor2 = 10;
    private int multyMaxFactor1 = 10;
    private int multyMaxFactor2 = 10;
    private int divMaxFactor1 = 100;
    private int divMaxFactor2 = 10;

    private void Start()
    {
        LoadSettings();
        SaveSettings();
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        addToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "AddBool"));
        subToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "SubBool"));
        multiplyToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "MultyBool"));
        divToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "DivBool"));
        playerName.text = PlayerPrefs.GetString("PlayerName", "Ghost");
        tmpInputField.onEndEdit.AddListener(OnInputEndEdit);
    }
    // Сохранение настроек
    public void SaveSettings()
    {
        PlayerPrefs.Save();
        LoadSettings();
    }

    private void OnInputEndEdit(string finalText)
    {
        if (finalText != "")
        {
            Debug.Log($"Ввод завершен: {finalText}");
            PlayerPrefs.SetString("PlayerName", finalText);
            SaveSettings();
        }
    }

    // Загрузка настроек
    public void LoadSettings()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        playerNameTMP.text = PlayerPrefs.GetString("PlayerName", "Ghost");
        playerName.text = PlayerPrefs.GetString("PlayerName", "Ghost");
        int maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        addToogle.isOn = PlayerPrefs.GetInt("AddBool", 1) == 1;
        addMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstAdd", addMaxFactor1).ToString();
        addMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondAdd", addMaxFactor2).ToString();
        subToogle.isOn = PlayerPrefs.GetInt("SubBool", 1) == 1;
        subMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstSub", subMaxFactor1).ToString();
        subMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondSub", subMaxFactor2).ToString();
        multiplyToogle.isOn = PlayerPrefs.GetInt("MultyBool", 1) == 1;
        multyMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstMulty", multyMaxFactor1).ToString();
        multyMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondMulty", multyMaxFactor2).ToString();
        divToogle.isOn = PlayerPrefs.GetInt("DivBool", 1) == 1;
        divMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstDiv", divMaxFactor1).ToString();
        divMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondDiv", divMaxFactor2).ToString();
    }

    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        LoadSettings();
    }

    public void IncrementValue(string fieldname)
    {
        int value = 0;
        switch (fieldname)
        {
            case "FirstAdd":
                value = PlayerPrefs.GetInt(fieldname, addMaxFactor1);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "FirstSub":
                value = PlayerPrefs.GetInt(fieldname, subMaxFactor1);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "FirstMulty":
                value = PlayerPrefs.GetInt(fieldname, multyMaxFactor1);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "FirstDiv":
                value = PlayerPrefs.GetInt(fieldname, divMaxFactor1);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondAdd":
                value = PlayerPrefs.GetInt(fieldname, addMaxFactor2);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondSub":
                value = PlayerPrefs.GetInt(fieldname, subMaxFactor2);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondMulty":
                value = PlayerPrefs.GetInt(fieldname, multyMaxFactor2);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondDiv":
                value = PlayerPrefs.GetInt(fieldname, divMaxFactor2);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
        }
        Debug.Log($"значение {fieldname} увеличилось на 1. Текущее значение {value}");
        SaveSettings();
    }

    public void DecrimentValue(string fieldname)
    {
        int value = 0;
        switch (fieldname)
        {
            case "FirstAdd":
                value = PlayerPrefs.GetInt(fieldname, addMaxFactor1);
                if (value > 1)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 0");
                }
                break;
            case "FirstSub":
                value = PlayerPrefs.GetInt(fieldname, subMaxFactor1);
                if (value > 1)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 0");
                }
                break;
            case "FirstMulty":
                value = PlayerPrefs.GetInt(fieldname, multyMaxFactor1);
                if (value > 1)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 0");
                }
                break;
            case "FirstDiv":
                value = PlayerPrefs.GetInt(fieldname, divMaxFactor1);
                if (value > 1)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 0");
                }
                break;
            case "SecondAdd":
                value = PlayerPrefs.GetInt(fieldname, addMaxFactor2);
                if (value > 1)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 0");
                }
                break;
            case "SecondSub":
                value = PlayerPrefs.GetInt(fieldname, subMaxFactor1);
                if (value > 1)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 0");
                }
                break;
            case "SecondMulty":
                value = PlayerPrefs.GetInt(fieldname, multyMaxFactor2);
                if (value > 1)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 0");
                }
                break;
            case "SecondDiv":
                value = PlayerPrefs.GetInt(fieldname, divMaxFactor2);
                if (value > 2)
                {
                    value--;
                    PlayerPrefs.SetInt(fieldname, value);
                }
                else
                {
                    popupManager.ErrorMsg("Значение не должно быть меньше 1");
                }
                break;
        }
        Debug.Log($"значение {fieldname} уменьшилось на 1. Текущее значение {value}");
        SaveSettings();
    }

    private void OnAnyToggleChanged(bool isOn, string playerPrefsKey)
    {
        // Сохраняем состояние текущего Toggle
        PlayerPrefs.SetInt(playerPrefsKey, isOn ? 1 : 0);

        // Проверяем, что хотя бы один Toggle включен
        if (!IsAnyToggleOn())
        {
            // Если ни один не включен - включаем текущий обратно
            SetToggleState(playerPrefsKey, true);
            Debug.LogWarning("Должен быть включен хотя бы один Toggle!");
            popupManager.ErrorMsg("Должен быть включен хотя бы один Toggle!");
        }
    }

    private bool IsAnyToggleOn()
    {
        return (addToogle.isOn || subToogle.isOn || multiplyToogle.isOn || divToogle.isOn);
    }

    private void SetToggleState(string playerPrefsKey, bool state)
    {
        PlayerPrefs.SetInt(playerPrefsKey, state ? 1 : 0);

        // Обновляем визуальное состояние Toggle
        switch (playerPrefsKey)
        {
            case "AddBool":
                addToogle.isOn = state;
                break;
            case "SubBool":
                subToogle.isOn = state;
                break;
            case "MultyBool":
                multiplyToogle.isOn = state;
                break;
            case "DivBool":
                divToogle.isOn = state;
                break;
        }
    }

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        AudioListener.volume = value;

        Debug.Log("Громкость звуков "+value*100+"%");
    }
}
