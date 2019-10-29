using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour {

	//ゴールに必要な点数
	private const int gameclearscore = 1000;

	public int currentscore = 0; //現在のスコア＝摂取たんぱく質量
	private int dotpoint = 5; //dot一つあたりの点数
    private float PlayerWeight = 100.0f; //プレイヤーの初期体重
    private int NecessaryDotNum = 20; //体重を１キロ落とすのに必要なスコア
    private int CheatingScore = 250; //チートデイ発動条件
    public int ScoreForCheating; //チートデイ発動可能かスコア格納用

    //妨害アイテムによるスコア影響
    private float EnemyScoreEffect = 1f;
    private float BurgerScoreEffect = 0.5f;
    private float PizzaScoreEffect = 0.8f;
    private float RiceScoreEffect = 0.3f;

    //Player初期位置の座標
    private float playerPosX = -4f;
    private float playerPosY = 0.7f;
    private float playerPosZ = -4f;

    //チートデイ発動中か否か？
    public bool IsCheatDay = false;
    //チートデイの時間制限
    private float CheatTimeLimit = 5;
    //Playerのスポットライト
    public GameObject PlayerSpotlight;



    //経過時間格納
    private float time;
    //制限時間格納
    private float TimeLimit = 123f;
    //カウントイン
    private float CountIn = 3f;
    public GameObject CountInScript;

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
    //カウントイン表示テキスト
    public GameObject CountInText;
    //スコア表示テキスト
    public GameObject ScoreText;
    //クリアタイム表示テキスト
    public GameObject ClearTimeText;
    //ゴールドア
    public GameObject Door;
    //ドアを塞ぐ壁
    public GameObject outerwall;
    //カメラオブジェクト
    public GameObject mainCamera;
    //タイトルテキスト
    public GameObject TitleText;
    //ボタン群
    public GameObject PlayButton;
    public GameObject HowToButton;
    public GameObject BackButton;
    public GameObject TimeAttackButton;
    public GameObject ScoreAttackButton;
    public GameObject BacktoTitleButton;
    public GameObject TryAgainButton;
    public GameObject RankingButton;
    //Cheating Button
    public Button CheatingButton;
    public bool IsCheatingAvailable = false;
    //ゲームステータスのUIキャンバス
    public GameObject GameUICanvas;
    //ライトオブジェクト
    public GameObject Lighting;
    //ステージクリエーター
    public GameObject StageCreater;
    //EnemyController
    public GameObject EnemyController;
    public GameObject SimplerEnemyController;

    //EnemyGenerator
    public GameObject EnemyGenerator;

    //TimeOverテキスト
    public GameObject TimeOverText;
    public GameObject GameOverText;

    //GameClearText
    public GameObject GameClearText;

    //Result画面関連
    public GameObject ResultBG;
    public GameObject ResultText;

    //Audio関連
    public AudioSource audiosource;
    public AudioSource CheatingAudioSource;

    public AudioClip TitleBGM;
    public AudioClip GameOverSound;
    


    

    //現在のゲーム進行ステータスを列挙型で
    public enum GameStatus
	{
        Title,
        CLEAR,
		GAME_OVER,
        CountForTimeAttack,
        CountForScoreAttack,
		TimeAttackonGoing,
        ScoreAttackonGoing,
		CanGoal,
        Result,
	}

	public GameStatus currentstatus;

	// Use this for initialization
	void Start () {
        currentstatus = GameStatus.Title;
        GameController();
		
	}
	
	// Update is called once per frame
	void Update () {

        TimeManagement();
        GameStatusJudge();
        CheatingDaySetup();
        CheatingCoroutine();
		}
		



	public void ScoreUpdate(){
        //チート用スコアのアップデート
        if (!IsCheatDay)
        {
            ScoreForCheating += dotpoint;
        }

        currentscore += dotpoint;
		Debug.Log (currentscore);
        this.ProteinText.GetComponent<Text>().text = "Protein : " + currentscore + "g";

        if(currentscore >= NecessaryDotNum)
        {
            PlayerWeight--;
            NecessaryDotNum += 100;
        }

        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

 
    //プレイヤーとの衝突検知で得点の増減を行う(チート前)
    public void EnemyCollision()
    {
        PlayerWeight += EnemyScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void BossCollision()
    {
        PlayerWeight += EnemyScoreEffect * 2;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void BurgerCollision()
    {
        PlayerWeight += BurgerScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void PizzaCollision()
    {
        PlayerWeight += PizzaScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void RiceCollision()
    {
        PlayerWeight += RiceScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    //プレイヤーとの衝突検知で得点の増減を行う(チートデイ発動後の衝突によるスコア影響)
    public void CheatEnemyCollision()
    {
        PlayerWeight -= EnemyScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void CheatBossCollision()
    {
        PlayerWeight -= EnemyScoreEffect * 2;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void CheatBurgerCollision()
    {
        PlayerWeight -= BurgerScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void CheatPizzaCollision()
    {
        PlayerWeight -= PizzaScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }

    public void CheatRiceCollision()
    {
        PlayerWeight -= RiceScoreEffect;
        this.WeightText.GetComponent<Text>().text = "Current Weight : " + PlayerWeight + "kg";
    }


    public void GameStatusJudge()
    {
        //GameOver判定
        if ((currentstatus == GameStatus.TimeAttackonGoing || currentstatus == GameStatus.ScoreAttackonGoing || currentstatus == GameStatus.CanGoal) && player.GetComponent<PlayerController>().PlayerMotivation <= 0)
        {
            currentstatus = GameStatus.GAME_OVER;
            audiosource.clip = GameOverSound;
            audiosource.PlayOneShot(GameOverSound);
            GameOverText.SetActive(true);
            GameOverText.transform.DOScale(new Vector3(1, 1, 1), 1f);
            player.SetActive(false);
            Lighting.GetComponent<Light>().intensity = 0.1f;
        }

        if(currentstatus == GameStatus.TimeAttackonGoing && TimeLimit <= 0)
        {
            currentstatus = GameStatus.GAME_OVER;
            audiosource.clip = GameOverSound;
            audiosource.PlayOneShot(GameOverSound);
            TimeOverText.SetActive(true);
            GameOverText.transform.DOScale(new Vector3(1, 1, 1), 1f);
            player.SetActive(false);
            Lighting.GetComponent<Light>().intensity = 0.1f;
            TimeText.SetActive(false);

        }
        //Goal出現判定
        if (currentscore >= gameclearscore && currentstatus == GameStatus.TimeAttackonGoing)
        {
            currentstatus = GameStatus.CanGoal;
            EnemyGenerator.GetComponent<EnemyGenerator>().BossGenerate();
            goal.SetActive(true);
            Door.transform.DORotate(endValue: new Vector3(0, -90f, 0), duration: 1.0f, mode: RotateMode.Fast);
            outerwall.SetActive(false);

        }

        if (currentstatus == GameStatus.GAME_OVER)
        {
            TimeLimit = 60f;
            StartCoroutine("ShowResultSummary", 2.5f);

        }

        if (currentstatus == GameStatus.CLEAR)
        {
            StartCoroutine("ShowResultSummary", 2.5f);

        }

        if(currentstatus == GameStatus.Result)
        {
            StopCoroutine("ShowResultSummary");

        }


    }

    public void GameController()
    {
        switch (currentstatus)
        {
            case GameStatus.Title:
                TitleScreen();
                break;
            case GameStatus.TimeAttackonGoing:

                break;
            case GameStatus.ScoreAttackonGoing:
                StageCreater.SetActive(true);
                CountIn -= Time.deltaTime;
                CountInText.GetComponent<Text>().text = CountIn.ToString("f0");
                break;
            case GameStatus.CanGoal:
                break;
            case GameStatus.CLEAR:
                break;
            case GameStatus.GAME_OVER:
                break;
            case GameStatus.Result:
                break;
        }
    }

    //タイトル画面アニメーションメソッド
    public void TitleScreen()
    {
        Lighting.GetComponent<Light>().intensity = 0.1f;
        TitleText.GetComponent<RectTransform>().DOLocalMove(new Vector3(0, 300f, 0), 2.5f);
        Invoke("ButtonGenerate", 3.0f);
    }

    //タイトル画面でタイトル文字より遅れてボタンを出すInvoke用のメソッド(BGMも同時に再生する）
    private void ButtonGenerate()
    {
      if(currentstatus == GameStatus.Title)
        {
            PlayButton.SetActive(true);
            HowToButton.SetActive(true);
            audiosource.clip = TitleBGM;
            audiosource.Play();
        }
    }

    public void TimeManagement()
    {
        if (currentstatus == GameStatus.ScoreAttackonGoing)
        {
            time = 0;
            time += Time.time;
            this.TimeText.GetComponent<Text>().text = "Time : " + time.ToString("f1");
        }

        if (currentstatus == GameStatus.TimeAttackonGoing)
        { 
            TimeLimit -= Time.deltaTime;
            if (TimeLimit <= 120f)
            {
                this.TimeText.GetComponent<Text>().text = "Time Limit : " + TimeLimit.ToString("f1");
            }

            if(TimeLimit <= 0)
            {
                TimeLimit = 0;
            }
        }
    }

    public void GameStart()
    {
        if(currentstatus == GameStatus.CountForTimeAttack)
        {
            //スコア初期化
            currentscore = 0;
            PlayerWeight = 100;
            ScoreForCheating = 0;
            //タイム初期化
            TimeLimit = 123f;
            //体力初期化
            player.GetComponent<PlayerController>().MotivationReflesh();
            //Cheating Buttonを非アクティブ化
            CheatingButton.interactable = false;
            //プレイヤーを初期位置へ
            player.SetActive(true);
            player.transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
            //取得ずみドットリストの初期化
            player.GetComponent<PlayerController>().RegeneList.Clear();
            //Doorを初期位置へ
            goal.SetActive(false);
            Door.transform.DORotate(endValue: new Vector3(0, 0, 0), duration: 1.0f, mode: RotateMode.Fast);
            outerwall.SetActive(true);
            //GameClearTextの非アクティブ化
            GameClearText.SetActive(false);
            


            StageCreater.SetActive(true);
            GameUICanvas.SetActive(true);
            EnemyGenerator.SetActive(true);
            Lighting.GetComponent<Light>().intensity = 1f;
            mainCamera.GetComponent<Transform>().DOMove(new Vector3(0, 8, -5), 1f);
            mainCamera.GetComponent<Transform>().DORotate(new Vector3(60, 0, 0), 1f);

            CountInScript.GetComponent<CountDown>().StartCoroutine("CountdownCoroutine");
            Invoke("TimeManagement", 3.0f);
        }

        if(currentstatus == GameStatus.CountForScoreAttack)
        {
            //スコア初期化
            currentscore = 0;
            PlayerWeight = 100;
            ScoreForCheating = 0;
            //体力初期化
            player.GetComponent<PlayerController>().MotivationReflesh();
            //Cheating Buttonを非アクティブ化
            CheatingButton.interactable = false;
            //プレイヤーを初期位置へ
            player.SetActive(true);
            player.transform.position = new Vector3(playerPosX, playerPosY, playerPosZ);
            //取得ずみドットリストの初期化
            player.GetComponent<PlayerController>().RegeneList.Clear();
            //Doorを初期位置へ
            goal.SetActive(false);
            Door.transform.DORotate(endValue: new Vector3(0, 0, 0), duration: 1.0f, mode: RotateMode.Fast);
            outerwall.SetActive(true);
                        //GameClearTextの非アクティブ化
            GameClearText.SetActive(false);

            StageCreater.SetActive(true);
            GameUICanvas.SetActive(true);
            EnemyGenerator.SetActive(true);
            Lighting.GetComponent<Light>().intensity = 1f;
            mainCamera.GetComponent<Transform>().DOMove(new Vector3(0, 8, -5), 1f);
            mainCamera.GetComponent<Transform>().DORotate(new Vector3(60, 0, 0), 1f);

            CountInScript.GetComponent<CountDown>().StartCoroutine("CountdownCoroutine");
            Invoke("TimeManagement", 3.0f);
        }
    }

    private IEnumerator ShowResultSummary(float waittime)
    {
        yield return new WaitForSeconds(waittime);
        GameOverText.SetActive(false);
        TimeOverText.SetActive(false);
        GameUICanvas.SetActive(false);
        ResultBG.SetActive(true);
        ResultText.SetActive(true);
        ScoreText.GetComponent<Text>().text = "Score : " + currentscore;
        ClearTimeText.GetComponent<Text>().text = "";
        currentstatus = GameStatus.Result;
    }


    public void CheatingDaySetup()
    {
        if (IsCheatDay == false)
        {
            if (ScoreForCheating >= CheatingScore)
            {
                IsCheatingAvailable = true;
                CheatingButton.interactable = IsCheatingAvailable;
            }

            if(ScoreForCheating < CheatingScore)
            {
                CheatingButton.interactable = false;
            }
        }
    }

    public void CheatingDay()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && IsCheatingAvailable == true)
        {
            IsCheatDay = true;
            ScoreForCheating = 0;

            IsCheatingAvailable = false;
            for (float passedtime = 0; passedtime <= CheatTimeLimit; passedtime += Time.deltaTime)
            {
                IsCheatDay = true;
                Lighting.GetComponent<Light>().intensity = 0.1f;
                PlayerSpotlight.SetActive(true);
            }
            IsCheatDay = false;
            Lighting.GetComponent<Light>().intensity = 1f;
            PlayerSpotlight.SetActive(false);
            CheatingButton.interactable = IsCheatingAvailable;
        }
    }
    //- 一定量プロテインを摂取したら、チートデイの力を借りてパワーアップしましょう！この間はどんな誘惑も打ち消すことができます。

    //試しにチートをコルーチンでコントロールしてみる

    public IEnumerator Cheatingday(float waittime)
    {
        IsCheatDay = true;
        CheatingAudioSource.Play();
        ScoreForCheating = 0;
        CheatingButton.interactable = false;
        Lighting.GetComponent<Light>().intensity = 0.1f;
        PlayerSpotlight.SetActive(true);
        yield return new WaitForSeconds(waittime);
        CheatingAudioSource.Pause();
        IsCheatDay = true;
        Lighting.GetComponent<Light>().intensity = 1f;
        PlayerSpotlight.SetActive(false);
        IsCheatDay = false;
    }

    public void CheatingCoroutine()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsCheatingAvailable == true)
        {
            StartCoroutine("Cheatingday", CheatTimeLimit);
        }
    }
}
