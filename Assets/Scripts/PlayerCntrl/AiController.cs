using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiController : PlayerController {
	float runDir = 1;
	float jumpTimer = 0;
	float jumpHold = 0.8f;
	float mapBoundary = 7.5f;

	public override void Refresh (float dt)
	{
		Act(dt);
		anim.Refresh(dt);
		timeSinceInitialJumpBurst += dt;
		if (isGrounded)
			body.drag = 0.5f;
		else
			body.drag = 0f;
	}

	private void Act(float dt){
		if ((transform.position.x > mapBoundary && runDir == 1) || (transform.position.x < -mapBoundary && runDir == -1)) {
			runDir *= -1;
		}
		Move (runDir,dt);
		if (Random.Range (0f, 1) <= 1/(2.5/dt) || (jumpTimer > 0)) {
			jumpTimer += dt;
			Jump (1, dt);
			if (jumpTimer > jumpHold)
				jumpTimer = 0;
		}
	}


}
