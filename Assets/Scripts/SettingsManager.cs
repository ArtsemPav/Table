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

    private void Start()
    {
        LoadSettings();
        SaveSettings();
        addToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "AddBool"));
        subToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "SubBool"));
        multiplyToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "MultyBool"));
        divToogle.onValueChanged.AddListener((value) => OnAnyToggleChanged(value, "DivBool"));
        tmpInputField.onEndEdit.AddListener(OnInputEndEdit);
    }
    // Сохранение настроек
    private void SaveSettings()
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
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        playerNameTMP.text = PlayerPrefs.GetString("PlayerName", "Ghost");
        int maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        addToogle.isOn = PlayerPrefs.GetInt("AddBool", 1) == 1;
        addMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstAdd", 11).ToString();
        addMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondAdd", 11).ToString();
        subToogle.isOn = PlayerPrefs.GetInt("SubBool", 1) == 1;
        subMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstSub", 21).ToString();
        subMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondSub", 11).ToString();
        multiplyToogle.isOn = PlayerPrefs.GetInt("MultyBool", 1) == 1;
        multyMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstMulty", 11).ToString();
        multyMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondMulty", 11).ToString();
        divToogle.isOn = PlayerPrefs.GetInt("DivBool", 1) == 1;
        divMaxFactorTMP1.text = PlayerPrefs.GetInt("FirstDiv", 101).ToString();
        divMaxFactorTMP2.text = PlayerPrefs.GetInt("SecondDiv", 11).ToString();
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
                value = PlayerPrefs.GetInt(fieldname, 11);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "FirstSub":
                value = PlayerPrefs.GetInt(fieldname, 11);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "FirstMulty":
                value = PlayerPrefs.GetInt(fieldname, 11);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "FirstDiv":
                value = PlayerPrefs.GetInt(fieldname, 11);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondAdd":
                value = PlayerPrefs.GetInt(fieldname, 11);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondSub":
                value = PlayerPrefs.GetInt(fieldname, 11);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondMulty":
                value = PlayerPrefs.GetInt(fieldname, 11);
                value++;
                PlayerPrefs.SetInt(fieldname, value);
                break;
            case "SecondDiv":
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
                value = PlayerPrefs.GetInt(fieldname, 11);
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
        return PlayerPrefs.GetInt("AddBool", 1) == 1 ||
               PlayerPrefs.GetInt("SubBool", 0) == 1 ||
               PlayerPrefs.GetInt("MultyBool", 0) == 1 ||
               PlayerPrefs.GetInt("DivBool", 0) == 1;
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
}
