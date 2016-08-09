using UnityEngine;
using System.Collections;
using UnityEngine. SceneManagement;
using UnityEngine . UI;

public class Player1 : MovingObject
{
     public int wallDamage = 1;
     public int pointsPerFood = 10;
     public int pointsPerSoda = 20;
     public int stonePoints = 7;
     public int exitPoints = 3;
     public float restartLevelDelay = 1f;
     public Text foodText1;
     public Text scrapText1;
     public Text scoreText1;
     public AudioClip moveSound1;
     public AudioClip moveSound2;
     public AudioClip moveSound3;
     public AudioClip eatSound1;
     public AudioClip eatSound2;
     public AudioClip drinkSound1;
     public AudioClip drinkSound2;
     public AudioClip stoneSound;
     public AudioClip gameOverSound;

     private Animator animator;
     private GameObject turnIndicator;
     private int food;
     private int scraps;
     private int score;
     private int recharge = 20;
     private bool skipTurn = false;
     private bool exitFirst = false;
     private bool noEnergy = false;

     void Awake()
     {
          turnIndicator = GameObject . Find ( "TurnIndicator1" );
     }

     protected override void Start ()
     {
          animator = GetComponent<Animator> ( );
          
          food = GameManager . instance . playerFoodPoints;
          scraps = GameManager . instance . playerScrapPoints;
          score = GameManager . instance . playerScorePoints;

          foodText1 . text = "Energy: " + food;
          scrapText1 . text = "Scraps: " + scraps;
          scoreText1 . text = "Score: " + score;

          base . Start ( );
	}
	
     private void OnDisable ()
     {
          GameManager . instance . playerFoodPoints = food + recharge;
          GameManager . instance . playerScorePoints = score;
          GameManager . instance . playerScrapPoints = scraps;
     }

	void Update ()
     {
          if ( !GameManager . instance . player1Turn )
          {
               turnIndicator . SetActive ( false );
               return;
          }

          turnIndicator . SetActive ( true );

          int horizontal = 0;
          int vertical = 0;

          horizontal = ( int ) Input . GetAxisRaw ( "Horizontal1" );
          vertical = ( int ) Input . GetAxisRaw ( "Vertical1" );

          if ( horizontal != 0)
               vertical = 0;

          if ( horizontal != 0 || vertical != 0 )
               AttemptMove<Wall> ( horizontal , vertical );
	}

     protected override void AttemptMove <T>  (int xDir, int yDir)
     {
          if ( noEnergy )
          {
               if (skipTurn)
               {
                    skipTurn = false;
                    GameManager . instance . player1Turn = false;
                    GameManager . instance . team1Turn = true;
                    return;
               }
          }

          if (food > 0)
               food--;
          foodText1 . text = "Energy: " + food;
          scoreText1 . text = "Score: " + score;
          scrapText1 . text = "Scraps: " + scraps;

          base . AttemptMove <T> ( xDir , yDir );

          RaycastHit2D hit;
          if (Move (xDir, yDir, out hit))
          {
               SoundManager . instance . RandomizeSfx ( moveSound1 , moveSound2 );
          }

          CheckIfGameOver ( );

          GameManager . instance . player1Turn = false;
          GameManager . instance . team1Turn = true;         
     }

     private void OnTriggerEnter2D (Collider2D other)
     {
          if (other.tag == "Exit")
          {
               food = food + (scraps * 2);
               score = score + exitPoints;
               scoreText1.text = "Score: " + score;
               scraps = 0;
               animator . SetTrigger ( "playerChop" );
               Invoke ( "Restart" , restartLevelDelay );
               enabled = false;
          }
          else if (other.tag == "Food")
          {
               food += pointsPerFood;
               foodText1 . text = "+" + pointsPerFood + " Energy: " + food;
               if ( food > 100 )
                    food = 100;
               SoundManager . instance . RandomizeSfx ( eatSound1 , eatSound2 );
               other . gameObject . SetActive ( false );
          }
          else if(other.tag == "Soda")
          {
               food += pointsPerSoda;
               foodText1 . text = "+" + pointsPerSoda + " Energy: " + food;
               if ( food > 100 )
                    food = 100;
               SoundManager . instance . RandomizeSfx ( drinkSound1 , drinkSound2 );
               other . gameObject . SetActive ( false );
          }
          else if (other.tag == "Stones")
          {
               score = score + stonePoints;
               scoreText1 . text = "+" + stonePoints + " Score: " + score;
               SoundManager . instance . PlaySingle ( stoneSound );
               other . gameObject . SetActive ( false );               
          }
         
     }

     protected override void OnCantMove <T> (T component)
     {
          Wall hitWall = component as Wall;
          hitWall . DamageWall ( wallDamage );
          animator . SetTrigger ( "playerChop" );
          scraps++;
          scrapText1 . text = "Scraps: " + scraps;
     }

     private void Restart()
     {
          SceneManager . LoadScene ( "Main" );
     }

     public void LoseFood (int loss)
     {
          animator . SetTrigger ( "playerHit" );
          food -= loss;
          foodText1 . text = "-" + loss + " Energy: " + food;
          CheckIfGameOver ( );
     }

     private void CheckIfGameOver()
     {
          if ( food <= 0 )
          {
               noEnergy = true;
               skipTurn = true;
               food = 0;               
          }

          if (score >= 100)
          {
               SoundManager . instance . PlaySingle ( gameOverSound );
               SoundManager . instance . musicSource . Stop ( );
               GameManager . instance . GameOver ( );
          }

     }
}
