using System;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class GameSettings
    {
        // Настройки звука
        [Range(0f, 1f)]
        public float masterVolume = 0.8f;

        [Range(0f, 1f)]
        public float musicVolume = 0.7f;

        [Range(0f, 1f)]
        public float sfxVolume = 0.8f;
        public bool musicEnabled = true;
        public bool sfxEnabled = true;



        // Язык и интерфейс
        public string language = "en";
        public bool showTutorial = true;
        public bool showDamageNumbers = true;
        public bool showSubtitles = true;


        // Конструктор с настройками по умолчанию
        public GameSettings()
        {
            // Можно установить начальные значения здесь
            // или оставить значения по умолчанию выше
        }

        // Метод для сброса к настройкам по умолчанию
        public void ResetToDefaults()
        {
            masterVolume = 0.8f;
            musicVolume = 0.7f;
            sfxVolume = 0.8f;
            musicEnabled = true;
            sfxEnabled = true;
            language = "en";
            showTutorial = true;
        }

        // Метод для проверки валидности настроек
        public bool ValidateSettings()
        {
            // Проверяем, что значения в допустимых диапазонах
            if (masterVolume < 0f || masterVolume > 1f) masterVolume = 0.8f;
            if (musicVolume < 0f || musicVolume > 1f) musicVolume = 0.7f;
            if (sfxVolume < 0f || sfxVolume > 1f) sfxVolume = 0.8f;

            return true;
        }
    }
}
