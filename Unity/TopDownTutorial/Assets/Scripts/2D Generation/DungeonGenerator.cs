using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Tilemaps;

public class DungeonGenerator : MonoBehaviour {

    public GameObject playerStart;
    public GameObject exit;

    public Tilemap groundTilemap;
    public Tilemap wallsTilemap;

    public Tile groundTile_1;
    public Tile groundTile_2;
    public Tile groundTile_3;
    public Tile groundTile_4;

    public Tile wall_surrounded;
    public Tile wall_free;
    public Tile wall_ns_corr;
    public Tile wall_ew_corr;
    public Tile wall_n;
    public Tile wall_s;
    public Tile wall_e;
    public Tile wall_w;
    public Tile wall_sw;
    public Tile wall_nw;
    public Tile wall_ne;
    public Tile wall_se;
    public Tile wall_nwe;
    public Tile wall_ens;
    public Tile wall_swe;
    public Tile wall_wns;

    public GameObject wall_1;
    public GameObject ground_1;

    [Range(50, 200)]
    public int height;

    [Range(50, 200)]
    public int width;

    private int[,] map;
    //public Vector3Int mapSize;

    [Range(1, 50)]
    public int rooms;

    [Range(2, 10)]
    public int MaxRoomSize;

    [Range(2, 10)]
    public int MinRoomSize;

    [Range(1, 10)]
    public int MaxCorrSize;

    [Range(1, 10)]
    public int MinCorrSize;

    void Start()
    {
        GenerateDungeon();
        draw();
    }

    private Tile choose_ground()
    {
        int num = Random.Range(0, 100);

        if (num <= 50)
        {
            return groundTile_1;
        }
        else if (num > 50 && num <= 75)
        {
            return groundTile_2;
        }
        else if (num > 75 && num <= 95)
        {
            return groundTile_3;
        }
        else if (num > 95 && num <= 100)
        {
            return groundTile_4;
        }
        return groundTile_1;
    }

    private Tile choose_wall(int north, int south, int east, int west)
    {
        if (north == 1 && south == 1 && east == 1 && west == 1) // surrounded by blocks
        {
            return wall_surrounded;
        }
        if (north == 0 && south == 0 && east == 0 && west == 0) // free
        {
            return wall_free;
        }
        else if (north == 0 && south == 1 && east == 1 && west == 1) // open north
        {
            return wall_n;
        }
        else if (north == 1 && south == 0 && east == 1 && west == 1) // open south
        {
            return wall_s;
        }
        else if (north == 1 && south == 1 && east == 0 && west == 1) // open east
        {
            return wall_e;
        }
        else if (north == 1 && south == 1 && east == 1 && west == 0) // open west
        {
            return wall_w;
        }
        else if (north == 1 && south == 0 && east == 0 && west == 1) // open south east
        {
            return wall_se;
        }
        else if (north == 0 && south == 1 && east == 0 && west == 1) // open north east
        {
            return wall_ne;
        }
        else if (north == 0 && south == 1 && east == 1 && west == 0) // open north west
        {
            return wall_nw;
        }
        else if (north == 1 && south == 0 && east == 1 && west == 0) // open south west
        {
            return wall_sw;
        }
        else if (north == 0 && south == 1 && east == 0 && west == 0) // open north west east
        {
            return wall_nwe;
        }
        else if (north == 0 && south == 0 && east == 1 && west == 0) // open north west south
        {
            return wall_wns;
        }
        else if (north == 1 && south == 0 && east == 0 && west == 0) // open south east west
        {
            return wall_swe;
        }
        else if (north == 0 && south == 0 && east == 0 && west == 1) // open north south east
        {
            return wall_ens;
        }
        else if (north == 1 && south == 1 && east == 0 && west == 0) // east west corridor
        {
            return wall_ew_corr;
        }
        else if (north == 0 && south == 0 && east == 1 && west == 1) // north south corridor
        {
            return wall_ns_corr;
        }
        return wall_surrounded;
    }

    private struct values
    {
        public int topLx;
        public int topLy;
        public int botRx;
        public int botRy;

        public values (int a, int b, int c, int d)
        {
            topLx = a;
            topLy = b;
            botRx = c;
            botRy = d;
        }
    };

    private void create_room(values val)
    {
        for (int i = val.topLy; i < val.botRy; i++)
        {
            for (int j = val.topLx; j < val.botRx; j++)
            {
                map[i, j] = 0;
            }
        }
    }

    private values northCorridor(values val, int new_len, int new_hght, int entry_offset, int corr_length)
    {
        int X = Random.Range(val.topLx + 1, val.botRx);
        int startY = val.topLy;

        // if room is out of bounds
        if (X - entry_offset <= 0 || startY - corr_length - new_hght <= 0 || X - entry_offset + new_len >= width - 1 || startY - corr_length >= height - 1)
        {
            return create_corridor(val);
        }

        // draw corridor
        for (int i = 0; i < corr_length + MinRoomSize; i++)
        {
            map[startY - i - 1, X] = 0;
            map[startY - i - 1, X + 1] = 0;
        }

        return new values(X - entry_offset, startY - corr_length - new_hght, X - entry_offset + new_len, startY - corr_length);
    }

    private values southCorridor(values val, int new_len, int new_hght, int entry_offset, int corr_length)
    {
        int X = Random.Range(val.topLx + 1, val.botRx);
        int startY = val.botRy;

        // if room is out of bounds
        if (X - entry_offset <= 0 || startY + corr_length <= 0 || X - entry_offset + new_len >= width - 1 || startY + corr_length + new_hght >= height - 1)
        {
            return create_corridor(val);
        }

        // draw corridor
        for (int i = 0; i < corr_length + MinRoomSize; i++)
        {
            map[startY + i, X] = 0;
            map[startY + i, X + 1] = 0;
        }

        return new values(X - entry_offset, startY + corr_length, X - entry_offset + new_len, startY + corr_length + new_hght);
    }

