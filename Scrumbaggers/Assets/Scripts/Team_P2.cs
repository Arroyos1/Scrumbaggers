using UnityEngine;
using System.Collections;

public class Team_P2 : MovingObject
{
	public int playerDamage;

	public AudioClip enemyAttack1;
	public AudioClip enemyAttack2;

	private Animator animator;
	private Transform target;
	private bool skipMove;
	private int health = 40;
     
	protected override void Start ()
	{
		GameManager . instance . AddEnemy2ToList ( this );
		animator = GetComponent<Animator> ( );
		target = GameObject . FindGameObjectWithTag ( "Player1" ) . transform;
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

	public void MoveEnemy2 ( )
	{
		int xDir = 0;
		int yDir = 0;

		if ( Mathf . Abs ( target . position . x - transform . position . x ) < float . Epsilon )
			yDir = target . position . y > transform . position . y ? 1 : -1;
		else
			xDir = target . position . x > transform . position . x ? 1 : -1;

		AttemptMove<Player1> ( xDir , yDir );
	}

	protected override void OnCantMove<T> (T component)
	{
          if ( component . CompareTag ( "Player1" ) )
          {
               Player1 hitPlayer1 = component as Player1;
               hitPlayer1 . LoseFood ( playerDamage );
          }

          else if ( component . CompareTag ( "Team1" ) )
          {
               Team_P1 hitTeam1 = component as Team_P1;
               hitTeam1 . LoseHealth ( playerDamage );
          }

          animator . SetTrigger ( "enemy2Attack" );

		SoundManager . instance . RandomizeSfx ( enemyAttack1 , enemyAttack2 );
	}

	public void LoseHealth ( int loss )
	{
		animator . SetTrigger ( "enemy2Attack" );
		health -= loss;
		Debug . Log ( health );
		//foodText1 . text = "-" + loss + " Food: " + food;
		//CheckIfGameOver ( );
		if (health <= 0)
		{
			gameObject . SetActive ( false );
		}
	}
}
