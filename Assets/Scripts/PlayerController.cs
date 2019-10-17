using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	//rigidbody取得
	private Rigidbody rigid;
	//アニメーター取得
	private Animator animator;
	//character controller コンポーネント取得
	private CharacterController myController;
	//GameManager取得
	public GameObject gameManager;
	//GameClearText取得
	public GameObject gameclearText;
    //StageCreaterオブジェクト取得
    public GameObject StageCreater;

	private float moveSpeed = 2.0f; //Playerの基本移動速度
	private float rotateSpeed = 1.0f; //回転速度

	private Vector3 movedirection; //移動方向

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator>();
		this.myController = GetComponent<CharacterController> ();
		this.rigid = GetComponent<Rigidbody> ();
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		PlayerMovement ();
	}
		

	public void PlayerMovement(){
		if (Input.GetKey(KeyCode.UpArrow)) {
			//上方向への移動
			this.transform.rotation = Quaternion.Euler (0, 0, 0);
			rigid.velocity = new Vector3(rigid.velocity.x,rigid.velocity.y, moveSpeed);
			this.animator.SetFloat("Speed", 1);
		}
		if (Input.GetKey(KeyCode.DownArrow)) {
			//方向への移動
			this.transform.rotation = Quaternion.Euler (0, 180, 0);
			rigid.velocity = new Vector3(rigid.velocity.x,rigid.velocity.y, -moveSpeed);
            this.animator.SetFloat("Speed", 1);
        }
		if (Input.GetKey(KeyCode.LeftArrow)) {
			//左方向への移動
			this.transform.rotation = Quaternion.Euler (0, 270, 0);
			rigid.velocity = new Vector3(-moveSpeed,rigid.velocity.y, rigid.velocity.z);
            this.animator.SetFloat("Speed", 1);
        }
		if (Input.GetKey(KeyCode.RightArrow)) {
			//上方向への移動
			this.transform.rotation = Quaternion.Euler (0, 90, 0);
			rigid.velocity = new Vector3(moveSpeed,rigid.velocity.y, rigid.velocity.z);
            this.animator.SetFloat("Speed", 1);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            //上方向への移動
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
            rigid.velocity = new Vector3(rigid.velocity.x, rigid.velocity.y, moveSpeed);
            this.animator.SetFloat("Speed", 1);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            this.animator.SetFloat("Speed", 0);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            this.animator.SetFloat("Speed", 0);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            this.animator.SetFloat("Speed", 0);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            this.animator.SetFloat("Speed", 0);
        }

    }

	void OnTriggerEnter(Collider col){
		if (col.gameObject.tag == "Dot") {
			gameManager.GetComponent<GameManager> ().ScoreUpdate ();
			Destroy (col.gameObject);
            GetComponent<ParticleSystem>().Play();

            int collisionPosX = (int)col.gameObject.transform.position.x * 2;
            int collisionPosY = (int)col.gameObject.transform.position.z * 2;

            
            
            

		} else if (col.gameObject.name == "Goal") {
			gameclearText.SetActive (true);
		} else if (col.gameObject.tag == "Enemy")
        {
            gameManager.GetComponent<GameManager>().EnemyCollision();
            Destroy(col.gameObject);
        } else if( col.gameObject.tag == "Burger")
        {
            gameManager.GetComponent<GameManager>().BurgerCollision();
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "Pizza")
        {
            gameManager.GetComponent<GameManager>().PizzaCollision();
            Destroy(col.gameObject);
        }
        else if (col.gameObject.tag == "Rice")
        {
            gameManager.GetComponent<GameManager>().RiceCollision();
            Destroy(col.gameObject);
        }

    }
}
