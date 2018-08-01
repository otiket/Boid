using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour {

    public static float r1 = 1;
    public static float r2 = 0.8f;
    public static float r3 = 0.1f;

    public static int CENTER_PULL_FACTOR = 300;
    public static int DIST_THRESHOLD = 1;

    private static float SPEED = 0.2f;

    private float vx = 0;
    private float vy = 0;

    private Vector2 v1 = Vector2.zero;
    private Vector2 v2 = Vector2.zero;
    private Vector2 v3 = Vector2.zero;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Check();
        Move();
	}
    public float GetVx()
    {
        return vx;
    }
    public float GetVy()
    {
        return vy;
    }

    public void Rotate(Vector3 averageVelocity)
    {
        this.transform.LookAt(averageVelocity);
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
        //             Quaternion.LookRotation(averageVelocity.normalized),
        //             Time.deltaTime * 3f);
    }

    void Move()
    {
        vx += r1 * v1.x + r2 * v2.x + r3 * v3.x;
        vy += r1 * v1.y + r2 * v2.y + r3 * v3.y;
        
        float vVector = Mathf.Sqrt(vx * vx + vy * vy);
        if (vVector > SPEED)
        {
            vx = (vx / vVector) * SPEED;
            vy = (vy / vVector) * SPEED;
        }

        this.transform.position += new Vector3(vx, vy, 0);


        float x = transform.position.x;
        float y = transform.position.y;
        /*if (x <= -12f)
        {
            this.transform.position = new Vector2(-12f, y);
            vx *= -1;
        }
        if (x >= 12f)
        {
            this.transform.position = new Vector2(12f, y);
            vx *= -1;
        }

        if (y <= -6f)
        {
            this.transform.position = new Vector2(x, -6f);
            vy *= -1;
        }
        if (y >= 6f)
        {
            this.transform.position = new Vector2(x, 6f);
            vy *= -1;
        }*/
    }
    

    public void ClearVector()
    {
        v1.x = 0;
        v1.y = 0;
        v2.x = 0;
        v2.y = 0;
        v3.x = 0;
        v3.y = 0;
    }
    
    void Check()
    {
        Rule1();
        Rule2();
        Rule3();
    }

    
    void Rule1()
    {
        for (int i = 0; i < GameManager.N; i++)
        {
            GameObject otherBall = GameManager.birds[i];
            if (this != otherBall.GetComponent<BirdManager>())
            {
                v1.x += otherBall.transform.position.x;
                v1.y += otherBall.transform.position.y;
            }
        }

        v1.x /= (GameManager.N - 1);
        v1.y /= (GameManager.N - 1);

        v1.x = (v1.x - this.transform.position.x) / CENTER_PULL_FACTOR;
        v1.y = (v1.y - this.transform.position.y) / CENTER_PULL_FACTOR;

    }

    void Rule2()
    {
        for (int i = 0; i < GameManager.N; i++)
        {
            GameObject otherBall = GameManager.birds[i];
            if (this != otherBall.GetComponent<BirdManager>())
            {
                if (Vector2.Distance(this.transform.position, otherBall.transform.position) < DIST_THRESHOLD)
                {
                    v2.x -= (otherBall.transform.position.x - this.transform.position.x);
                    v2.y -= (otherBall.transform.position.y - this.transform.position.y);
                }
            }
        }
    }
    
    void Rule3()
    {
        int NUMBER = GameManager.N;
        for (int i = 0; i < NUMBER; i++)
        {
            GameObject otherBall = GameManager.birds[i];
            if (this != otherBall.GetComponent<BirdManager>())
            {
                v3.x += otherBall.GetComponent<BirdManager>().GetVx();
                v3.y += otherBall.GetComponent<BirdManager>().GetVy();
            }
        }

        v3.x /= (NUMBER - 1);
        v3.y /= (NUMBER - 1);

        v3.x = (v3.x - vx) / 2;
        v3.y = (v3.y - vy) / 2;
    }
}
