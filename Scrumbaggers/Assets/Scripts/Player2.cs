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
     public AudioClip moveSound1;
     public AudioClip moveSound2;
     public AudioClip eatSound1;
     public AudioClip eatSound2;
     public AudioClip drinkSound1;
     public AudioClip drinkSound2;
     public AudioClip gameOverSound;

     private Animator animator;
     private int food;
     private int scraps;

	protected override void Start ()
     {
          animator = GetComponent<Animator> ( );

          food = GameManager . instance . playerFoodPoints;
          scraps = GameManager . instance . playerScrapPoints;

          foodText2 . text = "Food: " + food;
          scrapText2 . text = "Scraps: " + scraps;

          base . Start ( );
	}
	
     private void OnDisable ()
     {
          GameManager . instance . playerFoodPoints = food;
     }

	void Update ()
     {
          if ( !GameManager . instance . player2Turn ) return;

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
          GameManager . instance . enemy1Turn =true;          
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
