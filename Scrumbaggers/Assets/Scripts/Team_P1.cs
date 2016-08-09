using UnityEngine;
using System.Collections;

public class Team_P1 : MovingObject
{
     public int playerDamage;
     
     public AudioClip enemyAttack1;
     public AudioClip enemyAttack2;

     private Animator animator;
     private Transform target;
     private bool skipMove;
     private int health = 3;

     protected override void Start ()
     {
          GameManager . instance . AddEnemyToList ( this );
          animator = GetComponent<Animator> ( );
          target = GameObject . FindGameObjectWithTag ( "Player2" ) . transform;
          Debug . Log ( target . transform );
          base . Start ( );
	}	

     protected override void AttemptMove <T> (int xDir, int yDir)
     {
          if(skipMove)
          {
               skipMove = false;
               return;
          }

          base . AttemptMove<T> ( xDir , yDir );

          skipMove = true;
     }

     public void MoveEnemy1 ( )
     {
          int xDir = 0;
          int yDir = 0;

          if ( Mathf . Abs ( target . position . x - transform . position . x ) < float . Epsilon )
               yDir = target . position . y > transform . position . y ? 1 : -1;
          else
               xDir = target . position . x > transform . position . x ? 1 : -1;

          AttemptMove<Player2> ( xDir , yDir );
         
     }
    
     protected override void OnCantMove<T> (T component)
     {
          if (component.CompareTag ("Player2"))
          {
               Player2 hitPlayer2 = component as Player2;
               hitPlayer2 . LoseFood ( playerDamage );
          }
          
          else if (component.CompareTag ("Team_P2"))
          {
               Team_P2 hitTeam2 = component as Team_P2;
               hitTeam2 . LoseHealth ( playerDamage );
          }          
         
          animator . SetTrigger ( "enemyAttack" );

          SoundManager . instance . RandomizeSfx ( enemyAttack1 , enemyAttack2 );          
     }

     public void LoseHealth ( int loss )
     {
          //animator . SetTrigger ( "playerHit" );
          health -= loss;
          //foodText1 . text = "-" + loss + " Food: " + food;
          //CheckIfGameOver ( );
          if ( health == 0 )
          {
               gameObject . SetActive ( false );
          }
     }
}
