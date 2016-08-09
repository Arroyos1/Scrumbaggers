using UnityEngine;
using System.Collections;
using System . Collections . Generic;
using UnityEngine . UI;

public class GameManager : MonoBehaviour
{
     public float levelStartDelay = 2f;
     public float turnDelay = 0.1f;
     public static GameManager instance = null;
     public BoardManager boardScript;
     public int playerFoodPoints = 100;
     public int playerScrapPoints = 0;
     public int playerScorePoints = 0;
     public int player2FoodPoints = 100;
     public int player2ScrapPoints = 0;
     public int player2ScorePoints = 0;
     [HideInInspector]
     public bool player1Turn = true;
     [HideInInspector]
     public bool player2Turn = false;
     [HideInInspector]
     public bool team1Turn = false;
     [HideInInspector]
     public bool team2Turn = false;

     private Text levelText;
     private GameObject levelImage;
     private int level = 1;
     private List<Team_P1> team1;
     private List<Team_P2> team2;
     private bool team1Moving;
     private bool team2Moving;
     private bool doingSetup;

	void Awake ()
     {
          if ( instance == null )
               instance = this;
          else if ( instance != this )
               Destroy ( gameObject );

          DontDestroyOnLoad ( gameObject );
          team1 = new List<Team_P1> ( );
          team2 = new List<Team_P2> ( );
          boardScript = GetComponent<BoardManager> ( );
          InitGame ( );

     }

     private void OnLevelWasLoaded (int index)
     {
          level++;

          InitGame ( );
     }	
     void InitGame()
     {
          doingSetup = true;

          levelImage = GameObject . Find ( "LevelImage" );
          levelText = GameObject . Find ( "LevelText" ) . GetComponent<Text> ( );
          levelText . text = "Round " + level;
          levelImage . SetActive ( true );
          Invoke ( "HideLevelImage" , levelStartDelay );

          team1 . Clear ( );
          team2 . Clear ( );
          boardScript . SetupScene ( level );
     }

     private void HideLevelImage()
     {
          levelImage . SetActive (false);
          doingSetup = false;
     }

     public void GameOver ()
     {
          levelText . text = "After " + level + " days, you starved.";
          levelImage . SetActive( true );
          enabled = false;
     } 
     
     void Update()
     {
          if ( player1Turn || player2Turn || team1Moving || team2Moving || doingSetup )
               return;
          else if ( team1Turn )
                    StartCoroutine ( MoveEnemies1 ( ) );
          else if ( team2Turn )
                    StartCoroutine ( MoveEnemies2 ( ) );
     }

     public void AddEnemyToList(Team_P1 script)
     {
          team1 . Add ( script );
     }

     public void AddEnemy2ToList (Team_P2 script)
     {
          team2 . Add ( script );
     }

     IEnumerator MoveEnemies1 ( )
     {
          team1Moving = true;
          yield return new WaitForSeconds ( turnDelay );

          if (team1.Count == 0 && team2.Count == 0)
          {
               yield return new WaitForSeconds ( turnDelay );
          }

          for(int i = 0 ; i < team1. Count ; i++ )
          {
               team1 [ i ] . MoveEnemy1 ( );
               yield return new WaitForSeconds ( team1 [ i ] . moveTime );
          }

          team1Moving = false;
          team1Turn = false;
          player1Turn = true;
     }

     IEnumerator MoveEnemies2 ( )
     {
          team2Moving = true;
          yield return new WaitForSeconds ( turnDelay );

          if ( team1 . Count == 0 && team2 . Count == 0 )
          {
               yield return new WaitForSeconds ( turnDelay );
          }

          for ( int i = 0 ; i < team2 . Count ; i++ )
          {
               team2 [ i ] . MoveEnemy2 ( );
               yield return new WaitForSeconds ( team2 [ i ] . moveTime );
          }

          team2Moving = false;
          team2Turn = false;
          player2Turn = true;
     }
}
