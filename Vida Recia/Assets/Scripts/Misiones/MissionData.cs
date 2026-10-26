using System;
using UnityEngine;

[Serializable]
public class MissionData
{
    public int misionId;
    public string nombre;
    public string lugar;
    public float[] coordenadas;
}

[Serializable]
public class MissionList
{
    public MissionData[] misiones;
}