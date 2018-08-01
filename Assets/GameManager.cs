using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject originalObject;
    public static int N = 50;
    public static GameObject[] birds = new GameObject[N];

    GameObject cam;

    void Start () {
		for(int i = 0; i < N; i++)
        {
            float x = Random.Range(0.0f, 12.0f);
            float y = Random.Range(0.0f, 3f);
            birds[i] = Instantiate(originalObject, new Vector2(x, y), Quaternion.identity);
        }
	}
	
	// Update is called once per frame
	void Update () {
        float averageVx = 0;
        float averageVy = 0;
		foreach(GameObject bird in birds)
        {
            bird.GetComponent<BirdManager>().ClearVector();
            averageVx += bird.GetComponent<BirdManager>().GetVx();
            averageVy += bird.GetComponent<BirdManager>().GetVy();
        }
        averageVx /= N;
        averageVy /= N;
        foreach (GameObject bird in birds)
        {
            bird.GetComponent<BirdManager>().Rotate(GetCenter());
        }

        if (Input.GetMouseButtonDown(0))
        {
            BirdManager.SetCenter(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
	}

    public static Vector3 GetCenter()
    {
        float centerX = 0;
        float centerY = 0;
        foreach (GameObject bird in birds)
        {
            centerX += bird.transform.position.x;
            centerY += bird.transform.position.y;
        }
        centerX /= N;
        centerY /= N;
        return new Vector3(centerX, centerY,0);
    }
}
