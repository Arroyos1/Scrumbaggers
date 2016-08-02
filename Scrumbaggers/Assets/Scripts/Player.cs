using UnityEngine;
using System.Collections;
using UnityEngine. SceneManagement;
using UnityEngine . UI;

public class Player : MovingObject
{
     public int wallDamage = 1;
     public int pointsPerFood = 10;
     public int pointsPerSoda = 20;
     public float restartLevelDelay = 1f;
     public Text foodText1;
     public Text scrapText1;
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

     void Awake()
     {
          turnIndicator = GameObject . Find ( "TurnIndicator1" );
     }

     protected override void Start ()
     {
          animator = GetComponent<Animator> ( );
          
          food = GameManager . instance . playerFoodPoints;
          scraps = GameManager . instance . playerScrapPoints;

          foodText1 . text = "Food: " + food;
          scrapText1 . text = "Scraps: " + scraps;

          base . Start ( );
	}
	
     private void OnDisable ()
     {
          GameManager . instance . playerFoodPoints = food;
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
          food--;
          foodText1 . text = "Food: " + food;

          base . AttemptMove <T> ( xDir , yDir );

          RaycastHit2D hit;
          if (Move (xDir, yDir, out hit))
          {
               SoundManager . instance . RandomizeSfx ( moveSound1 , moveSound2 );
          }

          CheckIfGameOver ( );

          GameManager . instance . player1Turn = false;
          GameManager . instance . enemy2Turn = true;         
     }

     private void OnTriggerEnter2D (Collider2D other)
     {
          if (other.tag == "Exit")
          {
               Invoke ( "Restart" , restartLevelDelay );
               enabled = false;
          }
          else if (other.tag == "Food")
          {
               food += pointsPerFood;
               foodText1 . text = "+" + pointsPerFood + " Food: " + food;
               SoundManager . instance . RandomizeSfx ( eatSound1 , eatSound2 );
               other . gameObject . SetActive ( false );
          }
          else if(other.tag == "Soda")
          {
               food += pointsPerSoda;
               foodText1 . text = "+" + pointsPerSoda + " Food: " + food;
               SoundManager . instance . RandomizeSfx ( drinkSound1 , drinkSound2 );
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
          foodText1 . text = "-" + loss + " Food: " + food;
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
