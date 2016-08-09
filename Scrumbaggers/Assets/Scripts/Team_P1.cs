using UnityEngine;
using System.Collections;

public class Team_P1 : MovingObject
{
     public int playerDamage;
     
     public AudioClip enemyAttack1;
     public AudioClip enemyAttack2;

     private Animator animator;
     private Transform target;
     private Transform team;
     private bool skipMove;
     private int health = 40;

     protected override void Start ()
     {
          GameManager . instance . AddEnemyToList ( this );
          animator = GetComponent<Animator> ( );
          target = GameObject . FindGameObjectWithTag ( "Player2" ) . transform;
          team = GameObject . FindGameObjectWithTag ("Team2"). transform;
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
          //AttemptMove<Team_P2> ( xDir , yDir );
         
     }
    
     protected override void OnCantMove<T> (T component)
     {
         
          if (component.gameObject.CompareTag ("Player2"))
          {
               Player2 hitPlayer2 = component as Player2;
               hitPlayer2 . LoseFood ( playerDamage );
               Debug . Log ( component );
          }
          
          else if (component.CompareTag ("Team2"))
          {
               Team_P2 hitTeam2 = component as Team_P2;
               hitTeam2 . LoseHealth ( playerDamage );
               Debug . Log ( component );
          }          
         
          animator . SetTrigger ( "enemyAttack" );

          SoundManager . instance . RandomizeSfx ( enemyAttack1 , enemyAttack2 );          
     }

     public void LoseHealth ( int loss )
     {
          animator . SetTrigger ( "enemyAttack" );
          health -= loss;
          //foodText1 . text = "-" + loss + " Food: " + food;
          //CheckIfGameOver ( );
          if ( health <= 0 )
          {
               gameObject . SetActive ( false );
          }
     }
}
