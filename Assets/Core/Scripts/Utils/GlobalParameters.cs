using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalParameters
{
    // SCENES
    //// In order for a scene to be loaded it has to exist in the BUILD SETTINGS
    public enum Scenes { MainMenu, HegoaSandbox, Fungi_Spawn, Heat_Spawn, Heat_Caves, Heat_End};

    // TAGS
    public const string ORB_TAG = "Orb";
    public const string PLAYER_TAG = "Player";
    public const string ICE_TAG = "Ice";
    public const string DISPLACE_BOX_TAG = "DisplaceBox";
    public const string ELEVATOR_TAG = "Elevator";
    public const string WATER_TAG = "Water";
    public const string BREAKABLE_WALL_TAG = "BreakableWall";
    public const string LUMINOUS_CRYSTAL = "LuminousCrystal";
    public const string HEAT_ZONE_TAG = "HeatZone";
    public const string WORLD_BOUNDS_TAG = "WorldBounds";

    // LAYERS
    public const int GROUND_LAYER = 3;
    public const int PLAYER_LAYER = 6;
    public const int ICE_LAYER = 8;

    // SORTING LAYERS


    // INPUT ACTIONS
    public const string MOVE_ACTION = "Move";
    public const string LOOK_ACTION = "Look";
    public const string SPRINT_ACTION = "Sprint";
    public const string JUMP_ACTION = "Jump";
    public const string AIM_ACTION = "Aim";
    public const string SHOOT_ACTION = "Shoot";
    public const string CHANGEORB_ACTION = "ChangeOrb";
    public const string CHANGEORBWEIGTH_ACTION = "ChangeOrbWeigth";
    public const string CHANGEORBDIRECTLY_ACTION = "ChangeOrbDirectly";
    public const string CHANGE_WEIGHT_ACTION = "Change Weight";

    // ACTION MAPS
    public const string PLAYER_GROUND_MAP = "Ground";
    public const string PLAYER_WATER_SURFACE_MAP = "Water Surface";
    public const string PLAYER_UNDERWATER_MAP = "Underwater";

    // ORBS ID
    public enum Orbs { DISPERSION = 0, ICE = 1, WEIGHT = 2 };
}
