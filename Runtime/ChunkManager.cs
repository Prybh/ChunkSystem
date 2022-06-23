using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChunkManager 
{
    [SerializeField] private int m_chunkSize = 64;

    public int chunkSize => m_chunkSize;

    private List<Vector2Int> m_visibleChunks = new List<Vector2Int>();
    private IChunkViewer m_viewer;
    private Action<Vector2Int, bool> m_onVisibilityChanged;

    public void Initialize(IChunkViewer viewer, Action<Vector2Int, bool> onVisibilityChanged)
    {
        m_visibleChunks.Clear();

        m_viewer = viewer;
        m_viewer.Initialize(m_chunkSize);

        m_onVisibilityChanged = onVisibilityChanged;
    }

    public void Clear()
    {
        m_visibleChunks.Clear();
    }

    public bool Update()
    {
        bool needChunkVisibilityUpdate = m_viewer.NeedVisibilityUpdate();
        if (needChunkVisibilityUpdate)
        {
            return UpdateVisibleChunks();
        }
        return false;
    }

    public bool HasChunk(Vector2Int chunkCoords)
    {
        return m_visibleChunks.Contains(chunkCoords);
    }

    private bool UpdateVisibleChunks()
    {
        bool updated = false;

        // Update currently visible chunks
        HashSet<Vector2Int> alreadyUpdatedChunkCoords = new HashSet<Vector2Int>();
        for (int i = m_visibleChunks.Count - 1; i >= 0; i--)
        {
            Vector2Int chunk = m_visibleChunks[i];

            UnityEngine.Assertions.Assert.IsTrue(!alreadyUpdatedChunkCoords.Contains(chunk));
            alreadyUpdatedChunkCoords.Add(chunk);

            if (!m_viewer.ShouldChunkBeVisible(chunk))
            {
                if (m_onVisibilityChanged != null)
                {
                    m_onVisibilityChanged(chunk, false);
                }
                m_visibleChunks.RemoveAt(i);
                updated = true;
            }
        }

        // Find new chunks or update non visible chunks
        m_viewer.ComputeCoordsBounds(out Vector2Int min, out Vector2Int max);
        Vector2Int chunkCoords = new Vector2Int();
        for (chunkCoords.y = min.y; chunkCoords.y <= max.y; ++chunkCoords.y)
        {
            for (chunkCoords.x = min.x; chunkCoords.x <= max.x; ++chunkCoords.x)
            {
                if (!alreadyUpdatedChunkCoords.Contains(chunkCoords))
                {
                    alreadyUpdatedChunkCoords.Add(chunkCoords);

                    UnityEngine.Assertions.Assert.IsTrue(m_viewer.ShouldChunkBeVisible(chunkCoords));

                    if (!m_visibleChunks.Contains(chunkCoords))
                    {
                        if (m_onVisibilityChanged != null)
                        {
                            m_onVisibilityChanged(chunkCoords, true);
                        }
                        m_visibleChunks.Add(chunkCoords);
                        updated = true;
                    }
                }
            }
        }

        return updated;
    }
}
