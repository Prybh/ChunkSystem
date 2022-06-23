using UnityEngine;

public interface IChunkViewer
{
    bool Initialize(int chunkSize);
    bool NeedVisibilityUpdate();
    bool ShouldChunkBeVisible(Vector2Int chunkCoords);
    void ComputeCoordsBounds(out Vector2Int min, out Vector2Int max);
}
