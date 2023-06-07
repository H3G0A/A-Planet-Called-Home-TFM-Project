using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalParameters;

[System.Serializable]
public class GameData
{
    public string CurrentLevel;
    public float[] LastCheckpoint;
    public List<int> ActiveOrbs;

    //Empty constructor for new game
    public GameData()
    {
        int dispersionOrbID = (int)Orbs.DISPERSION;

        this.CurrentLevel = Scenes.HegoaSandbox.ToString();
        this.ActiveOrbs = new() { dispersionOrbID };
        this.LastCheckpoint = new float[] { 0, 0, 0 };
    }

    public override string ToString()
    {
        string result = "Current Level: " + CurrentLevel + "; Last Checkpoint: " + LastCheckpoint + "; ActiveOrbs: ";
        foreach(int orb in ActiveOrbs) { result += orb.ToString() + ", "; }
        return result;
    }
}
