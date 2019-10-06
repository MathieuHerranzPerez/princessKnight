using UnityEngine;

public class TactileInputs : PlayerInputs
{

    [Header("Setup")]
    [SerializeField]
    private Joystick joystick = default;
    [SerializeField]
    private float minDistanceForSwipe = 2f;

    // ---- INTERN ----
    private Touch leftFinger;
    private Touch rightFinger;
    private bool leftFingerPress = false;

    private Vector2 fingerUpPosition;
    private Vector2 fingerDownPosition;


    void Update()
    {
        // determine the fingers
        if (Input.touchCount > 0)
        {
            if (Input.touches[0].position.x < Screen.width / 2)
            {
                leftFinger = Input.touches[0];
                if (Input.touchCount > 1)
                {
                    rightFinger = Input.touches[1];
                }
            }
            else
            {
                rightFinger = Input.touches[0];
                if (Input.touchCount > 1)
                {
                    leftFinger = Input.touches[1];
                }
            }
        }


        // Vector3 moveDirection = Vector3.forward * joystick.Vertical + Vector3.right * joystick.Horizontal;
        //moveDirection.Normalize();
        //axisX = moveDirection.x;
        //axisY = moveDirection.z;
        axisX = joystick.Horizontal;
        axisY = joystick.Vertical;


        // detect swipe
        if (Input.touchCount > 0)
        {
            if (rightFinger.phase == TouchPhase.Began)
            {
                fingerUpPosition = rightFinger.position;
                fingerDownPosition = rightFinger.position;
                leftFingerPress = true;
            }

            if (leftFingerPress && rightFinger.phase == TouchPhase.Ended)
            {
                fingerDownPosition = rightFinger.position;
                Vector2 dashDir = DetectSwipe();

                if (dashDir != Vector2.zero)
                {
                    Debug.Log("Dash !");
                    Vector3 dir = new Vector3(dashDir.x, 0f, dashDir.y);
                    swipeDirection = dir;
                }
                else
                {
                    swipeDirection = Vector3.zero;
                    // it is a simple touch
                    Debug.Log("Attack !");
                    isAPressed = true;
                }

                leftFingerPress = false;
            }
            else
            {
                swipeDirection = Vector3.zero;
                isAPressed = false;
            }
        }
        else
        {
            // reset all
            swipeDirection = Vector3.zero;
            isAPressed = false;
        }
    }
    

    private Vector2 DetectSwipe()
    {
        Vector2 direction = Vector2.zero;
        if (IsSwipeDistanceCorrect())
        {
            direction = (fingerDownPosition - fingerUpPosition);
        }

        fingerUpPosition = fingerDownPosition;

        return direction.normalized;
    }

    private bool IsSwipeDistanceCorrect()
    {
        float squareDistance = (fingerDownPosition.x - fingerUpPosition.x) * (fingerDownPosition.x - fingerUpPosition.x) + (fingerDownPosition.y - fingerUpPosition.y) * (fingerDownPosition.y - fingerUpPosition.y);
        return squareDistance > minDistanceForSwipe * minDistanceForSwipe;
    }
}
