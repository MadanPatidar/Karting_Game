using UnityEngine;
public class Util
{
    public static int Coins
    {
        get
        {
            string sKey = "coins";
            return LocalStorage.GetInt(sKey);
        }
        set
        {
            string sKey = "coins";
            LocalStorage.SetInt(sKey, value);
        }
    }

    public  class LocalStorage
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