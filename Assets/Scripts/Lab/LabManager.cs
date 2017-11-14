using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabManager {

    #region Singleton
    private static LabManager instance;

    private LabManager() { }

    public static LabManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LabManager();
            }
            return instance;
        }
    }
    #endregion

}
