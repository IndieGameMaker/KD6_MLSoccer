using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using TMPro;

public class BallCtrl : MonoBehaviour
{
    public TMP_Text blueScoreText, redScoreText;
    private int blueScore, redScore;

    public Agent[] players;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void InitBall()
    {
        rb.velocity = rb.angularVelocity = Vector3.zero;
        transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
    }
}
