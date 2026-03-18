using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SpaceDefence;

public class WaveFactory
{
    private const float TOP_SPEED = 300;
    private const float START_SPEED = 150;

    public int AlienCount { get; private set; } = 1;

    public int WaveCount { get; private set; } = 0;

    public List<GameObject> NextWave()
    {
        Debug.Assert(AlienCount <= 0);
        AlienCount = Math.Clamp(WaveCount % 5 + 1, 1, 12);
        float speed = Math.Clamp(START_SPEED + 30 * WaveCount, START_SPEED, TOP_SPEED);
        List<GameObject> aliens = Enumerable.Range(0, AlienCount).Select(x => new Alien(speed)).ToList<GameObject>();
        WaveCount++;
        return aliens;
    }

    public void AlienDied()
    {
        AlienCount--;
    }

    public void ResetWaves()
    {
        WaveCount = 0;
        AlienCount = 0;
    }
}
