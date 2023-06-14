using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

[System.Serializable]
public class GameData
{
    public string CurrentLevel;
    public float[] LastCheckpointPosition;
    public string LastCheckpointScene;
    public List<int> ActiveOrbs;

    //Empty constructor for new game
    public GameData()
    {
        int dispersionOrbID = (int)Orbs.DISPERSION;

        this.CurrentLevel = Scenes.Fungi_Spawn.ToString();
        this.ActiveOrbs = new() { dispersionOrbID };
        this.LastCheckpointPosition = new float[] { 0, 0, 0 };
        this.LastCheckpointScene = this.CurrentLevel;
    }

    public override string ToString()
    {
        string result = "Current Level: " + CurrentLevel + "; Last Checkpoint: " + LastCheckpointPosition + "; ActiveOrbs: ";
        foreach(int orb in ActiveOrbs) { result += orb.ToString() + ", "; }
        return result;
    }
}
