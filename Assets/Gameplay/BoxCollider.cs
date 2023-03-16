using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BoxCollider : MonoBehaviour
{
    public GameObject WallClass;
    public GameObject TurretClass;

    public GameObject PublicVariablesHolder;
    private PublicVariables wallCountscript;

    public UnityEngine.UI.Button turretButton;
    public UnityEngine.UI.Button wallButton; 

    static bool wall = false;
    static bool turret = false;

    static int limitWall = 20;
    static int limitTurret = 4;

    static int n_walls = 0;
    static int n_turret = 0;

    void OnMouseDown()
    {
        if (wall && n_walls <= limitWall)
        {
            Debug.Log("Wall added");
            Instantiate(WallClass, transform.position, transform.rotation);
            Destroy(gameObject);
            n_walls += 1;
            wallCountscript.wallCount += 1;

        }
        else if (turret && n_turret <= limitTurret)
        {
            Debug.Log("Turret added");
            Instantiate(TurretClass, transform.position, transform.rotation);
            Destroy(gameObject);
            n_turret += 1;
            wallCountscript.turretCount += 1;
        }
        else
        {
            Debug.Log("nothing");
        }
    }

    void Start()
    {
        wallCountscript = PublicVariablesHolder.GetComponent<PublicVariables>();
        
        UnityEngine.UI.Button turretButtonScr = turretButton.GetComponent<UnityEngine.UI.Button>();
        turretButtonScr.onClick.AddListener(ChangeToTurret);
        
        UnityEngine.UI.Button wallButtonScr = wallButton.GetComponent<UnityEngine.UI.Button>();
        wallButtonScr.onClick.AddListener(ChangeToWall);

    }

    public void ChangeToTurret()
    {
        Debug.Log("Changed turret");
        turret = true;
        wall = false;
    }

    public void ChangeToWall()
    {
        Debug.Log("Changed wall");
        turret = false;
        wall = true;
    }

    void Update()
    {
        
      
            
        //if (Input.GetKey("a"))
        //{
        //    turret = false;
        //    wall = true;
        //}

        //if (Input.GetKey("d"))
        //{
        //    turret = true;
        //    wall = false;
        //}
    }
}
