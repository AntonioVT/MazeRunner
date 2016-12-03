using UnityEngine;
using System;
using System.Collections;

public class monsterSpawner : MonoBehaviour {

	public GameObject Crawler;
	public GameObject JavaFace;
	public GameObject RippedDude;

	// Use this for initialization
	void Start () {
		SpawnMonsters();
	}
	
	void SpawnMonsters(){
		GameObject[] allMonsters = { Crawler, JavaFace, RippedDude };
		GameObject randomMonster = allMonsters[UnityEngine.Random.Range(0, allMonsters.Length)];
		GameObject newMonster = UnityEngine.Object.Instantiate(randomMonster, transform.position, Quaternion.identity) as GameObject;
		float monsterScale = (float) UnityEngine.Random.Range(1, 5);
		newMonster.transform.localScale = new Vector3(monsterScale, monsterScale, monsterScale);
		FollowPlayer fp = newMonster.GetComponent<FollowPlayer>();
		fp.player = GameManager.Instance.goPlayer.transform;
		NavMeshAgent nva = newMonster.GetComponent<NavMeshAgent>();
		nva.speed = 11.0f - (monsterScale * 2);
		
		Wait (5.0f, () => {
			SpawnMonsters();
		});
	}
	
	void Wait(float seconds, Action action){
		StartCoroutine(_wait(seconds, action));
	}
	IEnumerator _wait(float time, Action callback){
		yield return new WaitForSeconds(time);
		callback();
	}

}
