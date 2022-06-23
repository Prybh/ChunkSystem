using UnityEngine;

#if UNITY_EDITOR

public class EditorChunkViewer : IChunkViewer
{
    private const float cameraBoundsExtent = 5f;
    private static Camera editorCamera => UnityEditor.SceneView.lastActiveSceneView.camera;

    private int m_chunkSize;

    public bool Initialize(int chunkSize)
    {
        m_chunkSize = chunkSize;
        return true;
    }

    public bool NeedVisibilityUpdate()
    {
        return true;
    }

    public bool ShouldChunkBeVisible(Vector2Int chunkCoords)
    {
        Bounds cameraBounds = CameraOrthographicBounds(editorCamera);
        cameraBounds.Expand(cameraBoundsExtent);

        Vector2Int position = chunkCoords * m_chunkSize;
        Vector3 worldPosition = new Vector3(position.x, position.y, 0.0f);
        Vector3 halfSize = Vector3.one * 0.5f * m_chunkSize;
        Bounds chunkBounds = new Bounds(worldPosition + halfSize, 2.0f * halfSize);

        return cameraBounds.Intersects(chunkBounds);
    }

    public void ComputeCoordsBounds(out Vector2Int min, out Vector2Int max)
    {
        Bounds cameraBounds = CameraOrthographicBounds(editorCamera);
        Vector2 camMin = new Vector2(cameraBounds.min.x, cameraBounds.min.y) - Vector2.one * cameraBoundsExtent;
        Vector2 camMax = new Vector2(cameraBounds.max.x, cameraBounds.max.y) + Vector2.one * cameraBoundsExtent;
        min = ChunkCoordinates.GetChunkCoordsFromWorldPos(m_chunkSize, camMin);
        max = ChunkCoordinates.GetChunkCoordsFromWorldPos(m_chunkSize, camMax);
    }

    private static Bounds CameraOrthographicBounds(Camera camera, float sizeZ = 2000f)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2f;
        return new Bounds(camera.transform.position, new Vector3(cameraHeight * screenAspect, cameraHeight, sizeZ));
    }
}
#endif // UNITY_EDITOR