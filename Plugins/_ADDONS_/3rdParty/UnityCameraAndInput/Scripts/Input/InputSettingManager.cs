using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSettingManager : MonoBehaviour
{
    [System.Serializable]
    public struct InputSetting
    {
        public string keyName;
        public KeyCode keyCode;
    }

    public static InputSettingManager Singleton { get; protected set; }

    public InputSetting[] settings;

    internal readonly Dictionary<string, HashSet<KeyCode>> Settings = new Dictionary<string, HashSet<KeyCode>>();

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = GetComponent<InputSettingManager>();
        DontDestroyOnLoad(gameObject);

        if (settings != null && settings.Length > 0)
        {
            foreach (InputSetting setting in settings)
            {
                if (!Settings.ContainsKey(setting.keyName))
                    Settings[setting.keyName] = new HashSet<KeyCode>();
                if (!Settings[setting.keyName].Contains(setting.keyCode))
                    Settings[setting.keyName].Add(setting.keyCode);
            }
        }
    }
}
