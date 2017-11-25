using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPC : PlayerController {

    bool setActive = true;

    public void SetCharActive(bool _setActive)
    {
        setActive = _setActive;
    }

    public override void Refresh(float dt)
    {
        if(setActive)
            base.Refresh(dt);
    }
}
