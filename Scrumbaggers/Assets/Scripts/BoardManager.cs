using UnityEngine;
using System; //to use serializable attribut
using System.Collections.Generic;
using Random = UnityEngine . Random;

public class BoardManager : MonoBehaviour
{
     [Serializable] //Requires System
     public class Count
     {
          public int minimum;
          public int maximum;

          public Count(int min, int max)
          {
               minimum = min;
               maximum = max;
          }
     }

     public int columns = 19;
     public int rows = 9;

     [HideInInspector]
     public int team1Count;
     [HideInInspector]
     public int team2Count;
     //public int scrumbags_P1;
     //public int scrumbags_P2;

     public Count wallCount = new Count (8, 12);
     public Count foodCount = new Count ( 2 , 6 );

     public GameObject exit;
     public GameObject exit_P2;
     public GameObject stones;
     public GameObject [ ] floorTiles;
     public GameObject [ ] wallTiles;
     public GameObject [ ] outerWallTiles;
     public GameObject [ ] foodTiles;
     public GameObject [ ] team1Tiles;
     public GameObject [ ] team2Tiles;

     private Transform boardHolder;

     private List <Vector3> gridPositions = new List <Vector3> ( );  //requires System.Collections.Generic
     private List <Vector3> stonePositions = new List <Vector3> ( );
     private List <Vector3> enemyPositions_P1 = new List <Vector3> ( );
	private List <Vector3> enemyPositions_P2 = new List <Vector3> ( );

     private bool stoned = false;
     private bool enemyDeploy_P1 = false;
     private bool enemyDeploy_P2 = false;


     /// <summary>
     /// Called in SetupScene() clears the current List, goes through each potential gridposition and adds to list
     /// </summary>
     void InitializeLists ()
     {
        stonePositions . Clear ( );

          for ( int x = 3 ; x < columns - 3 ; x++ )
          {
               for ( int y = 3 ; y < rows - 3 ; y++ )
               {
                    stonePositions . Add ( new Vector3 ( x , y , 0f ) );
               }
          }

		gridPositions . Clear ( );

          for (int x = 1 ; x < columns - 1 ; x++ )
          {
               for (int y = 1 ; y < rows - 1 ; y++ )
               {
                    gridPositions . Add ( new Vector3 ( x , y , 0f ) );
               }
          }

          enemyPositions_P1. Clear();
          for ( int x = 1 ; x < ( columns / 2 ) - 1 ; x++ )
         {
               for (int y = 1 ; y < rows - 1 ; y++ )
               {
                    enemyPositions_P1 . Add ( new Vector3 ( x , y , 0f ) );
               }
          }

          enemyPositions_P2 . Clear ( );

          for ( int x = ( columns / 2 ) + 1 ; x < columns - 1 ; x++ )
          {
               for ( int y = 1 ; y < rows - 1 ; y++ )
               {
                    enemyPositions_P2 . Add ( new Vector3 ( x , y , 0f ) );
               }
          }
     }

     /// <summary>
     /// Called in SetupScene() Creates an Empty Gameobject in Heirarchy to hold the instantiated floor and wall tiles that make up the board
     /// </summary>
     void BoardSetup()
     {
          boardHolder = new GameObject ( "Board" ) . transform;
          
          for (int x = -1 ; x < columns + 1 ; x++ )
          {
               for (int y = -1 ; y < rows + 1 ; y++ )
               {
                    GameObject toInstantiate = floorTiles [ Random . Range ( 0 , floorTiles . Length ) ];

                    if ( x == -1 || x == columns || y == -1 || y == rows )
                         toInstantiate = outerWallTiles [ Random . Range ( 0 , outerWallTiles . Length ) ];

                    GameObject instance = Instantiate ( toInstantiate , new Vector3 ( x , y , 0f ) , Quaternion . identity ) as GameObject;

                    instance . transform . SetParent ( boardHolder );
               }
          }
     }

	
     /// <summary>
     /// returns a random Vector3 grid position from the list and removes it so it is not chosen again
     /// </summary>
     /// <returns></returns>
     Vector3 RandomPosition ()
     {
		int stoneIndex = Random.Range( 0, stonePositions . Count );
          int randomIndex = Random . Range ( 0 , gridPositions . Count );
          int enemy1Index = Random . Range ( 0 , enemyPositions_P1 . Count );
          int enemy2Index = Random.Range(0, enemyPositions_P2 . Count );

          if (!stoned)
     	{
			Vector3 stonePosition = stonePositions [ stoneIndex ];
			stoned = true;

			gridPositions . RemoveAt ( stoneIndex );
               enemyPositions_P1 . RemoveAt ( stoneIndex );
               enemyPositions_P2 . RemoveAt ( stoneIndex );
               	
          	return stonePosition;
     	}
     	else if (enemyDeploy_P1)
          {
               Vector3 enemyPosition_P1 = enemyPositions_P1 [ enemy1Index ];
               enemyDeploy_P1 = false;
               gridPositions . RemoveAt ( enemy1Index );
               enemyPositions_P1 . RemoveAt ( enemy1Index );
               enemyPositions_P2 . RemoveAt ( enemy1Index );
               return enemyPosition_P1;
          }
          else if ( enemyDeploy_P2 )
          {
               Vector3 enemyPosition_P2 = enemyPositions_P2 [ enemy2Index ];
               enemyDeploy_P2 = false;
               gridPositions . RemoveAt ( enemy2Index );
               enemyPositions_P2 . RemoveAt ( enemy2Index );
               return enemyPosition_P2;
          }
          else
     	{
			Vector3 randomPosition = gridPositions [ randomIndex ];
          	gridPositions . RemoveAt ( randomIndex );
          	return randomPosition; //to LayoutObjectAtRandom()
     	}
          
     }
     
