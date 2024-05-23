using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private GameManager gm;

    public float rotationSpeed = 15f;

    private CharacterController controller;
    private Animator animator;
    public bool isOutside;

    
    
    public Vector3 mazeMinBounds = new Vector3(-9.6f, 0, -4.5f);
    public Vector3 mazeMaxBounds = new Vector3(0.6f, 0, 5.5f);

    public GameManager.Difficulty gameDifficulty;

    public bool canMove;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        StartCoroutine(levelLoader());
    }

    IEnumerator levelLoader()
    {
        print("Loading difficulty");
        yield return new WaitForSeconds(1);
        gameDifficulty = gm.CurrentDifficulty;
        print("player level " + gameDifficulty);
        yield return null;

    }
    private void Update()
    {



        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;


        if (moveDirection.magnitude >= 0.1f)
        {
            Vector3 movement = moveDirection * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            controller.Move(movement);
            animator.SetBool("Run_b", true);
        }

        else animator.SetBool("Run_b", false);

        //small maze x -0.6 9.5 z -0.6 9.5
        //mid maze x -0.6 14.6 z -0.6 14.6 
        //big  x  -0.6 19.6 z -0.6 19.6

        if (gameDifficulty == GameManager.Difficulty.Easy)
        {
            if (transform.position.x < -0.6 || transform.position.x > 9.6 || transform.position.z < -0.6 || transform.position.z > 9.6)
            {
                gm.GameOver(false);
            }
        }

        else if (gameDifficulty == GameManager.Difficulty.Medium)
        {
            if (transform.position.x < -0.6 || transform.position.x > 14.6 || transform.position.z < -0.6 || transform.position.z > 14.6)
            {
                gm.GameOver(false);
            }
        }

        else if (gameDifficulty == GameManager.Difficulty.Hard)
        {
            if (transform.position.x < -0.6 || transform.position.x > 19.6 || transform.position.z < -0.6 || transform.position.z > 19.6)
            {
                gm.GameOver(false);
            }
        }




    }

    private void OnEnable()
    {
        transform.position = new Vector3(0, 0.4f, 0);
        transform.rotation = Quaternion.identity;
    }

}
