using UnityEngine;
using System.Collections;
using UnityEngine. SceneManagement;
using UnityEngine . UI;

public class Player2 : MovingObject
{
     public int wallDamage = 1;
     public int pointsPerFood = 10;
     public int pointsPerSoda = 20;
     public float restartLevelDelay = 1f;
     public Text foodText2;
     public Text scrapText2;
     public Text scoreText2;
     public AudioClip moveSound1;
     public AudioClip moveSound2;
     public AudioClip eatSound1;
     public AudioClip eatSound2;
     public AudioClip drinkSound1;
     public AudioClip drinkSound2;
     public AudioClip gameOverSound;

     private Animator animator;
     private GameObject turnIndicator;
     private int food;
     private int scraps;
     private int score;

     void Awake()
     {
          turnIndicator = GameObject . Find ( "TurnIndicator2" );
     }
	protected override void Start ()
     {
          animator = GetComponent<Animator> ( );

          food = GameManager . instance . player2FoodPoints;
          scraps = GameManager . instance . player2ScrapPoints;
          score = GameManager . instance . player2ScorePoints;

          foodText2 . text = "Food: " + food;
          scrapText2 . text = "Scraps: " + scraps;
          scoreText2 . text = "Score: " + score;

          base . Start ( );
	}
	
     private void OnDisable ()
     {
          GameManager . instance . player2FoodPoints = food;
          GameManager . instance . player2ScorePoints = score;
          GameManager . instance . player2ScrapPoints = scraps;
     }

	void Update ()
     {
          if ( !GameManager . instance . player2Turn )
          {
               turnIndicator . SetActive ( false );
               return;
          }

          turnIndicator . SetActive ( true );

          int horizontal = 0;
          int vertical = 0;

          horizontal = ( int ) Input . GetAxisRaw ( "Horizontal2" );
          vertical = ( int ) Input . GetAxisRaw ( "Vertical2" );

          if ( horizontal != 0)
               vertical = 0;

          if ( horizontal != 0 || vertical != 0 )
               AttemptMove<Wall> ( horizontal , vertical );
	}

     protected override void AttemptMove <T>  (int xDir, int yDir)
     {
          food--;
          foodText2 . text = "Food: " + food;

          base . AttemptMove <T> ( xDir , yDir );

          RaycastHit2D hit;
          if (Move (xDir, yDir, out hit))
          {
               SoundManager . instance . RandomizeSfx ( moveSound1 , moveSound2 );
          }

          CheckIfGameOver ( );

          GameManager . instance . player2Turn = false;
          GameManager . instance . team1Turn =true;          
     }

     private void OnTriggerEnter2D (Collider2D other)
     {
          if (other.tag == "Exit2")
          {
               score = score + scraps;
               scoreText2 . text = "Score: " + score;
               scraps = 0;
               animator . SetTrigger ( "player2Chop" );
               Invoke ( "Restart" , restartLevelDelay );
               enabled = false;
          }
          else if (other.tag == "Food")
          {
               food += pointsPerFood;
               foodText2 . text = "+" + pointsPerFood + " Food: " + food;
               SoundManager . instance . RandomizeSfx ( eatSound1 , eatSound2 );
               other . gameObject . SetActive ( false );
          }
          else if(other.tag == "Soda")
          {
               food += pointsPerSoda;
               foodText2 . text = "+" + pointsPerSoda + " Food: " + food;
               SoundManager . instance . RandomizeSfx ( drinkSound1 , drinkSound2 );
               other . gameObject . SetActive ( false );
          }
     }

     protected override void OnCantMove <T> (T component)
     {
          Wall hitWall = component as Wall;
          hitWall . DamageWall ( wallDamage );
          animator . SetTrigger ( "player2Chop" );
          scraps++;
          scrapText2 . text = "Scraps: " + scraps;
     }

     private void Restart()
     {
          SceneManager . LoadScene ( "Main" );
     }

     public void LoseFood (int loss)
     {
          animator . SetTrigger ( "player2Hit" );
          food -= loss;
          foodText2. text = "-" + loss + " Food: " + food;
          CheckIfGameOver ( );
     }

     private void CheckIfGameOver()
     {
          if ( food <= 0 )
          {
               SoundManager . instance . PlaySingle ( gameOverSound );
               SoundManager . instance . musicSource . Stop ( );
               GameManager . instance . GameOver ( );
          }               
     }
}
