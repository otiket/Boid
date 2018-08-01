using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoBehaviour {

    private static Vector2 center = Vector2.zero;

    private static float r1 = 1;
    private static float r2 = 0.8f;
    private static float r3 = 0.1f;
    private static int CENTER_PULL_FACTOR = 600;
    private static int DIST_THRESHOLD = 1;

    private static float SPEED = 0.2f;

    private float vx = 0;
    private float vy = 0;

    private Vector2 v1 = Vector2.zero;
    private Vector2 v2 = Vector2.zero;
    private Vector2 v3 = Vector2.zero;

    public bool isMouseUsed = false;
    private GameObject learderBird;

    private Vector3 topLeft;
    private Vector3 bottomRight;
    void Start () {
        topLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topLeft.Scale(new Vector3(1, -1, 1));
        bottomRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        bottomRight.Scale(new Vector3(1, -1, 1));

        learderBird = GameObject.Find("Player");
    }

    public static void SetCenter(Vector2 newCenter)
    {
        center = newCenter;
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

    public void Rotate(Vector3 targetPosition)
    {
        //var pos = Camera.main.WorldToScreenPoint(transform.localPosition);
        //var rotation = Quaternion.LookRotation(Vector3.forward, Input.mousePosition - pos);
        //transform.localRotation = rotation;

        var vec = (learderBird.transform.position - this.transform.position).normalized;
        var angle = (Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
        if(vec.x > 0)
        {
            this.transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        }
        else
        {
            angle -= 180.0f;
            this.transform.rotation = Quaternion.Euler(0.0f, 180.0f, angle);
        }
        
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
        
        /*if (x <= topLeft.x)
        {
            this.transform.position = new Vector2(topLeft.x, y);
            vx *= -1;
        }
        if (x >= bottomRight.x)
        {
            this.transform.position = new Vector2(bottomRight.x, y);
            vx *= -1;
        }

        if (y <= topLeft.y)
        {
            this.transform.position = new Vector2(x, topLeft.y);
            vy *= -1;
        }
        if (y >= bottomRight.y)
        {
            this.transform.position = new Vector2(x, bottomRight.y);
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
        if (isMouseUsed)
        {
            center = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            v1.x = (center.x - this.transform.position.x) / CENTER_PULL_FACTOR;
            v1.y = (center.y - this.transform.position.y) / CENTER_PULL_FACTOR;
        }
        else
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

            //v1.x = (v1.x - this.transform.position.x) / CENTER_PULL_FACTOR;
            //v1.y = (v1.y - this.transform.position.y) / CENTER_PULL_FACTOR;
            v1.x = (learderBird.transform.position.x - this.transform.position.x) / CENTER_PULL_FACTOR;
            v1.y = (learderBird.transform.position.y - this.transform.position.y) / CENTER_PULL_FACTOR;
        }
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
