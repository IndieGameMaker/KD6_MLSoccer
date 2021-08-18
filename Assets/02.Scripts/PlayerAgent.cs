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
        bps = GetComponent<BehaviorParameters>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        bps.TeamId = (int)team;
        GetComponent<Renderer>().material = materials[bps.TeamId];
    }

    public override void OnEpisodeBegin()
    {
        InitPlayer();
    }


}
