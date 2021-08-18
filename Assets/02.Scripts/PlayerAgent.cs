using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;  // Behaviour Parameters

public class PlayerAgent : Agent
{
    public enum TEAM
    {
        BLUE, RED
    }

    public TEAM team = TEAM.BLUE;
    public Material[] materials;
    //Player's Init Position, Rotation
    private Vector3 initBluePos = new Vector3(-5.5f, 0.5f, 0.0f);
    private Vector3 initRedPos = new Vector3(5.5f, 0.5f, 0.0f);
    private Quaternion initBlueRot = Quaternion.Euler(Vector3.up * 90.0f);
    private Quaternion initRedRot = Quaternion.Euler(-Vector3.up * 90.0f);

    private BehaviorParameters bps;
    private Transform tr;
    private Rigidbody rb;

    void InitPlayer()
    {
        tr.localPosition = (team == TEAM.BLUE) ? initBluePos : initRedPos;
        tr.localRotation = (team == TEAM.BLUE) ? initBlueRot : initRedRot;
    }

    public override void Initialize()
    {
        MaxStep = 10000;

        bps = GetComponent<BehaviorParameters>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        bps.TeamId = (int)team;
        GetComponent<Renderer>().material = materials[bps.TeamId];
    }

    public override void OnEpisodeBegin()
    {
        InitPlayer();
        rb.velocity = rb.angularVelocity = Vector3.zero;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*
            전후이동 : 정지, 전진, 후진  (0, 1, 2)  (Non, W, S)
            좌우이동 : 정지, 왼쪽, 오른쪽 (0, 1, 2)  (Non, Q, E)
            좌우회전 : 정지, 왼쪽, 오른쪽 (0, 1, 2)  (Non, A, D)
        */

        var actions = actionsOut.DiscreteActions;
    }

}
