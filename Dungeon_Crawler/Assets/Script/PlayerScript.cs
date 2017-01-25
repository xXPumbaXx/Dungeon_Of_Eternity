using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    enum Frontdirection { TOP, RIGHT,BOTTOM, LEFT };

    //Bool declaration
    private bool isDoingAction = false;
    private bool isTurningRight = true;
    private bool isTurning = false;
    private bool isMoving = false;
    private bool isPlayerTurn = true;

    //Float declaration
    private float targetTurningAngle;

    //GameObject declaration
    private GameObject currentTile;
    private GameObject tileDestination;
    private GameObject nothingTile;//The gameobject returned if you cant go to a tile(ex. there is a wall)
    //the 4 tile around the current tile
    private GameObject TileTop;
    private GameObject TileRight;
    private GameObject TileBottom;
    private GameObject TileLeft;

    //Const declaration
    private const float TURNSPEED = 3;
    private const float MOVSPEED = 0.1f;

    //Declaration de frontDirection
    private Frontdirection forward = Frontdirection.TOP;

    // Use this for initialization
    void Start ()
    {
        tileDestination = currentTile;
        nothingTile = GameObject.FindGameObjectWithTag("Empty");
	}
	
	// Update is called once per frame
	void Update ()
    {
        UpdateDirection();
        CheckForEnnemy();

        if (isMoving == true || isTurning == true)//Check if the player is doing an action so 2 action cant be made at the same time
        {
            isDoingAction = true;
        }
        else
        {
            isDoingAction = false;
        }

        ListenToKey();

        if (isTurning == true)
        {
            StartTurning();
        }

        if (isMoving == true)
        {
            MoveToTile();
        }
	}

    void CheckForEnnemy()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            isPlayerTurn = true;
        }
    }

    void ListenToKey()
    {
        if (Input.GetKeyDown("q") && isDoingAction == false && isPlayerTurn == true)
        {
            isTurningRight = true;
            isTurning = true;
            targetTurningAngle = transform.rotation.eulerAngles.y - 90;
            if (targetTurningAngle < 0)
            {
                targetTurningAngle += 360;
            }
            DetermineDirection();
        }

        if (Input.GetKeyDown("e") && isDoingAction == false && isPlayerTurn == true)
        {
            isTurningRight = false;
            isTurning = true;
            targetTurningAngle = transform.rotation.eulerAngles.y + 90;
            DetermineDirection();
        }

        if (Input.GetKeyDown("w") && isDoingAction == false && isPlayerTurn == true)
        {
            tileDestination = TileTop;
            if (tileDestination != nothingTile && tileDestination.GetComponent<TileScript>().GetIsEnemyOnTile() == false)//Making the move
            {
                isMoving = true;
            }
        }

        if (Input.GetKeyDown("d") && isDoingAction == false && isPlayerTurn == true)
        {
            tileDestination = TileRight;
            if (tileDestination != nothingTile && tileDestination.GetComponent<TileScript>().GetIsEnemyOnTile() == false)//Making the move
            {
                isMoving = true;
            }
        }

        if (Input.GetKeyDown("s") && isDoingAction == false && isPlayerTurn == true)
        {
            tileDestination = TileBottom;
            if (tileDestination != nothingTile && tileDestination.GetComponent<TileScript>().GetIsEnemyOnTile() == false)//Making the move
            {
                isMoving = true;
            }
        }

        if (Input.GetKeyDown("a") && isDoingAction == false && isPlayerTurn == true)
        {
            tileDestination = TileLeft;
            if (tileDestination != nothingTile && tileDestination.GetComponent<TileScript>().GetIsEnemyOnTile() == false)//Making the move
            {
                isMoving = true;
            }
        }

        if (Input.GetKeyDown("space") && isDoingAction == false && isPlayerTurn == true)
        {
            isPlayerTurn = false;
        }
    }

    void StartTurning()
    {
        float currentAngle = transform.rotation.eulerAngles.y;

        if (isTurningRight == true)
        {
            currentAngle -= 1 * TURNSPEED;
        }
        else
        {
            currentAngle += 1 * TURNSPEED;
        }
        
        transform.eulerAngles = new Vector3(0, currentAngle, 0);

        if (currentAngle >= (targetTurningAngle - 1) && currentAngle <= (targetTurningAngle + 1))
        {
            transform.eulerAngles = new Vector3(0, targetTurningAngle, 0);
            isTurning = false;
        }
    }

    void MoveToTile()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(tileDestination.transform.position.x, (tileDestination.transform.position.y + 1), tileDestination.transform.position.z), MOVSPEED);
        if (transform.position == new Vector3(tileDestination.transform.position.x, (tileDestination.transform.position.y + 1), tileDestination.transform.position.z))//What happen when he is done moving
        {
            currentTile.GetComponent<TileScript>().SetPlayerOnMe(false);//Player stop being on the last tile...
            isMoving = false;
            currentTile = tileDestination;
            currentTile.GetComponent<TileScript>().SetPlayerOnMe(true);//...and apear on the destination
            isPlayerTurn = false;
        }
    }

    void DetermineDirection()
    {
        switch ((int)targetTurningAngle)
        {
            case 0:
                forward = Frontdirection.TOP;
                break;
            case 90:
                forward = Frontdirection.RIGHT;
                break;
            case 180:
                forward = Frontdirection.BOTTOM;
                break;
            case 270:
                forward = Frontdirection.LEFT;
                break;
            case 360:
                forward = Frontdirection.TOP;
                break;
        }
    }

    void UpdateDirection()
    {
        switch (forward)
        {
            case Frontdirection.TOP:
                TileTop = currentTile.GetComponent<TileScript>().GetTopTile();
                TileRight = currentTile.GetComponent<TileScript>().GetRightTile();
                TileBottom = currentTile.GetComponent<TileScript>().GetBottomTile();
                TileLeft = currentTile.GetComponent<TileScript>().GetLeftTile();
                break;
            case Frontdirection.RIGHT:
                TileTop = currentTile.GetComponent<TileScript>().GetRightTile();
                TileRight = currentTile.GetComponent<TileScript>().GetBottomTile();
                TileBottom = currentTile.GetComponent<TileScript>().GetLeftTile();
                TileLeft = currentTile.GetComponent<TileScript>().GetTopTile();
                break;
            case Frontdirection.BOTTOM:
                TileTop = currentTile.GetComponent<TileScript>().GetBottomTile();
                TileRight = currentTile.GetComponent<TileScript>().GetLeftTile();
                TileBottom = currentTile.GetComponent<TileScript>().GetTopTile();
                TileLeft = currentTile.GetComponent<TileScript>().GetRightTile();
                break;
            case Frontdirection.LEFT:
                TileTop = currentTile.GetComponent<TileScript>().GetLeftTile();
                TileRight = currentTile.GetComponent<TileScript>().GetTopTile();
                TileBottom = currentTile.GetComponent<TileScript>().GetRightTile();
                TileLeft = currentTile.GetComponent<TileScript>().GetBottomTile();
                break;
        }
    }

    public void SetCurrentTile(GameObject tile)
    {
        currentTile = tile;
    }

    public void SetPlayerTurn(bool playerTurn)
    {
        isPlayerTurn = playerTurn;
    }

    public bool GetPlayerTurn()
    {
        return isPlayerTurn;
    }
}
