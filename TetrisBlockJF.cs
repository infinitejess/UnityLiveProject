using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisBlockJF : MonoBehaviour
{
    public Vector3 rotationPoint; //variable for rotating blocks
    private float previousTime; //global varaible used for vertical/horizontal movement
    public float fallTime = 0.8f;  //global varaible used for vertical/horizontal movement - time before the block moves down
    public static int height = 18; //global variable used to keep blocks in game screen (static so it's the same varibale for all blocks)
    public static int width = 10;  //global variable used to keep blocks in game screen (static so it's the same varibale for all blocks)
    private static Transform[,] grid = new Transform[width, height];  //array to store location on grid for collision

    // Start is called before the first frame update
    void Start()
    {
   

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))  //check if left arrow key input
        {
            transform.position += new Vector3(-1, 0, 0);  //shift to the left
            if (!ValidMove())
                transform.position -= new Vector3(-1, 0, 0); //check is move is valid, otherwise undo the movement
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))  //check if right arrow key input
        {
            transform.position += new Vector3(1, 0, 0);  //shift to the right
            if (!ValidMove())
                transform.position -= new Vector3(1, 0, 0); //check is move is valid, otherwise undo the movement
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))  //rotate with Up arrow
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90); //rotate 90 degrees with local position
            if (!ValidMove())
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90); 
        }

        if(Time.time - previousTime > (Input.GetKey(KeyCode.DownArrow) ? fallTime / 10 : fallTime))  //check if down arrow is pressed which returns falltime/10 or return fall time
        {
            transform.position += new Vector3(0, -1, 0); //makes block fall
            if (!ValidMove())
            {
                transform.position -= new Vector3(0, -1, 0); //check is move is valid, otherwise undo the movement
                AddToGrid(); //when the blocks hit the ground, add to grid
                CheckForLines(); //call check lines function
                this.enabled = false;
                FindObjectOfType<SpawnTetromino>().NewTetromino(); //when it can no longer move vertically, spawn new
            }
            previousTime = Time.time; //prevents it from falling forever, resets time
        }
    }

    void CheckForLines() //checking all grid positions 
    {
        for(int i = height-1; i >= 0; i--)
        {
            if(HasLine(i)) //if there is a line
            {
                DeleteLine(i); //delete it if it has a line
                RowDown(i); //check next row
                LinesClearJF.LinesCleared += 1; //add one to the score in UI text
                if(LinesClearJF.LinesCleared == 5)
                {
                    SceneManager.LoadScene("JFWin");
                }
            }
        }
    }

    bool HasLine(int i)
    {
        for(int j = 0; j < width; j++) //check all elements
        {
            if (grid[j, i] == null) //check for null in grid
                return false;
        }
        return true; 
    }

    void DeleteLine(int i)
    {
        for(int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject); //delete the line
            grid[j, i] = null; //assign grid new after
        }
    }

    void RowDown(int i)
    {
        for(int y = i; y < height; y++)
        {
            for (int j = 0; j < width; j++)
            {
                if (grid[j, y] != null)
                {
                    grid[j, y - 1] = grid[j, y];
                    grid[j, y] = null;
                    grid[j, y - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void AddToGrid() //loop all children and add their transform to corresponding index
    {
        foreach(Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;  
        }
        CheckEndGame();
    }

    void CheckEndGame()
    {
        //for every block in the column
        for(int j = 0; j < width; j++)
        {
            //check to see if there are many blocks in the highest row
            if (grid[j,height-1] != null)
            {
                //if there are blocks at the top, the game is over
                GameOver();
            }
        }
    }

    public void GameOver()
    {
       SceneManager.LoadScene("JFGameOver");
    }

    bool ValidMove() //keeping the blocks from leaving the game area by checking position
    {
        foreach (Transform children in transform) //browse all children
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x); //round to x position
            int roundedY = Mathf.RoundToInt(children.transform.position.y); //round to y position

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height) //checking if bigger than background size
            {
                return false; //then return false
            }
            if (grid[roundedX, roundedY] != null) //check if position is taken by another child, if so return false
                return false;
        }
        return true;  //if none are outside return true
    }

}
