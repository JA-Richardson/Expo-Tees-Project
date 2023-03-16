using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTileSpawner : MonoBehaviour
{ 
    public GameObject TileClass;
    public GameObject WallClass;
    public GameObject TurretClass;
    private Vector3 TilePosition = new Vector3(-95,0,-95);

    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < 19; j++)
        {
            for (int i = 0; i < 19; i++)
            {
                if (i <= 6 || i >= 12 || j <= 6 || j >= 12)
                {                    
                    if ((i == 0 && j != 9) || (i == 18 && j != 9) || (j == 0 && i != 9) || (j == 18 && i != 9))
                    {
                        Instantiate(WallClass, TilePosition, transform.rotation);
                    }
                    else
                    {
                        if((i != 9 || j != 0) && (i != 10 || j != 0) && (i != 9 || j != 18) && (i != 10 || j != 18) && (j != 9 || i != 0) && (j != 10 || i != 0) && (j != 9 || i != 18) && (j != 10 || i != 18))
                        {
                            Instantiate(TileClass, TilePosition, transform.rotation);
                        }
                    }
                }
               TilePosition.z  = TilePosition.z + 10;
            }
            TilePosition.z = -95;
            TilePosition.x = TilePosition. x + 10;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}