using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicAI : MonoBehaviour
{
    //a reference to our target (players1 tank)
    public Transform target;
    public Transform myTransform;
    public float movementSpeed = 1;
    public Game game;
    public float nextWaypointDistance = 20f;
    float angle;
    int delay;

    Path path;
    int currentWaypoint;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2d;

    [HideInInspector]
    public TileCreation selectedTile;		//The tile that the player is currently hovering over.
    public SpriteRenderer ghostTile;		//The translucent tile which shows the tile that the player's cursor is on
    public float stoppingDistance = 3f;
    public float retreatDistance = 5f;
    public float inRangeDistance = 5f;
    public float distance,Xdif,Ydif;
    public Vector2 Playerdirection;
    public Vector2 Player;
    //public Rigidbody2D rb2d;
    public float avoidingMultiplier;
    public bool avoid;
    //float rocketsPosibilityNum = Random.Range(-10.0f, 10.0f);

    // Start is called before the first frame update
    void Start()
    {
        //find our rigidbody
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        //scan for paths in case new obstacles have been spawned
        AstarPath.active.Scan();
        InvokeRepeating("UpdatePath", 0f, .5f);
    }

    void OnPathComplete(Path p)
    {
        if (p == null) throw new System.Exception("error");
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else Debug.Log(p.error);
    }
    
    void UpdatePath() 
    {
        if (seeker.IsDone()) 
        {
            seeker.StartPath(rb2d.position, target.position, OnPathComplete);
        }
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        //only 
        if (MenuUI.getFlag() == 2 || MenuUI.getFlag() == 4) {

            delay+=2;    //delay in bullet throwing means less bullets

            if (MenuUI.getFlag() == 2)
            {
                delay--; //a bit les delay in campaign
                if (path == null)
                {
                    return;
                }

                if (currentWaypoint >= path.vectorPath.Count)
                {
                    reachedEndOfPath = true;
                    return;
                }
                else
                {
                    reachedEndOfPath = false;
                }

                /*
                transform.position = Vector2.MoveTowards(transform.position, path.vectorPath[currentWaypoint], 10 * Time.deltaTime);

                Vector3 dir = path.vectorPath[currentWaypoint] - transform.position;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/

                Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2d.position).normalized;
                if (game.player2Tank.canMove)
                {
                    if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
                    {
                        Vector2 force = direction * 0.03f * Time.fixedDeltaTime;
                        rb2d.AddForce(force);
                    }

                }


                float distance = Vector2.Distance(rb2d.position, path.vectorPath[currentWaypoint]);

                if (distance < nextWaypointDistance)
                {
                    currentWaypoint++;
                }


                Vector3 dir = target.position - transform.position; //direction of the target relatively to the npc  
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 270; //angle needed to move towards the target
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); //rotate the npc towards the target
            }

            game.player2Tank.direction = transform.rotation * Vector3.up;

            if (delay > 55 && game.player2Tank.canShoot) { //if the tank can shoot

                float rocketsPosibilityNum = (int) Random.Range(-10.0f, 10.0f);

                if (game.player2Tank.numOfRockets > 0 && EnemyInRange() && (game.player2Score > 7 || game.player1Tank.health == 2 || rocketsPosibilityNum > 8))
                    game.player2Tank.ShootRocket();

                game.player2Tank.Shoot();
                delay = 0;
            }
        }
    }

    public bool EnemyInRange()
    {
        return Vector2.Distance(transform.position, target.position) <= inRangeDistance;
    }

}
