using System.Collections.Generic;
using UnityEngine;

namespace Game.Data
{
    // 1. Базовый сериализуемый словарь
    [System.Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        public List<TKey> Keys = new List<TKey>();
        public List<TValue> Values = new List<TValue>();

        [System.NonSerialized] 
        private Dictionary<TKey, TValue> _dictionary;

        public Dictionary<TKey, TValue> Dictionary
        {
            get
            {
                if (_dictionary == null)
                {
                    _dictionary = new Dictionary<TKey, TValue>();
                    UpdateDictionary();
                }
                return _dictionary;
            }
        }

        private void UpdateDictionary()
        {
            _dictionary.Clear();
            for (int i = 0; i < Keys.Count && i < Values.Count; i++)
            {
                _dictionary[Keys[i]] = Values[i];
            }
        }

        public void UpdateLists()
        {
            // Важно: проверяем, что словарь инициализирован
            if (_dictionary == null)
            {
                // Если словаря нет, инициализируем его из текущих списков
                _ = Dictionary; // Это вызовет инициализацию через свойство
                return;
            }

            Keys.Clear();
            Values.Clear();

            foreach (var kvp in _dictionary)
            {
                Keys.Add(kvp.Key);
                Values.Add(kvp.Value);
            }
        }

        public void Initialize()
        {
            if (_dictionary == null)
            {
                _dictionary = new Dictionary<TKey, TValue>();
                UpdateDictionary();
            }
        }
    }

    // 2. Конкретные типы словарей
    [System.Serializable] public class LevelStarsDict : SerializableDictionary<string, int> { }
    [System.Serializable] public class UnlockedLevelsDict : SerializableDictionary<string, bool> { }
    [System.Serializable] public class InventoryDict : SerializableDictionary<string, int> { }

    // 3. Класс для хранения статистики уровня
    [System.Serializable]
    public class LevelStats
    {
        public string LevelId;
        public int Stars; // 0-3
        public float BestTime;
        public int Attempts;
        public bool IsCompleted;
    }
}