using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingShapeManager : MonoBehaviour
{
    CinemachineConfiner2D Confiner;

    public void Start()
    {
        Confiner = GetComponent<CinemachineConfiner2D>();
        GameObject levelObject = GameObject.Find("LevelCollider");
        PolygonCollider2D polygonCollider = levelObject.GetComponent<PolygonCollider2D>();
        Confiner.m_BoundingShape2D = polygonCollider;
    }
}
