using UnityEngine;
public class LocalStorage
{

    public static bool isEventEnable
    {
        get
        {
            string sKey = "seasonal_event";
            if (!MyPlayerPrefs.HasKey(sKey))
            {
                MyPlayerPrefs.SetBool(sKey, false);
            }
            return MyPlayerPrefs.GetBool(sKey);
        }
        set
        {
            string sKey = "seasonal_event";
            MyPlayerPrefs.SetBool(sKey, value);
        }
    }

    public static string KartColor
    {
        get
        {
            string sKey = "kart_color";
            if (!MyPlayerPrefs.HasKey(sKey))
            {
                MyPlayerPrefs.SetString(sKey, "FF0000");
            }
            return MyPlayerPrefs.GetString(sKey);
        }
        set
        {
            string sKey = "kart_color";
            MyPlayerPrefs.SetString(sKey, value);
        }
    }


    public static int Coins
    {
        get
        {
            string sKey = "coins";
            return MyPlayerPrefs.GetInt(sKey);
        }
        set
        {
            string sKey = "coins";
            MyPlayerPrefs.SetInt(sKey, value);
        }
    }

    public  class MyPlayerPrefs
    {
        public static bool HasKey(string sKey)
        {
            return PlayerPrefs.HasKey(sKey);
        }
        public static void SetInt(string sKey, int iValue)
        {
            PlayerPrefs.SetInt(sKey, iValue);
        }
        public static int GetInt(string sKey)
        {
            return PlayerPrefs.GetInt(sKey);
        }
        public static void SetString(string sKey, string sValue)
        {
            PlayerPrefs.SetString(sKey, sValue);
        }
        public static string GetString(string sKey)
        {
            return PlayerPrefs.GetString(sKey);
        }
        public static void SetBool(string sKey, bool bValue)
        {
            int iValue = bValue ? 1 : 0;
            PlayerPrefs.SetInt(sKey, iValue);
        }
        public static bool GetBool(string sKey)
        {
            int iValue = PlayerPrefs.GetInt(sKey);
            return iValue == 1 ? true : false;
        }

        public static void DeleteAllKeys()
        {
            PlayerPrefs.DeleteAll();
        }
    }

}