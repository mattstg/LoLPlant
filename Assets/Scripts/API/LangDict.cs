using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class LangDict {

    #region Singleton
    private static LangDict instance;

    private LangDict() { }

    public static LangDict Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LangDict();
            }
            return instance;
        }
    }
    #endregion

    
    public string languageSelected = "en";
    JSONNode langNode;

    public void SetLanguage(string lang)
    {
        languageSelected = lang;
    }

    public void SetNode(JSONNode js)
    {
        langNode = js;
    }

    public string GetText(string keyName)
    {
        try
        {
            return langNode[languageSelected][keyName].Value;
        }
        catch
        {
            return "error, no text found for key: " + keyName + " in langauge: " + languageSelected;
        }
        
    }
}
