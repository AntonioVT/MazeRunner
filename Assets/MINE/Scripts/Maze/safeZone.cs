using UnityEngine;
using System.Collections;

public class safeZone : MonoBehaviour {

	public GameObject mazeWallSZ;

	void Start () {
		Vector3 myPosition = transform.position;
		Vector3 myScale = transform.localScale;
		Vector3 mazeWallSZScale = mazeWallSZ.transform.localScale;
		
		float x = myPosition.x - myScale.x/2.0f;
		float z = myPosition.z - myScale.z/2.0f;
		
		float xx = myPosition.x + myScale.x/2.0f;
		float zz = myPosition.z + myScale.z/2.0f;
		
		int times = (int) Mathf.Ceil(myScale.x/mazeWallSZScale.x);
		float xA = 0.0f;
		
		for (int i = 0; i < times; i++, xA+=4)
		{
			if (!(i > (times/2.0f)-3 && i < (times/2.0f)+3))
			{
				UnityEngine.Object.Instantiate(mazeWallSZ, new Vector3(xx-xA, mazeWallSZScale.y/2.0f, zz), Quaternion.identity);
				UnityEngine.Object.Instantiate(mazeWallSZ, new Vector3(x+xA, mazeWallSZScale.y/2.0f, z), Quaternion.identity);
				UnityEngine.Object.Instantiate(mazeWallSZ, new Vector3(x, mazeWallSZScale.y/2.0f, z+xA), Quaternion.identity);
				UnityEngine.Object.Instantiate(mazeWallSZ, new Vector3(xx, mazeWallSZScale.y/2.0f, zz-xA), Quaternion.identity);
			}
		}
		UnityEngine.Object.Instantiate(mazeWallSZ, new Vector3(xx-xA, mazeWallSZScale.y/2.0f, zz), Quaternion.identity);
		UnityEngine.Object.Instantiate(mazeWallSZ, new Vector3(x+xA, mazeWallSZScale.y/2.0f, z), Quaternion.identity);
	}
}
