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
        // 물리력을 초기화
        rb.velocity = rb.angularVelocity = Vector3.zero;
        // 축구공의 위치를 초기화
        transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
    }

    void OnCollisionEnter(Collision coll)
    {
        // Blue 골대에 들어갔을 경우
        // Red Team +1 Reward
        // Blue Team -1 Reward
        // EndEpisode

        if (coll.collider.CompareTag("BLUE_GOAL"))
        {
            // 리워드 적용
            players[1].AddReward(+1.0f);
            players[0].AddReward(-1.0f);
            // 학습을 종료
            players[0].EndEpisode();
            players[1].EndEpisode();
            // 볼 위치 초기화
            InitBall();
            // 점수 기록
            redScoreText.text = (++redScore).ToString();
        }

        // Red 골대에 들어갔을 경우
        // Blue Team +1 Reward
        // Red Team -1 Reward
        // EndEpisode
        if (coll.collider.CompareTag("RED_GOAL"))
        {
            // 리워드 적용
            players[1].AddReward(-1.0f);
            players[0].AddReward(+1.0f);
            // 학습을 종료
            players[0].EndEpisode();
            players[1].EndEpisode();
            // 볼 위치 초기화
            InitBall();
            // 점수 기록
            blueScoreText.text = (++blueScore).ToString();
        }
    }
}
