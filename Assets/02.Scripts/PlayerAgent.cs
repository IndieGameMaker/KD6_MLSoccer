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
        actions.Clear();

        // Branch 0 전진/후진
        if (Input.GetKey(KeyCode.W)) actions[0] = 1;
        if (Input.GetKey(KeyCode.S)) actions[0] = 2;

        // Branch 1 좌/우 이동
        if (Input.GetKey(KeyCode.Q)) actions[1] = 1;
        if (Input.GetKey(KeyCode.E)) actions[1] = 2;

        // Branch 2 좌/우 회전
        if (Input.GetKey(KeyCode.A)) actions[2] = 1;
        if (Input.GetKey(KeyCode.D)) actions[2] = 2;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var action = actions.DiscreteActions;
        // Debug.LogFormat("[0]={0}, [1]={1}, [2]={2}",
        //                 action[0],
        //                 action[1],
        //                 action[2]);

        Vector3 dir = Vector3.zero;
        Vector3 rot = Vector3.zero;

        switch (action[0])
        {
            case 1: dir = tr.forward; break;
            case 2: dir = -tr.forward; break;
        }

        switch (action[1])
        {
            case 1: dir = -tr.right; break;
            case 2: dir = tr.right; break;
        }

        switch (action[2])
        {
            case 1: rot = -tr.up; break;
            case 2: rot = tr.up; break;
        }

        tr.Rotate(rot, Time.deltaTime * 100.0f);
        rb.AddForce(dir * 1.0f, ForceMode.VelocityChange);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("ball"))
        {
            // Ball Touch + Reward
            AddReward(0.2f);

            // Ball Kick
            Vector3 kickDir = coll.GetContact(0).point - tr.position;
            coll.gameObject.GetComponent<Rigidbody>().AddForce(kickDir.normalized * 800.0f);
        }
    }

}
