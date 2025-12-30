using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using System;
using System.Collections.Generic;

namespace Core.TextUtil
{
    public enum TextType
    {
        Common,
        UI,
        Dialogue
    }

    public sealed class TextUtil
    {
        private static TextUtil _instance;
        public static TextUtil Instance => _instance ??= new TextUtil();

        private readonly Dictionary<string, StringTable> _tableCache = new();
        private bool _initialized;

        private static readonly Dictionary<TextType, string> _tableNameCache =
            new()
            {
                { TextType.Common,   "CommonTextTable" },
                { TextType.UI,       "UITextTable" },
                { TextType.Dialogue, "DialogueTextTable" },
            };

        private static readonly Dictionary<Type, Dictionary<Enum, string>> _enumNameCache
            = new();

        private TextUtil() { }

        public void Initialize()
        {
            if (_initialized)
                return;

            _ = LocalizationSettings.AvailableLocales;
            _ = LocalizationSettings.StringDatabase;

            _initialized = true;
        }

        private StringTable GetTable(TextType type)
        {
            return GetTable(_tableNameCache[type]);
        }

        private StringTable GetTable(string tableName)
        {
            if (!_initialized)
            {
                Debug.LogError("TextUtil not initialized");
                return null;
            }

            if (_tableCache.TryGetValue(tableName, out var table))
                return table;

            table = LocalizationSettings.StringDatabase.GetTable(tableName);
            _tableCache[tableName] = table;
            return table;
        }

        public string GetText(TextType type, string entryKey)
        {
            var table = GetTable(type);
            return table?.GetEntry(entryKey)?.GetLocalizedString()
                   ?? $"#{entryKey}";
        }

        public string Get<TEnum>(TextType type, TEnum id, string field)
            where TEnum : Enum
        {
            // entryKey = Category.Id.Field
            string enumName = GetEnumName(id);
            string entryKey = enumName + "." + field;

            return GetText(type, entryKey);
        }

        private static string GetEnumName<TEnum>(TEnum value)
            where TEnum : Enum
        {
            var type = typeof(TEnum);

            if (!_enumNameCache.TryGetValue(type, out var map))
            {
                map = new Dictionary<Enum, string>();
                _enumNameCache[type] = map;
            }

            if (!map.TryGetValue(value, out var name))
            {
                name = value.ToString();
                map[value] = name;
            }

            return name;
        }

        public void ClearCache()
        {
            _tableCache.Clear();
        }
    }
}
