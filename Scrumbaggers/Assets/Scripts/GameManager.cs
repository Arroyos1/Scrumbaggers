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
     [HideInInspector]
     public bool player1Turn = true;
     [HideInInspector]
     public bool player2Turn = false;
     [HideInInspector]
     public bool enemy1Turn = false;
     [HideInInspector]
     public bool enemy2Turn = false;

     private Text levelText;
     private GameObject levelImage;
     private int level = 20;
     private List<Enemy> enemies;
     private List<Enemy2> enemies2;
     private bool enemy1Moving;
     private bool enemy2Moving;
     private bool doingSetup;

	void Awake ()
     {
          if ( instance == null )
               instance = this;
          else if ( instance != this )
               Destroy ( gameObject );

          DontDestroyOnLoad ( gameObject );
          enemies = new List<Enemy> ( );
          enemies2 = new List<Enemy2> ( );
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
          levelText . text = "Day" + level;
          levelImage . SetActive ( true );
          Invoke ( "HideLevelImage" , levelStartDelay );

          enemies . Clear ( );
          enemies2 . Clear ( );
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
          if ( player1Turn || player2Turn || enemy1Moving || enemy2Moving || doingSetup )
               return;
          else if ( enemy1Turn )
                    StartCoroutine ( MoveEnemies1 ( ) );
          else if ( enemy2Turn )
                    StartCoroutine ( MoveEnemies2 ( ) );
     }

     public void AddEnemyToList(Enemy script)
     {
          enemies . Add ( script );
     }

     public void AddEnemy2ToList (Enemy2 script)
     {
          enemies2 . Add ( script );
     }

     IEnumerator MoveEnemies1 ( )
     {
          enemy1Moving = true;
          yield return new WaitForSeconds ( turnDelay );

          if (enemies.Count == 0 && enemies2.Count == 0)
          {
               yield return new WaitForSeconds ( turnDelay );
          }

          for(int i = 0 ; i < enemies. Count ; i++ )
          {
               enemies [ i ] . MoveEnemy1 ( );
               yield return new WaitForSeconds ( enemies [ i ] . moveTime );
          }

          enemy1Moving = false;
          enemy1Turn = false;
          player1Turn = true;
     }

     IEnumerator MoveEnemies2 ( )
     {
          enemy2Moving = true;
          yield return new WaitForSeconds ( turnDelay );

          if ( enemies . Count == 0 && enemies2 . Count == 0 )
          {
               yield return new WaitForSeconds ( turnDelay );
          }

          for ( int i = 0 ; i < enemies2 . Count ; i++ )
          {
               enemies2 [ i ] . MoveEnemy2 ( );
               yield return new WaitForSeconds ( enemies2 [ i ] . moveTime );
          }

          enemy2Moving = false;
          enemy2Turn = false;
          player2Turn = true;
     }
}
