using UnityEngine;

public static class ChunkCoordinates
{
    public static void GetChunksCoordsAndLocalTileCoordsFromWorldPos(int chunkSize, Vector2 worldPosition, out Vector2Int chunkCoords, out Vector2Int localTileCoords)
    {
        chunkCoords = GetChunkCoordsFromWorldPos(chunkSize, worldPosition);
        Vector2 localPosition = worldPosition - chunkSize * chunkCoords;
        localTileCoords = new Vector2Int(Mathf.FloorToInt(localPosition.x), Mathf.FloorToInt(localPosition.y));
    }

    public static void GetChunksCoordsAndLocalTileCoordsFromWorldTileCoords(int chunkSize, Vector2Int worldTileCoords, out Vector2Int chunkCoords, out Vector2Int localTileCoords)
    {
        chunkCoords = GetChunkCoordsFromWorldTileCoords(chunkSize, worldTileCoords);
        Vector2Int startTileCoords = chunkSize * chunkCoords;
        localTileCoords = new Vector2Int(worldTileCoords.x - startTileCoords.x, worldTileCoords.y - startTileCoords.y);
    }

    public static Vector2Int GetChunkCoordsFromWorldPos(int chunkSize, Vector2 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x / chunkSize), Mathf.FloorToInt(worldPosition.y / chunkSize));
    }

    public static Vector2Int GetChunkCoordsFromWorldTileCoords(int chunkSize, Vector2Int worldTileCoords)
    {
        return new Vector2Int(Mathf.FloorToInt((float)worldTileCoords.x / chunkSize), Mathf.FloorToInt((float)worldTileCoords.y / chunkSize)); // FloorToInt needed to properly handle negative coordinates
    }

    public static Vector2Int GetTileCoordsFromWorldPos(int chunkSize, Vector2 worldPosition)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPosition.x), Mathf.FloorToInt(worldPosition.y));
    }

    public static Vector2Int GetLocalTileCoordsFromWorldPos(int chunkSize, Vector2 worldPosition)
    {
        Vector2 localPosition = worldPosition - chunkSize * GetChunkCoordsFromWorldPos(chunkSize, worldPosition);
        return new Vector2Int(Mathf.FloorToInt(localPosition.x), Mathf.FloorToInt(localPosition.y));
    }

    public static Vector2Int GetLocalTileCoordsFromWorldTileCoords(int chunkSize, Vector2Int worldTileCoords)
    {
        Vector2Int startTileCoords = chunkSize * GetChunkCoordsFromWorldTileCoords(chunkSize, worldTileCoords);
        return new Vector2Int(worldTileCoords.x - startTileCoords.x, worldTileCoords.y - startTileCoords.y);
    }
}