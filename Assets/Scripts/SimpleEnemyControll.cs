using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyControll : MonoBehaviour {

    public GameObject SimpleEnemy;

    private float movespeed = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        EnemyMove();
    }

    void EnemyMove()
    {
        if (transform.position.x >= 4.5f)
        {
            this.transform.position = new Vector3(this.transform.position.x - movespeed, this.transform.position.y, this.transform.position.z);
        } else if(this.transform.position.x <= -4.5f){
            this.transform.position = new Vector3(this.transform.position.x + movespeed, this.transform.position.y, this.transform.position.z);
        }
        else
        {

        }
       
    }
}
