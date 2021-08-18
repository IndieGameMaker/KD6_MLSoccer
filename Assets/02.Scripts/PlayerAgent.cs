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

    private BehaviorParameters bps;
    private Transform tr;
    private Rigidbody rb;

    public override void Initialize()
    {
        bps = GetComponent<BehaviorParameters>();
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        bps.TeamId = (int)team;
    }

}
