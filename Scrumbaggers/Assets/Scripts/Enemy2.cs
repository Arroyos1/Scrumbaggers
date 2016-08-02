using UnityEngine;
using System.Collections;

public class Enemy2 : MovingObject
{
     public int playerDamage;
     public AudioClip enemyAttack1;
     public AudioClip enemyAttack2;

     private Animator animator;
     private Transform target;
     private bool skipMove;

	protected override void Start ()
     {
          GameManager . instance . AddEnemy2ToList ( this );
          animator = GetComponent<Animator> ( );
          target = GameObject . FindGameObjectWithTag ( "Player2" ) . transform;
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

     public void MoveEnemy2 ( )
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
          Player2 hitPlayer = component as Player2;

          animator . SetTrigger ( "enemy2Attack" );

          SoundManager . instance . RandomizeSfx ( enemyAttack1 , enemyAttack2 );

          hitPlayer . LoseFood ( playerDamage );
     }
}
