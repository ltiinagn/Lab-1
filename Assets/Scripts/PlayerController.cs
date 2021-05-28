using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed, upSpeed;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private bool onGroundState = false;

    public Transform enemyLocation;
    public Text scoreText;
    private int score = 0;
    private bool countScoreState = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
	    Application.targetFrameRate =  30;
	    marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        scoreText.text = "Score: " + score.ToString();
    }

    public float maxSpeed;
    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate()
    {
        if (Input.GetKeyDown("space") && onGroundState){
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            countScoreState = true; //check if Gomba is underneath
        }
        
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        if (onGroundState && (!Input.GetKey("a") && !Input.GetKey("d"))) {
            // stop
            marioBody.velocity = Vector2.zero;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
        }

        // when jumping, and Gomba is near Mario and we haven't registered our score
        if (!onGroundState && countScoreState)
        {
            if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
            {
                countScoreState = false;
                score++;
                Debug.Log(score);
            }
        }
    }

    // called when the cube hits the floor
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) {
            onGroundState = true; // back on ground
            countScoreState = false; // reset score state
            scoreText.text = "Score: " + score.ToString();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Goomba")) {
            Debug.Log("Collided with Goomba!");
            GameObject cameraObject = GameObject.Find("Main Camera");
            cameraObject.GetComponent<CameraController>().enabled = false;
            GameObject mainGameObject = GameObject.Find("UI").GetComponent<MenuController>().mainGameObject;
            Destroy(mainGameObject);
            Transform transformUI = GameObject.Find("UI").transform;
            foreach (Transform eachChild in transformUI) {
                if (eachChild.name != "Score") {
                    Debug.Log("Child found. Name: " + eachChild.name);
                    // enable them
                    eachChild.gameObject.SetActive(true);
                }
            }
        }
    }
}
