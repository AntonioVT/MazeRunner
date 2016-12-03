using UnityEngine;
using System.Collections;
using System;

public class elevateWall : MonoBehaviour
{
	private float elevateTime = 0.0f;
	
	void Start()
	{
		Vector3 center  = new Vector3(162, 0, 162);
		Vector3 further = new Vector3(325, 0,   0);
		Vector3 myPosition = transform.position;
		
		float distMax = Vector3.Distance(center, further);
		float myDist = Vector3.Distance(center, myPosition);
		
		float maxTime = 6.0f;
		
		elevateTime = maxTime - (maxTime * myDist) / distMax;
		
		Wait (elevateTime, () => {
			StartCoroutine(elevateWallCoroutine());
		});
	}
	
	void Wait(float seconds, Action action){
		StartCoroutine(_wait(seconds, action));
	}
	IEnumerator _wait(float time, Action callback){
		yield return new WaitForSeconds(time);
		callback();
	}
	
	IEnumerator elevateWallCoroutine(){
		float yDelta  = 0.175f;
		float maxPosY = transform.localScale.y / 2.0f;
		
		Vector3 posOriginal = transform.position;
		
		for(float newPosY = posOriginal.y;  newPosY <= maxPosY ; newPosY += yDelta){
			transform.position = new Vector3(posOriginal.x, newPosY, posOriginal.z);
			yield return new WaitForSeconds(0.01f);
		}
	}
	
	
}
