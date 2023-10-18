/*sing System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 15.0f;

    private bool isTravelling;
    private Vector3 travelDirection;
    private Vector3 nextCollissionPosition;

    public int minSwipeRecognition = 500;
    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private Color solveColor;

    private void Start()
    {
        // Ensure Rigidbody is referenced
        rb = GetComponent<Rigidbody>();
        solveColor = Random.ColorHSV(0.5f, 1);
        GetComponent<MeshRenderer>().material.color = solveColor;
    }


    private void FixedUpdate()
    {
        if (isTravelling)
        {   //addForce
            rb.velocity = speed * travelDirection;
        }

        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), 0.05f);
        int i = 0;
        while (i < hitColliders.Length)
        {

            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();
            if (ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }
            i++;
        }

        if (nextCollissionPosition != Vector3.zero) //&& Vector3.Distance(transform.position
        {
            if (Vector3.Distance(transform.position, nextCollissionPosition) < 1)
            {
                 isTravelling = false;
                 travelDirection = Vector3.zero;
                 nextCollissionPosition = Vector3.zero;
            }
        }

        /*if (nextCollissionPosition != Vector3.zero && Vector3.Distance(transform.position, nextCollissionPosition) < 1)
        {
            StopBallMovement();
            travelDirection = Vector3.zero;
            nextCollissionPosition = Vector3.zero;
        }*/

/*        if (isTravelling)
        {
            return;
        }


        if (Input.GetMouseButton(0))
        {
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePosLastFrame != Vector2.zero)
            {
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition)
                {
                    return;
                }

                currentSwipe.Normalize();

                //Up/Down
                if (currentSwipe.x > -0.5 && currentSwipe.x < 0.5)
                {
                    //Go Up/Down
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                if (currentSwipe.y > -0.5 && currentSwipe.y < 0.5)
                {
                    //Go Left/Right
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);

                }
            }

            swipePosLastFrame = swipePosCurrentFrame;
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }
*/
/*private void StopBallMovement()
{
    isTravelling = false;
    rb.velocity = Vector3.zero;
    rb.angularVelocity = Vector3.zero;
}*/

/*    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollissionPosition = hit.point;
        }

        isTravelling = true;
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 15;

    public int minSwipeRecognition = 500;

    private bool isTraveling;
    private Vector3 travelDirection;

    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private Vector3 nextCollisionPosition;

    private Color solveColor;

    public AudioClip ballSwipe, bgSound;
    private AudioSource audioPlayer;

    private void Start()
    {
        solveColor = Random.ColorHSV(.5f, 1); // Only take pretty light colors
        GetComponent<MeshRenderer>().material.color = solveColor;
        audioPlayer = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        // Set the balls speed when it should travel
        if (isTraveling)
        {
            rb.velocity = travelDirection * speed;
        }

        // Paint the ground
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), .05f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();

            if (ground && !ground.isColored)
            {
                ground.ChangeColor(solveColor);
            }

            i++;
        }

        // Check if we have reached our destination
        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isTraveling)
            return;

        // Swipe mechanism
        if (Input.GetMouseButton(0))
        {
            audioPlayer.PlayOneShot(ballSwipe);
            // Where is the mouse now?
            swipePosCurrentFrame = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if (swipePosLastFrame != Vector2.zero)
            {

                // Calculate the swipe direction
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition) // Minium amount of swipe recognition
                    return;

                currentSwipe.Normalize(); // Normalize it to only get the direction not the distance (would fake the balls speed)

                // Up/Down swipe
                if (currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                // Left/Right swipe
                if (currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }
            }


            swipePosLastFrame = swipePosCurrentFrame;
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            swipePosLastFrame = Vector2.zero;
            currentSwipe = Vector2.zero;
        }
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        // Check with which object we will collide
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isTraveling = true;
    }
}
