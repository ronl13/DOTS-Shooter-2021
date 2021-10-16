using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugUIScript : MonoBehaviour
{
    public PlayerShooting playerShooting;

    [Header("Text Components")]
    public TextMeshProUGUI usingDOTSText;
    public TextMeshProUGUI bulletCounterText;
    public TextMeshProUGUI spreadShotText;
    public TextMeshProUGUI fpsText;

    private void Update() {
        
        usingDOTSText.text = playerShooting.UseECS == false ? "DOTS/ECS Enabled: <color=red>FALSE" : "DOTS/ECS Enabled: <color=green>TRUE";

        bulletCounterText.text = "Current Bullets: <color=orange>" + playerShooting.currentBullets.ToString();
        
        spreadShotText.text = playerShooting.spreadShot == false ? "Spread Shot Disabled" : ("Spread Amount: <color=orange>" + playerShooting.spreadAmount);

        //DEBUG
        float currentFPS = (1f / Time.unscaledDeltaTime);
        fpsText.text = "Current FPS: " + currentFPS.ToString("F2");
    }
}
