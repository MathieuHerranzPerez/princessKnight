﻿using UnityEngine;

public abstract class PlayerInputs : MonoBehaviour
{

    public float Horizontal { get { return axisX; } }
    public float Verical { get { return axisY; } }
    public Vector3 Swipe { get { return swipeDirection; } }
    public bool A { get { return isAPressed; } }

    // ---- INTERN ----
    protected float axisX = 0f;
    protected float axisY = 0f;
    protected Vector3 swipeDirection = Vector3.zero;
    protected bool isAPressed = false;
}
