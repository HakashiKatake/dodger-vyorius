using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    CarController winner;
    private void OnTriggerEnter(Collider other)
    {
        LapTracker lapTracker = other.GetComponent<LapTracker>();
        if (lapTracker != null && lapTracker.passedAllCheckPoint)
        {
            if (lapTracker.currentCheckpointIndex == 0 && lapTracker.currentLap <= lapTracker.totalLaps)
            {
                lapTracker.currentLap++;
                lapTracker.LapTxt.text = $"Lap : {lapTracker.currentLap} / {lapTracker.totalLaps}";
                lapTracker.passedAllCheckPoint = false;
                if (lapTracker.currentLap > lapTracker.totalLaps)
                {
                    // Win
                    // Race Finished
                    if(winner == null)
                    {
                        winner = lapTracker.GetComponent<CarController>();
                        Debug.Log(winner.isPlayerOne ? "Player 1 Win" : "Player 2 win");
                    }
                    
                    
                    lapTracker.cam.enabled = false;
                    lapTracker.cam.transform.SetParent(null);
                }
            }
        }
    }
}
