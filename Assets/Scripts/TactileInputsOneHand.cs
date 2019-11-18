using UnityEngine;

public class TactileInputsOneHand : PlayerInputs
{

    [Header("Setup")]
    [SerializeField]
    private Joystick joystick = default;
    [SerializeField]
    private float minDistanceForSwipe = 2f;
    [SerializeField]
    private float maxTimeForSwipe = 0.15f;

    // ---- INTERN ----
    private Touch currentTouch;
    private int touchId;

    private float timeTouching = 0f;

    private Vector2 fingerUpPosition;
    private Vector2 fingerDownPosition;


    void Update()
    {
        axisX = joystick.Horizontal;
        axisY = joystick.Vertical;
        if (Input.touchCount > 0)
        {
            currentTouch = Input.GetTouch(0);
            timeTouching += Time.deltaTime;
            if (currentTouch.phase == TouchPhase.Began)
            {
                fingerUpPosition = currentTouch.position;
                fingerDownPosition = currentTouch.position;
                timeTouching = 0f;
            }

            if (currentTouch.phase == TouchPhase.Ended)
            {
                fingerDownPosition = currentTouch.position;
                if(timeTouching > maxTimeForSwipe)
                {

                }
                else
                {
                    axisX = 0f;
                    axisY = 0f;
                    Vector2 dashDir = DetectSwipe();
                    if (dashDir != Vector2.zero)
                    {
                        Vector3 dir = new Vector3(dashDir.x, 0f, dashDir.y);
                        swipeDirection = dir;
                    }
                    else
                    {
                        swipeDirection = Vector3.zero;
                        // it is a simple touch
                        isAPressed = true;
                    }
                }
            }
            else
            {
                swipeDirection = Vector3.zero;
                isAPressed = false;
            }
        }
        else
        {
            timeTouching = 0f;
            // reset all
            swipeDirection = Vector3.zero;
            isAPressed = false;
        }
    }
    

    private Vector2 DetectSwipe()
    {
        Vector2 direction = Vector2.zero;
        if (timeTouching < maxTimeForSwipe && IsSwipeDistanceCorrect())
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
