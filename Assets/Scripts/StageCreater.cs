using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageCreater : MonoBehaviour {

    //Itemクラスの宣言
    public class Item
    {
        public int xPosition;
        public int zPosition;

        public Item(int x, int z)
        {
            this.xPosition = x;
            this.zPosition = z;
        }
    }

    //Itemリストの宣言
    public List<Item> ItemList = new List<Item>();

    //ハンバーガーの最大生成数
    private int ItemLimit = 4;


    //z方向の最大値/最小値(int型にするため整数値、本来はかける0.5でfloatにキャストしてposition化する。)
    private int MaxVertical = 9;
    private int MinVertical = -9;

    //x方向の最大値/最小値
    private int MaxHorizontal = 9;
    private int MinHorizontal = -8;
    //ItemのY軸位置
    private float posY = 0.7f;

    //経過時間の変数
    private float seconds = 0f;
    //妨害アイテム生成の基準時間
    private float GenerateTime = 10f;
    //Dotの自動生成時間
    private float DotGenerationTime = 5f;

    //Burgerオブジェクトの割当
    public GameObject BurgerPrefab;
    //pizzaオブジェクト
    public GameObject PizzaPrefab;
    //ご飯オブジェクト
    public GameObject RicePrefab;

    //Dotオブジェクトの割当
    public GameObject DotPrefab;


	// Use this for initialization
	void Start () {
        //ハンバーガー生成
        for(int i = 0; i< ItemLimit; i++)
        {
            int ItemDecider = Random.Range(1, 11);
            if (ItemDecider >= 2 && ItemDecider <= 5)
            { //ハンバーガー生成

                int X = Random.Range(-8, 9);
                int Z = Random.Range(-8, 9);

                float posX = X * 0.5f;
                float posZ = Z * 0.5f;

                Instantiate(BurgerPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                ItemList.Add(new Item(X, Z));
            }
            if (ItemDecider > 5 && ItemDecider <= 8)
            { //Pizza生成

                int X = Random.Range(-8, 9);
                int Z = Random.Range(-8, 9);

                float posX = X * 0.5f;
                float posZ = Z * 0.5f;

                Instantiate(PizzaPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                ItemList.Add(new Item(X, Z));
            }
            if (ItemDecider > 8 && ItemDecider <= 11)
            { //ご飯生成

                int X = Random.Range(-8, 9);
                int Z = Random.Range(-8, 9);

                float posX = X * 0.5f;
                float posZ = Z * 0.5f;

                Instantiate(RicePrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                ItemList.Add(new Item(X, Z));
            }

        }
        
        //Dot生成
        for (int Z = MinVertical; Z < MaxVertical; Z += 1)
        {
            for (int X = MinHorizontal; X < MaxHorizontal; X += 1)
            {
                float posX = X * 0.5f;
                float posZ = Z * 0.5f;

                if (!ItemExists(X, Z))
                {
                    Instantiate(DotPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                    ItemList.Add(new Item(X,Z));
                }

            }
        }

    }
	
	// Update is called once per frame
	void Update () {
        //時間経過とともにドットとフード生成
        seconds = Time.deltaTime;
        if(seconds >= GenerateTime)
        {
            int X = Random.Range(-8, 9);
            int Z = Random.Range(-8, 9);

            float posX = X * 0.5f;
            float posZ = Z * 0.5f;

            if (!ItemExists(X, Z))
            {

                Instantiate(BurgerPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                ItemList.Add(new Item(X, Z));
            }

            GenerateTime += 15f;

        }

        if (seconds >= DotGenerationTime)
        {
            int X = Random.Range(-8, 9);
            int Z = Random.Range(-8, 9);

            float posX = X * 0.5f;
            float posZ = Z * 0.5f;

            if (!ItemExists(X, Z))
            {

                Instantiate(DotPrefab, new Vector3(posX, posY, posZ), Quaternion.identity);
                ItemList.Add(new Item(X, Z));
            }

            DotGenerationTime += 5f;
        }
    }

    private bool ItemExists(int x, int z)
    {
        for(int i = 0; i<ItemList.Count; i++)
        {
            if(ItemList[i].xPosition == x && ItemList[i].zPosition== z)
            {
                return true;
            }
        }
        return false;
    }
}


   