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

     public Count wallCount = new Count (8, 12);
     public Count foodCount = new Count ( 2 , 6 );

     public GameObject exit;
     public GameObject exit_P2;
     public GameObject stones;
     public GameObject [ ] floorTiles;
     public GameObject [ ] wallTiles;
     public GameObject [ ] outerWallTiles;
     public GameObject [ ] foodTiles;
     public GameObject [ ] enemyTiles;
     public GameObject [ ] enemy2Tiles;

     private Transform boardHolder;
     private List <Vector3> gridPositions = new List <Vector3> ( );  //requires System.Collections.Generic
     private List<Vector3> stonePositions = new List<Vector3> ( );

     
     /// <summary>
     /// Called in SetupScene() clears the current List, goes through each potentioal gridposition and adds to list
     /// </summary>
     void InitialiseList ()
     {
          gridPositions . Clear ( );

          for (int x = 1 ; x < columns - 1 ; x++ )
          {
               for (int y = 1 ; y < rows - 1 ; y++ )
               {
                    gridPositions . Add ( new Vector3 ( x , y , 0f ) );
               }
          }
     }

     void InitializeLocationList()
     {
          stonePositions . Clear ( );
          for ( int x = 3 ; x < columns - 3 ; x++ )
          {
               for ( int y = 3 ; y < rows - 3 ; y++ )
               {
                    stonePositions . Add ( new Vector3 ( x , y , 0f ) );
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
          int randomIndex = Random . Range ( 0 , gridPositions . Count );
          Vector3 randomPosition = gridPositions [ randomIndex ];
          gridPositions . RemoveAt ( randomIndex );
          return randomPosition;
     }

     Vector3 RandomStonePosition ( )
     {
          int randomIndex = Random . Range ( 0 , stonePositions . Count );
          Vector3 randomPosition = stonePositions [ randomIndex ];
          return randomPosition;
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

     void LayoutStoneAtRandom ( )
     {
          Vector3 randomPosition = RandomStonePosition ( );
          Instantiate ( stones , randomPosition , Quaternion . identity);
     }


     /// <summary>
     /// Calls Above Functions and adds the exit tile
     /// Called from GameManager InitGame() passing the level variable to it
     /// </summary>
     /// <param name="level"></param>
     public void SetupScene (int level)
     {
    	BoardSetup ( );
  		InitialiseList ( );
   		InitializeLocationList ( );

      	LayoutObjectAtRandom ( wallTiles , wallCount . minimum , wallCount . maximum );
      	LayoutObjectAtRandom ( foodTiles , foodCount . minimum , foodCount . maximum );

      	int enemyCount = ( int ) Mathf . Log ( level , 2f );                                                //Places Enemy 1
      	LayoutObjectAtRandom ( enemyTiles , enemyCount , enemyCount );

      	int enemy2Count = ( int ) Mathf . Log ( level , 2f );                                               //Places Enemy 2
      	LayoutObjectAtRandom ( enemy2Tiles , enemy2Count , enemy2Count );

      	Instantiate ( exit , new Vector3 ( columns - 1 , rows - 5 , 0f ) , Quaternion . identity );         
		Instantiate ( exit_P2 , new Vector3 ( columns - columns, rows - 5 , 0f ) , Quaternion . identity ); 

      	LayoutStoneAtRandom ( );      
     }
}
