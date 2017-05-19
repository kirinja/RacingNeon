using System;

[Serializable]
public struct HighScore
{
    public string Name;
    public float Time;

    public HighScore(string name, float time)
    {
        Name = name;
        Time = time;
    }
}
