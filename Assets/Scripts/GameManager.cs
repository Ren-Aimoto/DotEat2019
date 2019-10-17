using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	//ゴールに必要な点数
	private const int gameclearscore = 200;

	private int currentscore = 0; //現在のスコア＝摂取たんぱく質量
	private int dotpoint = 10; //dot一つあたりの点数
    private float PlayerWeight = 80.0f; //プレイヤーの初期体重
    private int NecessaryDotNum = 100; //体重を１キロ落とすのに必要なスコア

    //妨害アイテムによるスコア影響
    private float EnemyScoreEffect = 1f;
    private float BurgerScoreEffect = 0.5f;
    private float PizzaScoreEffect = 0.8f;
    private float RiceScoreEffect = 0.3f;
    

	//生成するゴール
	public GameObject goal;
	//プレイヤーオブジェクト
	public GameObject player;
    //摂取たんぱく質のテキストUI
    public GameObject ProteinText;
    //現在体重表示テキスト
    public GameObject WeightText;
    //経過時間表示テキスト
    public GameObject TimeText;

	public enum GameStatus
	{
		CLEAR,
		FAILED,
		onGoing,
		CanGoal,
	}

	GameStatus currentstatus = GameStatus.onGoing;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.TimeText.GetComponent<Text>().text = "経過時間" + Time.time;

			if (currentscore >= gameclearscore) {
				currentstatus = GameStatus.CanGoal;
				goal.SetActive(true);
			}


            
		}
		



	public void ScoreUpdate(){
		currentscore += dotpoint;
		Debug.Log (currentscore);
        this.ProteinText.GetComponent<Text>().text = "摂取たんぱく質 : " + currentscore + "g";

        if(currentscore >= NecessaryDotNum)
        {
            PlayerWeight--;
            NecessaryDotNum += 100;
        }

        this.WeightText.GetComponent<Text>().text = "現在の体重 : " + PlayerWeight + "kg";
    }

 
    //プレイヤーとの衝突検知で得点の増減を行う
    public void EnemyCollision()
    {
        PlayerWeight += EnemyScoreEffect;
        this.WeightText.GetComponent<Text>().text = "現在の体重 : " + PlayerWeight + "kg";
    }

    public void BurgerCollision()
    {
        PlayerWeight += BurgerScoreEffect;
        this.WeightText.GetComponent<Text>().text = "現在の体重 : " + PlayerWeight + "kg";
    }

    public void PizzaCollision()
    {
        PlayerWeight += PizzaScoreEffect;
        this.WeightText.GetComponent<Text>().text = "現在の体重 : " + PlayerWeight + "kg";
    }

    public void RiceCollision()
    {
        PlayerWeight += RiceScoreEffect;
        this.WeightText.GetComponent<Text>().text = "現在の体重 : " + PlayerWeight + "kg";
    }

    public void ItemRegenerator()
    {

    }



}
