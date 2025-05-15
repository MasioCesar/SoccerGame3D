using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDetector : MonoBehaviour
{
    [SerializeField] private Player scriptPlayer;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Ball"))
        {
            if (name.Equals("GoalDetector1"))
            {
                scriptPlayer.IncreaseMyScore();
            }
            else
            {
                scriptPlayer.IncreaseOtherScore();
            }
        }
    }
}