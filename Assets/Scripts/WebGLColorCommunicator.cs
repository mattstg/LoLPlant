using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebGLColorCommunicator : MonoBehaviour {

    public Text cmdText;
    string currentText = "WEBGL DEBUG COMMUNICATOR";

    public void ST(string _text)
    {
        currentText += "\n" + _text;
        cmdText.text = currentText;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            cmdText.gameObject.SetActive(!cmdText.gameObject.activeInHierarchy);
    }
}
