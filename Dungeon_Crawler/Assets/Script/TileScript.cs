using UnityEngine;
using System.Collections;

public class TileScript : MonoBehaviour {

    //Type of tile
    public enum TileType { Normal, CornerTopLeft, CornerTopRight, CornerBottomLeft, CornerBottomRight, WallRight, WallTop, WallBottom, WallLeft };

    //Skeleton reference
    public GameObject Skeleton;
    private GameObject skeletonRef;
    //Reference to the 4 tile around this tile
    public GameObject TileTop;
    public GameObject TileRight;
    public GameObject TileBottom;
    public GameObject TileLeft;

    //What occupy the tile
    private bool isPlayerOnMe = false;
    private bool isEnemyOnMe = false;

    //Player starting tile
    public bool PlayerStartingTile = false;
    public bool EnemySpawn = false;

    //Player reference
    private GameObject player;

    //Type of tile
    public TileType tileType;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (PlayerStartingTile == true)
        {
            player.transform.position = new Vector3(transform.position.x, (transform.position.y + 1), transform.position.z);
            player.GetComponent<PlayerScript>().SetCurrentTile(this.gameObject);
        }

        if (EnemySpawn == true)
        {
            skeletonRef = (GameObject)Instantiate(Skeleton, transform.position, Quaternion.identity);
            skeletonRef.GetComponent<SkeletonScript>().SetCurrentTile(this.gameObject);
        }
    }

    public GameObject GetTopTile()
    {
        return TileTop;
    }

    public GameObject GetRightTile()
    {
        return TileRight;
    }

    public GameObject GetBottomTile()
    {
        return TileBottom;
    }

    public GameObject GetLeftTile()
    {
        return TileLeft;
    }

    public bool GetIsPlayerOnTile() { return isPlayerOnMe; }//To check what is on the tile
    public bool GetIsEnemyOnTile() { return isEnemyOnMe; }

    public void SetPlayerOnMe(bool choice) { isPlayerOnMe = choice; }//To set what is on the tile
    public void SetEnemyOnMe(bool choice) { isEnemyOnMe = choice; }
}
