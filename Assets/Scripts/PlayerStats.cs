using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats
{
    //
    [System.Serializable]
    public class PlayerStat
    {
        float m_value, m_max, m_min;

        public PlayerStat(float value, float min, float max)
        {
            m_min = min;
            m_max = max;
            SetValue(value);
        }

        public float Value()
        {
            return m_value;
        }
        public float MaxValue()
        {
            return m_max;
        }
        public float MinValue()
        {
            return m_min;
        }
        public void SetValue(float value)
        {
            m_value = Mathf.Clamp(value, m_min, m_max);
        }
        public void ChangeValue(float change)
        {
            m_value = Mathf.Clamp(m_value + change, m_min, m_max);
        }
    }

    //Define various player values
    static Dictionary<string, PlayerStat> m_stats;
    static List<KeyItem> m_keyItems;

    static PlayerStats()
    {
        Reset();
    }

    public static void Reset()
    {
        m_stats = new Dictionary<string, PlayerStat>();
        m_stats.Add("health", new PlayerStat(100f, 0f, 100f));
        m_stats.Add("bolts", new PlayerStat(5f, 0f, 100f));
        m_stats.Add("medkits", new PlayerStat(0f, 0f, 2f));

        m_keyItems = new List<KeyItem>();
    }

    public static float GetStat(string name)
    {
        return m_stats[name].Value();
    }
    public static bool StatMaxed(string name)
    {
        return m_stats[name].Value() == m_stats[name].MaxValue();
    }
    public static bool HasKeyItem(KeyItem requiredItem)
    {
        foreach(KeyItem item in m_keyItems)
        {
            if (item.name == requiredItem.name)
                return true;
        }
        return false;
    }
    public static bool HasMultipleKeyItems(KeyItem[] items)
    {
        foreach(KeyItem item in items)
        {
            if (!HasKeyItem(item))
                return false;
        }
        return true;
    }

    public static void SetStat(string name, float value)
    {
        m_stats[name].SetValue(value);
        Debug.Log(name + " is now " + m_stats[name].Value());
    }
    public static void ChangeStat(string name, float change)
    {
        m_stats[name].ChangeValue(change);
        Debug.Log(name + " is now " + m_stats[name].Value());
    }

    public static void AddKeyItem(KeyItem item)
    {
        m_keyItems.Add(item);
    }
    public static void RemoveKeyItem(KeyItem item)
    {
        //NOT SURE IF THIS WILL WORK BASED ON VALUE OR REFERENCE
        m_keyItems.Remove(item);
    }
}