	void LayoutStoneAtRandom ( )
     {
     	Vector3 stonePosition = RandomPosition ( );
    	     Instantiate ( stones , stonePosition , Quaternion . identity);
		GameObject wallChoice = wallTiles [ Random . Range ( 0 , wallTiles . Length ) ];
      	Instantiate ( wallChoice , stonePosition , Quaternion . identity );
     }
     /// <summary>
     /// Selects a random object from the gameobject array and instantiates it at the random position returned from RandomPosition()
     /// </summary>
     /// <param name="tileArray"></param>
     /// <param name="minimum"></param>
     /// <param name="maximum"></param>
     void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
     {
          int objectCount = Random . Range ( minimum , maximum + 1 );

          for(int i = 0 ; i < objectCount ; i ++ )
          {
               Vector3 randomPosition = RandomPosition ( );
               GameObject tileChoice = tileArray [ Random . Range ( 0 , tileArray . Length ) ];
               Instantiate ( tileChoice , randomPosition , Quaternion . identity );
          }
     }

     void LayoutEnemy1AtRandom( GameObject [ ] tileArray , int minimum , int maximum )
     {
     	for (int i = 0; i < team1Count; i++)
     	{
               enemyDeploy_P1 = true;
               Vector3 randomPosition = RandomPosition();
               GameObject tileChoice = tileArray [ Random . Range ( 0 , tileArray . Length ) ];
               Instantiate ( tileChoice , randomPosition , Quaternion . identity );
          }
     }

     void LayoutEnemy2AtRandom ( GameObject [ ] tileArray , int minimum , int maximum )
     {
          for ( int i = 0 ; i < team2Count ; i++ )
          {
               enemyDeploy_P2 = true;
               Vector3 randomPosition = RandomPosition ( );
               GameObject tileChoice = tileArray [ Random . Range ( 0 , tileArray . Length ) ];
               Instantiate ( tileChoice , randomPosition , Quaternion . identity );
          }
     }

     /// <summary>
     /// Calls Above Functions and adds the exit tile
     /// Called from GameManager InitGame() passing the level variable to it
     /// </summary>
     /// <param name="level"></param>
     public void SetupScene (int level)
     {
    	BoardSetup ( );
  		InitializeLists ( );
   		
		LayoutStoneAtRandom ( );

      	LayoutObjectAtRandom ( wallTiles , wallCount . minimum , wallCount . maximum );
      	LayoutObjectAtRandom ( foodTiles , foodCount . minimum , foodCount . maximum );

          team1Count = ( int ) Mathf . Log ( level , 2f ); //Places Enemy 1
         	LayoutEnemy1AtRandom ( team1Tiles , team1Count , team1Count );

          team2Count = ( int ) Mathf . Log ( level , 2f );                                               //Places Enemy 2
        	LayoutEnemy2AtRandom ( team2Tiles , team2Count , team2Count );

      	Instantiate ( exit , new Vector3 ( columns - 1 , rows - 5 , 0f ) , Quaternion . identity );         
		Instantiate ( exit_P2 , new Vector3 ( columns - columns, rows - 5 , 0f ) , Quaternion . identity );       	      
     }
}
