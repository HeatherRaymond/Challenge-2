using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    public float moveHorizontal;
    public Text score;
    public Text lives;
    public Text winText;
    public AudioSource musicSource;
    public AudioClip musicClipOne;
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool facingRight = true;
    private bool onGround = true;


    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        SetScoreText();
        SetLivesText();
        winText.text = "";
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        moveHorizontal = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (onGround == false)
            {
                anim.SetInteger("State", 2);
            }
            else
            {
                anim.SetInteger("State", 0);
            }
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }

        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }

        if (Input.GetKey("escape"))
            Application.Quit();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            SetScoreText();
        }
        else if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
            SetLivesText();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            onGround = true;

            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
                anim.SetInteger("State", 2);
            }
        }
        else
        {
            onGround = false;
            anim.SetInteger("State", 0);
        }
    }

    void SetScoreText()
    {
        score.text = "Score: " + scoreValue.ToString();
        if (scoreValue >= 8)
        {
            winText.text = "You win! Game created by Heather Raymond!";
            musicSource.clip = musicClipOne;
            musicSource.Play();
            musicSource.loop = false;
    }
        if (scoreValue == 4)
        {
            transform.position = new Vector2(43.66f, 2.04f);
            livesValue = 3;
            SetLivesText();
        }
    }

    void SetLivesText()
    {
        lives.text = "Lives: " + livesValue.ToString();
        if (livesValue <= 0)
        {
            winText.text = "You Lose!";
            Destroy(this);
        }
    }
}
