using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicKeys : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.A))
            TAEventManager.Instance.ReceiveActionTrigger("ClosePopup");
	}
}
