using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class Controls : MonoBehaviour
{
    [Header("Player 1 Controls")]
    public KeyCode p1MoveForward;
    public KeyCode p1MoveBackwards;
    public KeyCode p1TurnLeft;
    public KeyCode p1TurnRight;
    public KeyCode p1Shoot;
    public KeyCode p1ShootRocket;

    [Header("Player 2 Controls")]
    public KeyCode p2MoveForward;
    public KeyCode p2MoveBackwards;
    public KeyCode p2TurnLeft;
    public KeyCode p2TurnRight;
    public KeyCode p2Shoot;
    public KeyCode p2ShootRocket;
    public GameObject player;

    [Header("Components")]
    public Game game;

    void Update()
    {
        //Player 1
        if (SceneManager.GetActiveScene().name != "Space")
            game.player1Tank.rig.velocity = Vector2.zero;
        //else object will continue moving as in space there is less force to stop it

        if (game.player1Tank.canMove)
        {
            if (Input.GetKey(p1MoveForward)) {
                game.player1Tank.Move(1);
            }
            if (Input.GetKey(p1MoveBackwards)) {
                game.player1Tank.Move(-1);
            }
            if (Input.GetKey(p1TurnLeft)) {
                game.player1Tank.Turn(-1);
            }
            if (Input.GetKey(p1TurnRight)) {
                game.player1Tank.Turn(1);
            }
        }

        if (game.player1Tank.canShoot)
        {
            if (Input.GetKeyDown(p1Shoot)) {
                if (SceneManager.GetActiveScene().name == "Space")
                    game.player1Tank.ShootLazer();
                else game.player1Tank.Shoot();
            }
            if (Input.GetKeyDown(p1ShootRocket)) {
                game.player1Tank.ShootRocket();
            }
        }

        //Player 2

        // if (MenuUI.getFlag() != 2)
        if (MenuUI.getFlag() == 3)
        {
            if (SceneManager.GetActiveScene().name != "Space")
                game.player2Tank.rig.velocity = Vector2.zero;
            //else object will continue moving as in space there is less force to stop it

            if (game.player2Tank.canMove)
            {
                if (Input.GetKey(p2MoveForward)) {
                    game.player2Tank.Move(1);
                }
                if (Input.GetKey(p2MoveBackwards)) {
                    game.player2Tank.Move(-1);
                }
                if (Input.GetKey(p2TurnLeft)) {
                    game.player2Tank.Turn(-1);
                }
                if (Input.GetKey(p2TurnRight)) {
                    game.player2Tank.Turn(1);
                }
            }

            if (game.player2Tank.canShoot)
            {
                if (Input.GetKeyDown(p2Shoot)) {
                    if (SceneManager.GetActiveScene().name == "Space")
                        game.player2Tank.ShootLazer();
                    else game.player2Tank.Shoot();
                }
                if (Input.GetKeyDown(p2ShootRocket)) {
                    game.player2Tank.ShootRocket();
                }
            }
        }
    }

}
