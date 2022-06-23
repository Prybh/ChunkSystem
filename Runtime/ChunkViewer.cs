using UnityEngine;

[System.Serializable]
public class ChunkViewer : IChunkViewer
{
    [SerializeField] private float m_viewerMoveThresholdForChunkUpdate = 16.0f;
    [SerializeField] private float m_maxViewDistance = 64.0f;

    private int m_chunkSize;
    private Transform m_viewer;
    private Vector2 m_viewerPositionOld;

    private Vector2 viewerPosition => new Vector2(m_viewer.position.x, m_viewer.position.y);
    private float sqrViewerMoveThresholdForChunkUpdate => m_viewerMoveThresholdForChunkUpdate * m_viewerMoveThresholdForChunkUpdate;
    private float sqrMaxViewDistance => m_maxViewDistance * m_maxViewDistance;

    public void SetViewer(Transform viewerTransform)
    {
        m_viewer = viewerTransform;
    }

    public bool Initialize(int chunkSize)
    {
        m_chunkSize = chunkSize;
        m_viewerPositionOld = viewerPosition;
        return true;
    }

    public bool NeedVisibilityUpdate()
    {
        if ((m_viewerPositionOld - viewerPosition).sqrMagnitude > sqrViewerMoveThresholdForChunkUpdate)
        {
            m_viewerPositionOld = viewerPosition;
            return true;
        }
        return false;
    }

    public bool ShouldChunkBeVisible(Vector2Int chunkCoords)
    {
        Vector2Int position = chunkCoords * m_chunkSize;
        Vector3 worldPosition = new Vector3(position.x, position.y, 0.0f);
        Vector3 halfSize = Vector3.one * 0.5f * m_chunkSize;
        Bounds chunkBounds = new Bounds(worldPosition + halfSize, 2.0f * halfSize);

        return chunkBounds.SqrDistance(viewerPosition) <= sqrMaxViewDistance;
    }

    public void ComputeCoordsBounds(out Vector2Int min, out Vector2Int max)
    {
        Vector2Int currentChunkCoords = ChunkCoordinates.GetChunkCoordsFromWorldPos(m_chunkSize, viewerPosition);
        int chunksVisibleInViewDst = Mathf.CeilToInt(m_maxViewDistance / m_chunkSize);
        min = new Vector2Int(currentChunkCoords.x - chunksVisibleInViewDst, currentChunkCoords.y - chunksVisibleInViewDst);
        max = new Vector2Int(currentChunkCoords.x + chunksVisibleInViewDst, currentChunkCoords.y + chunksVisibleInViewDst);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_viewerPositionOld, m_viewerMoveThresholdForChunkUpdate);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(viewerPosition, m_maxViewDistance);
    }
}