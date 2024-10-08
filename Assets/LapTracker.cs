using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapTracker : MonoBehaviour
{
    public int playerID;
    public int currentLap = 1;
    public int totalLaps = 3;
    public int currentCheckpointIndex = 0;
    public Transform[] checkpoints;
    public float finishLineThreshold = 5f;
    public TMP_Text LapTxt;

    private int lastCheckpointIndex = -1;
    public bool passedAllCheckPoint = false;
    public CameraFollow cam;

    private void Start()
    {
        LapTxt.text = $"Lap : {currentLap} / {totalLaps}";
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckPoint checkpoint = other.GetComponent<CheckPoint>();
        if (checkpoint != null)
        {            
            if (checkpoint.checkpointIndex == currentCheckpointIndex)
            {
                lastCheckpointIndex = currentCheckpointIndex;
                currentCheckpointIndex++;

                if (currentCheckpointIndex >= checkpoints.Length)
                {
                    currentCheckpointIndex = 0;
                    passedAllCheckPoint = true;
                    if (currentLap <= totalLaps)
                    {
                        // completed lap
                    }                    
                }
            }

            else if (checkpoint.checkpointIndex < lastCheckpointIndex)
            {
                // car is going in the oppsite Direction
                // lap progress wont count
            }
        }
    }
}
