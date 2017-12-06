using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TAEventTimer : MonoBehaviour {

    //This is not a TA class, but the one that is attached to the gameobject to countdown in the scene
    bool initialized = false;
    float time;
    string eventName;

	public void Initialize(float _time, string _eventName)
    {
        initialized = true;
        eventName = _eventName;
        time = _time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!initialized)
            return;

        time -= Time.deltaTime;
        if(time <= 0)
        {
            TAEventManager.Instance.ReceiveActionTrigger(eventName);
            Debug.Log("Timer triggered");
            Destroy(this.gameObject);
        }	
	}
}
