using UnityEngine;
using System.Collections;
using System;

public class connectMaze : MonoBehaviour
{
	private bool alreadyDead = false;

	void Start()
	{
		GameObject Manager = GameObject.Find("MazeManager");
		
		for (int i = 0; i < Manager.GetComponent<mazeSpawner>().connectMazePos.Count; i++)
		{
			if(Manager.GetComponent<mazeSpawner>().connectMazePos[i].x == transform.position.x &&
				Manager.GetComponent<mazeSpawner>().connectMazePos[i].y == transform.position.z)
				{
					Destroy(gameObject);
				}
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		if (!alreadyDead && collision.gameObject.name == "Cube(Clone)" ) {
			connectMaze script = collision.gameObject.GetComponent<connectMaze>();
			if (script != null) {
				 script.alreadyDead = true;
				 Destroy(collision.gameObject);
			}
		}
		
		if ( collision.gameObject.name == "SafeZone(Clone)" )
		{
			Destroy(gameObject);
		}
	}
}
