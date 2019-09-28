using UnityEngine;

public class ControllerInputs : PlayerInputs
{
    void Update()
    {
        float axisXTmp = Input.GetAxis("AxisX");
        float axisYTmp = Input.GetAxis("AxisY");

        Vector2 tmp = new Vector2(axisXTmp, axisYTmp);
        tmp.Normalize();
        axisX = tmp.x;
        axisY = tmp.y;

        float rightXMov = Input.GetAxis("rightAxisX");
        float rightYMov = Input.GetAxis("rightAxisY");

        swipeDirection = Vector3.zero;
        // can compare to 0 (there is a dead value)
        if (rightXMov != 0 || rightYMov != 0)
        {
            Vector3 dir = new Vector3(rightXMov, 0f, rightYMov);
            dir.Normalize();
            swipeDirection = dir;
        }

        isAPressed = Input.GetButtonDown("btnA");
    }
}
