using System;
using System.Collections.Generic;

public static class EventSystem
{
    public static Action ActionGridCreated;
    public static Action ActionFillInEmptyCells;
    public static Action ActionSpawnCompleted;
    public static Action<List<Grid.GridSystem.GridCell>> ActionMatchesFound;
    public static Action ActionRefill;
}