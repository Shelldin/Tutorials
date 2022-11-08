using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGrid : MonoBehaviour
{
    public static MoveGrid instance;
    
    public MovePoint startPoint;
    
    public Vector2Int spawnRange;

    public LayerMask groundLayerCheck,
        obstacleLayerCheck;

    public float obstacleCheckRange = .4f;

    public List<MovePoint> allMovePoints = new List<MovePoint>();
    
    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        
        GenerateMovementGrid(); 
       
        HideMovePoints();
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //create the grid which units can move on
    public void GenerateMovementGrid()
    {
        //for loops determine length and width of grid
        for (int x = -spawnRange.x; x <= spawnRange.x; x++)
        {
            for (int y = -spawnRange.y; y <= spawnRange.x; y++)
            {
                RaycastHit hit;
                
                //raycast to determine if there is ground. if there's not, no move point is made
                if (Physics.Raycast(transform.position + new Vector3(x, 10f, y),
                        Vector3.down, out hit, 20f, groundLayerCheck))
                {
                    //overlap circle to determine if there are obstacles. If there are, no move point is made
                    if (Physics.OverlapSphere(hit.point, obstacleCheckRange, obstacleLayerCheck).Length == 0)
                    {
                        //if there is ground and no obstacles a move point is made
                        MovePoint newPoint = Instantiate(startPoint, hit.point,
                            transform.rotation);
                        //assign a parent to make editor more manageable
                        newPoint.transform.SetParent(transform); 
                        
                        //add move points to a list so we can hide them in the game
                        allMovePoints.Add(newPoint);
                    }
                }
            }
        }
        //set initial star point inactive so there aren't 2 overlapping when the grid is generated
        startPoint.gameObject.SetActive(false);
    }
    //hide each move point in the allMovePoints list
    public void HideMovePoints()
    {
        foreach (MovePoint movePoint in allMovePoints)
        {
            movePoint.gameObject.SetActive(false);
        }
    }

    public void ShowPointsInRange(float moveRange, Vector3 centerPoint)
    {
        HideMovePoints();

        foreach (MovePoint movePoint in allMovePoints)
        {
            if (Vector3.Distance(centerPoint, movePoint.transform.position) <= moveRange)
            {
                movePoint.gameObject.SetActive(true);

                foreach (CharacterController charCon in GameManager.instance.allCharacters)
                {
                    if (Vector3.Distance(charCon.transform.position, movePoint.transform.position) < .5f)
                    {
                        movePoint.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
