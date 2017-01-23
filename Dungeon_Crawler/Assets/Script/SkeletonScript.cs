using UnityEngine;
using System.Collections;

public class SkeletonScript : MonoBehaviour {

    enum Direction { TOP, RIGHT, BOTTOM, LEFT };

    //Enum declaration
    private Direction directionToGo = Direction.TOP;

    //Const declaration
    private const float TURNSPEED = 0.03f;
    private const float MOVSPEED = 0.1f;
    private const int NUMBEROFDIRECTION = 4;

    //GameObject declaration
    private GameObject player;
    private GameObject currentTile;
    private GameObject targetTile;
    private GameObject nothingTile;//The gameobject returned if you cant go to a tile(ex. there is a wall)

    //Bool declaration
    private bool isPlayerTurn;
    private bool isTurning = false;
    private bool isTimeToMove = false;
    private bool isDoingaction = false;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        nothingTile = GameObject.FindGameObjectWithTag("Empty");
    }
	
	// Update is called once per frame
	void Update ()
    {
        isPlayerTurn = player.GetComponent<PlayerScript>().GetPlayerTurn();//Check if the player made his turn

        if (isTimeToMove == true || isTurning == true)
        {
            isDoingaction = true;
        }
        else
        {
            isDoingaction = false;
        }
        
        if (isPlayerTurn == false && isDoingaction == false)
        {
            AIDecision();
        }

        if (isTurning == true)
        {
            RotateTowardObject(targetTile);
        }

        if (isTimeToMove)
        {
            //Debug.Log(targetTile.name);
            MoveToTile(targetTile);
        }
    }

    void AIDecision()
    {
        if (IsPlayerAround() == true)
        {
            //RotateTowardObject(player);
            player.GetComponent<PlayerScript>().SetPlayerTurn(true);
        }
        else
        {
            targetTile = ChooseTileToMove();
            targetTile = CheckIfTileIsRight(targetTile);
            isTurning = true;
        }
    }

    private GameObject ChooseTileToMove()
    {
        GameObject tileChosen;

        if (player.transform.position.x >= (transform.position.x - 1) && player.transform.position.x <= (transform.position.x + 1))//Check if the player is in the same vertical row to the enemy
        {
            if (player.transform.position.z < transform.position.z)//If the player is to the bottom of you go bottom
            {
                tileChosen = currentTile.GetComponent<TileScript>().GetBottomTile();
                directionToGo = Direction.BOTTOM;
            }
            else
            {
                tileChosen = currentTile.GetComponent<TileScript>().GetTopTile();
                directionToGo = Direction.TOP;
            }
        }
        else
        {
            if (player.transform.position.x < transform.position.x)//If the player is to the left of you go left
            {
                tileChosen = currentTile.GetComponent<TileScript>().GetLeftTile();
                directionToGo = Direction.LEFT;
            }
            else
            {
                tileChosen = currentTile.GetComponent<TileScript>().GetRightTile();
                directionToGo = Direction.RIGHT;
            }
        }

        return tileChosen;
    }

    private GameObject CheckIfTileIsRight(GameObject tile)
    {
        GameObject rightTile = tile;

        for (int i = 0; i < 4; i++)//Find a direction till he find a valid one
        {
            if (rightTile == nothingTile)
            {
                directionToGo = FindNextDirection(directionToGo);
                rightTile = GetDirectionTile(directionToGo);
            }
            else
            {
                break;
            }
        }

        return rightTile;
    }

    void RotateTowardObject(GameObject targetObject)//Rotate the ennemy to an object beforemoving it
    {
        Vector3 targetDir = targetObject.transform.position - transform.position;
        Vector3 targetVecor = new Vector3(targetDir.x, 0, targetDir.z);
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetVecor, TURNSPEED, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);

        //Over complicated if to tell if its done turning
        if ((transform.forward.x >= (targetVecor.normalized.x - 0.01) && transform.forward.x <= (targetVecor.normalized.x + 0.01)) && (transform.forward.z >= (targetVecor.normalized.z - 0.01) && transform.forward.z <= (targetVecor.normalized.z + 0.01)))
        {
            isTurning = false;
            isTimeToMove = true;
        }
    }

    void MoveToTile(GameObject targetObject)//Move the ennemy to the targetted tile
    {
        transform.position = Vector3.MoveTowards(transform.position, targetObject.transform.position, MOVSPEED);
        if (transform.position == targetObject.transform.position)
        {
            currentTile.GetComponent<TileScript>().SetEnemyOnMe(false);//Enemy stop being on the last tile...
            isTimeToMove = false;
            currentTile = targetObject;
            currentTile.GetComponent<TileScript>().SetEnemyOnMe(true);//...and apear on the destination
            player.GetComponent<PlayerScript>().SetPlayerTurn(true);//End the enemy turn
        }
    }

    Direction FindNextDirection(Direction currentDirection)
    {
        switch (currentDirection)
        {
            case Direction.TOP:
                currentDirection = Direction.RIGHT;
                break;
            case Direction.RIGHT:
                currentDirection = Direction.BOTTOM;
                break;
            case Direction.BOTTOM:
                currentDirection = Direction.LEFT;
                break;
            case Direction.LEFT:
                currentDirection = Direction.TOP;
                break;
        }
        return currentDirection;
    }

    GameObject GetDirectionTile(Direction currentDirection)
    {
        GameObject targetTile = currentTile;

        switch (currentDirection)
        {
            case Direction.TOP:
                targetTile = currentTile.GetComponent<TileScript>().GetTopTile();
                break;
            case Direction.RIGHT:
                targetTile = currentTile.GetComponent<TileScript>().GetRightTile();
                break;
            case Direction.BOTTOM:
                targetTile = currentTile.GetComponent<TileScript>().GetBottomTile();
                break;
            case Direction.LEFT:
                targetTile = currentTile.GetComponent<TileScript>().GetLeftTile();
                break;
        }
        return targetTile;
    }

    bool IsPlayerAround()
    {
        Direction directionCheck = Direction.TOP;
        GameObject tileToCheck;

        for (int i = 0; i < NUMBEROFDIRECTION; i++)
        {
            directionCheck = FindNextDirection(directionCheck);
            tileToCheck = GetDirectionTile(directionCheck);

            if (tileToCheck.GetComponent<TileScript>().GetIsPlayerOnTile() == true)//If there is a player on that tile
            {
                return true;
            }
        }
        return false;
    }

    public void SetCurrentTile(GameObject tile)
    {
        currentTile = tile;
    }
}
