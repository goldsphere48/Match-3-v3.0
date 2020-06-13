namespace Match_3_v3._0.Data
{
    internal enum GameState
    {
        WaitForUserInput,
        Swapping,
        Falling,
        Matching,
        Generating,
        CombinationChecking,
        CellDestroying,
        WaitForFalling,
        DestroyersMoving
    }
}