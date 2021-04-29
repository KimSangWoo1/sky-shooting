using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mapmesh;

public class Map_V2 : MonoBehaviour
{
    public Vector2 mapSize;
    [Range(0, 1)]
    public float outLine;
    [Range(0, 20)]
    public int seed;

    List<Coord> walkCoords;
    List<Coord> obstacleCoords;

    Queue<Coord> shuffledTileCoords;
    Queue<Coord> obstacleTileCoords;
    public float radius;
    public float margine;
    public LayerMask obstacleLayer;

    private Bounds bound;
    private void Start()
    {
        bound = GetComponent<Renderer>().bounds;
        GeneratorMap();
    }

    private void Update()
    {
        Coord c = GetRandomCoord();
        Debug.DrawRay(new Vector3(c.x, 0f, c.y), Vector3.up, Color.red, 5f);

        Coord d = GetObstacleCoord();
        Debug.DrawRay(new Vector3(d.x, 0f, d.y), Vector3.up, Color.black, 5f);

    }

    public void GeneratorMap()
    {
        //맵 사이즈 정하기
        transform.localScale = new Vector3(mapSize.x, 1f, mapSize.y);

        Vector3 size = bound.extents; //bounds는 Object 실제 크기  .extends는 size의 반값
        size = size - Vector3.one * margine; //margine두기

        int sizeX = (int)size.x;
        int sizeY = (int)size.z;


        Collider[] colliders = new Collider[1];

        walkCoords = new List<Coord>(); //움직일 수 있는 곳
        obstacleCoords = new List<Coord>(); //장애물 있는 곳
        
        //저장
        for (int x = -sizeX; x <= sizeX; x=x+10)
        {
            for (int y = -sizeY; y <= sizeY; y=y+10)
            {
                //장애물 감지
                int count = Physics.OverlapSphereNonAlloc(new Vector3(x, 0f, y), radius, colliders, obstacleLayer, QueryTriggerInteraction.Collide);
                //장애물 저장
                if (count != 0)
                {
                    Coord obstacleTile = new Coord(x, y);
                    obstacleCoords.Add(obstacleTile);
                }
                // Walkable 저장
                else
                {
                    walkCoords.Add(new Coord(x, y));
                }
            }
        }

        //셔플
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(walkCoords.ToArray(), seed));
        obstacleTileCoords = new Queue<Coord>(Utility.ShuffleArray(obstacleCoords.ToArray(), seed));
        /* 
        // 셔플되있는 타일들을 다 받아온다. 순서대로 다 큐에 삽입
        shuffledTileCoords = new Queue<Coord>(Utility.ShuffleArray(allTileCoords.ToArray(), seed));

        string name = "Generated Map";

        if (transform.transform.Find(name))
        {
            DestroyImmediate(transform.transform.Find(name).gameObject);
        }

        GameObject MapHolder = new GameObject(name);
        Vector3 position;

        MapHolder.transform.parent = transform;
        //타일 만들기    
        for (int x = 0; x <= mapSize.x; x++)
        {
            for (int y = 0; y <= mapSize.y; y++)
            {
                position = CoordToPosition(x, y);
                GameObject new_tile = Instantiate(tilePrefab, position, tilePrefab.transform.rotation);
                new_tile.transform.parent = MapHolder.transform;
                new_tile.transform.localScale = Vector3.one * (1 - outLine);
            }
        }
        */

    }

    Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-mapSize.x / 2 + 0.5f + x, 0f, -mapSize.y / 2 + 0.5f + y);
    }

    //셔플된 첫번째 (x,y) 타일을 가져온다
    public Coord GetRandomCoord()
    {                                                       // ---->FIFO 선입선출
        Coord randomCoord = shuffledTileCoords.Dequeue(); //큐 출 ]=======[(맨 첫번째 들어온) 맨뒤에꺼 빼기
        shuffledTileCoords.Enqueue(randomCoord);          //큐 입 큐에 넣기
        return randomCoord;
    }

    public Coord GetObstacleCoord()
    {
        Coord randomCoord = obstacleTileCoords.Dequeue();
        obstacleTileCoords.Enqueue(randomCoord);          
        return randomCoord;
    }
}