    private values eastCorridor(values val, int new_len, int new_hght, int entry_offset, int corr_length)
    {
        int Y = Random.Range(val.topLy + 1, val.botRy);
        int startX = val.botRx;

        // if room is out of bounds
        if (startX + corr_length <= 0 || Y + entry_offset - new_hght <= 0 || startX + corr_length + new_len >= width - 1 || Y + entry_offset >= height - 1)
        {
            return create_corridor(val);
        }

        // draw corridor
        for (int i = 0; i < corr_length + MinRoomSize; i++)
        {
            map[Y, startX + i] = 0;
            map[Y + 1, startX + i] = 0;
        }

        return new values(startX + corr_length, Y + entry_offset - new_hght, startX + corr_length + new_len, Y + entry_offset);
    }

    private values westCorridor(values val, int new_len, int new_hght, int entry_offset, int corr_length)
    {
        int Y = Random.Range(val.topLy + 1, val.botRy);
        int startX = val.topLx;

        // if room is out of bounds
        if (startX - corr_length - new_len <= 0 || Y + entry_offset - new_hght <= 0 || startX - corr_length >= width - 1 || Y + entry_offset >= height - 1)
        {
            return create_corridor(val);
        }

        // draw corridor
        for (int i = 0; i < corr_length + MinRoomSize; i++)
        {
            map[Y, startX - i - 1] = 0;
            map[Y + 1, startX - i - 1] = 0;
        }

        return new values(startX - corr_length - new_len, Y + entry_offset - new_hght, startX - corr_length, Y + entry_offset);
    }

    private values create_corridor(values val)
    {
        int dir = Random.Range(0, 4);
        values new_val = new values();

        // generate new coords for next room
        int new_len = Random.Range(MinRoomSize, MaxRoomSize);
        int new_hght = Random.Range(MinRoomSize, MaxRoomSize);
        int entry_offset = Random.Range(0, new_len);
        int corr_length = Random.Range(MinCorrSize, MaxCorrSize);

        switch (dir)
        {
            case 0:
                { // north
                  //cout << "north" << endl;
                    new_val = northCorridor(val, new_len, new_hght, entry_offset, corr_length);
                    break;
                }
            case 1:
                { // east
                  //cout << "east" << endl;
                    new_val = eastCorridor(val, new_len, new_hght, entry_offset, corr_length);
                    break;
                }
            case 2:
                { // south
                  //cout << "south" << endl;
                    new_val = southCorridor(val, new_len, new_hght, entry_offset, corr_length);
                    break;
                }
            case 3:
                { // west
                  //cout << "west" << endl;
                    new_val = westCorridor(val, new_len, new_hght, entry_offset, corr_length);
                    break;
                }
        }
        return new_val;
    }

    private void GenerateDungeon()
    {
        map = GenerateArray(width, height, false);
        int a = width / 2 - Random.Range(MinRoomSize, MaxRoomSize);
        int b = height / 2 - Random.Range(MinRoomSize, MaxRoomSize);
        int offset = Random.Range(MinRoomSize, MaxRoomSize);
        values val = new values( a, b, a + offset, b + offset );

        // place player in the middle of the first room
        GameObject start_location = (GameObject)Instantiate(playerStart);
        int x = b + (offset / 2);
        int y = a + (offset / 2);
        start_location.transform.position = new Vector3Int(-x + width / 2 + 1, -y + height / 2 + 1, 0);

        for (int i = 0; i < rooms; i++)
        {
            create_room(val);
            if (i != rooms - 1)
                val = create_corridor(val);
        }

        GameObject exit_location = (GameObject)Instantiate(exit);
        int y_exit = (val.topLx + val.botRx) / 2;
        int x_exit = (val.botRy + val.topLy) / 2;
        exit_location.transform.position = new Vector3Int(-x_exit + width / 2 + 1, -y_exit + height / 2 + 1, 0);
    }

    public void draw()
    {
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (map[x, y] == 0)
                {
                    GameObject ground_test = (GameObject)Instantiate(ground_1);
                    int x_ground = -x + width / 2;
                    int y_ground = -y + height / 2;
                    ground_test.transform.position = new Vector3Int(x_ground, y_ground, 0);

                    //groundTilemap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), choose_ground());
                }
                else
                {
                    GameObject wall_test = (GameObject)Instantiate(wall_1);
                    int x_wall = -x + width / 2;
                    int y_wall = -y + height / 2;
                    wall_test.transform.position = new Vector3Int(x_wall, y_wall, 0);

                    /*
                    Tile tile;
                    if (x == 0 || y == 0 || x == map.GetUpperBound(0) - 1 || y == map.GetUpperBound(1) - 1)
                    {
                        tile = wall_surrounded;
                    }
                    else
                    {
                        int north = map[x, y - 1];
                        int south = map[x, y + 1];
                        int east = map[x - 1, y];
                        int west = map[x + 1, y];
                        tile = choose_wall(north, south, east, west);
                    }
                    wallsTilemap.SetTile(new Vector3Int(-x + width / 2, -y + height / 2, 0), tile);
                    */
                }
            }
        }
    }

    private static int[,] GenerateArray(int width, int height, bool empty)
    {

        int[,] map = new int[width, height];
        for (int x = 0; x < map.GetUpperBound(0); x++)
        {
            for (int y = 0; y < map.GetUpperBound(1); y++)
            {
                if (empty)
                {
                    map[x, y] = 0;
                }
                else
                {
                    map[x, y] = 1;
                }
            }
        }

        return map;
    }
}
