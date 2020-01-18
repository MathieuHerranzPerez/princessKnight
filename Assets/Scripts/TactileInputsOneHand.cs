using UnityEngine;

public class TactileInputsOneHand : PlayerInputs
{
    [SerializeField] private float minDistanceForSwipe = 100f;
    [SerializeField] private float maxTimeForSwipe = 0.2f;

    [Header("Setup")]
    [SerializeField] private Transform joystickBounds = default;
    [SerializeField] private Transform joystickCenter = default;

    // ---- INTERN ----

    private int currentTouchId = -1;
    private float lastTouchTime;
    private Vector2 startingPoint;

    private float minDistanceForSwipeSquare;

    private void Start()
    {
        minDistanceForSwipeSquare = minDistanceForSwipe * minDistanceForSwipe;
        joystickBounds.gameObject.SetActive(false);
    }

    void Update()
    {
        axisX = 0f;
        axisY = 0f;
        swipeDirection = Vector3.zero;
        isAPressed = false;

#if UNITY_EDITOR
        UpdateEditor();
#else
        UpdateMobile();
#endif

    }

    private void UpdateEditor()
    {
        Vector2 mousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            startingPoint = mousePos;
            lastTouchTime = Time.time;

            joystickBounds.position = mousePos;
        }
        else if(Input.GetMouseButton(0))
        {
            Vector2 offset = mousePos - startingPoint;
            Vector2 direction = Vector2.ClampMagnitude(offset, 100f);
            axisX = direction.x / 100f;
            axisY = direction.y / 100f;

            if(Time.time - lastTouchTime >= maxTimeForSwipe)
            {
                joystickBounds.gameObject.SetActive(true);
                joystickCenter.position = new Vector2(joystickBounds.position.x + direction.x, joystickBounds.position.y + direction.y);
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if (Time.time - lastTouchTime < maxTimeForSwipe)
            {
                // it is a simple touch
                if (startingPoint == mousePos)
                {
                    isAPressed = true;
                }
                else
                {
                    Vector2 dashDir = DetectSwipe(mousePos);
                    // it is a dash
                    if (dashDir != Vector2.zero)
                    {
                        Vector3 dir = new Vector3(dashDir.x, 0f, dashDir.y);
                        swipeDirection = dir;
                    }
                }
            }

            joystickBounds.gameObject.SetActive(false);
        }
    }

    private void UpdateMobile()
    {
        int i = 0;
        while (i < Input.touchCount)
        {
            Touch touch = Input.GetTouch(i);
            Vector2 touchPos = touch.position;

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchTime = Time.time;
                currentTouchId = touch.fingerId;
                startingPoint = touchPos;

                joystickBounds.position = touchPos;
            }
            else if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) && currentTouchId == touch.fingerId)
            {
                Vector2 offset = touchPos - startingPoint;
                Vector2 direction = Vector2.ClampMagnitude(offset, 100f);
                axisX = direction.x / 100f;
                axisY = direction.y / 100f;

                if (Time.time - lastTouchTime >= maxTimeForSwipe)
                {
                    joystickBounds.gameObject.SetActive(true);
                    joystickCenter.position = new Vector2(joystickBounds.position.x + direction.x, joystickBounds.position.y + direction.y);
                }
            }
            else if ((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && currentTouchId == touch.fingerId)
            {
                currentTouchId = -1;

                if (Time.time - lastTouchTime < maxTimeForSwipe)
                {
                    // it is a simple touch
                    if (startingPoint == touchPos)
                    {
                        isAPressed = true;
                    }
                    else
                    {
                        Vector2 dashDir = DetectSwipe(touch.position);
                        // it is a dash
                        if (dashDir != Vector2.zero)
                        {
                            Vector3 dir = new Vector3(dashDir.x, 0f, dashDir.y);
                            swipeDirection = dir;
                        }
                    }
                }

                joystickBounds.gameObject.SetActive(false);
            }
            ++i;
        }
    }

    private Vector2 DetectSwipe(Vector2 lastPoint)
    {
        Vector2 direction = Vector2.zero;
        if (IsSwipeDistanceCorrect(lastPoint))
        {
            direction = (lastPoint - startingPoint);
        }

        return direction.normalized;
    }

    private bool IsSwipeDistanceCorrect(Vector2 lastPoint)
    {
        float squareDistance = (lastPoint.x - startingPoint.x) * (lastPoint.x - startingPoint.x) + (lastPoint.y - startingPoint.y) * (lastPoint.y - startingPoint.y);
        return squareDistance > minDistanceForSwipeSquare;
    }
}
