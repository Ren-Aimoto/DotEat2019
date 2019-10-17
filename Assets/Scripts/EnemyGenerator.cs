using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour {

	//Enemyの配置最大数
	private const int MAX_ENEMYNUM = 4;

	public GameObject EnemyPrefab; //エネミープレハブの読み込み
	private float VerticalLimit = 4.0f;
	private float HorizontalLimit = 4.0f;
    private int EnemyNumber = 0;



	// Use this for initialization
	void Start () {
        for (int i = 0; i < 4; i++)
        {
            float posX = (int)Random.Range(-8, 10) * 0.5f;
            float posZ = (int)Random.Range(-8, 10) * 0.5f;
            Instantiate(EnemyPrefab, new Vector3(posX, 0, posZ), Quaternion.identity);

        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
