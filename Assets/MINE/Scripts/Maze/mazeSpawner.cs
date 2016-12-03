using UnityEngine;
using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public struct IntVector2 {
         public int x, y;
 
         public IntVector2(int x, int y) {
             this.x = x;
             this.y = y;
         }
 
         public static IntVector2 operator +(IntVector2 a, IntVector2 b) {
             return new IntVector2(a.x + b.x, a.y + b.y);
         }
 
         public static IntVector2 operator +(IntVector2 a, Vector2 b) {
             return new IntVector2(a.x + (int)b.x, a.y + (int)b.y);
         }
 
         public static IntVector2 operator -(IntVector2 a, IntVector2 b) {
             return new IntVector2(a.x - b.x, a.y - b.y);
         }
 
         public static IntVector2 operator -(IntVector2 a, Vector2 b) {
             return new IntVector2(a.x - (int)b.x, a.y - (int)b.y);
         }
 
         public static IntVector2 operator *(IntVector2 a, int b) {
             return new IntVector2(a.x * b, a.y * b);
         }
 }

namespace MazeGeneration
{	
	public static class Extensions
	{
		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, System.Random rng)
		{
			var e = source.ToArray();
			for (var i = e.Length - 1; i >= 0; i--)
			{
				var swapIndex = rng.Next(i + 1);
				yield return e[swapIndex];
				e[swapIndex] = e[i];
			}
		}
 
		public static CellState OppositeWall(this CellState orig)
		{
			return (CellState)(((int) orig >> 2) | ((int) orig << 2)) & CellState.Initial;
		}
 
		public static bool HasFlag(this CellState cs,CellState flag)
		{
			return ((int)cs & (int)flag) != 0;
		}
	}
    
 
    [Flags]
    public enum CellState
    {
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8,
        Visited = 128,
        Initial = Top | Right | Bottom | Left,
    }
 
    public struct RemoveWallAction
    {
        public IntVector2 Neighbour;
        public CellState Wall;
    }
 
    public class Maze : ScriptableObject
    {
        private CellState[,] _cells;
        private int _width;
        private int _height;
		private Vector3 _pos;
		private System.Random _rng;
		private GameObject _wall;
		private List<string> mazeString = new List<string>();
 		
		public void Init(int width, int height, Vector3 pos, int seed, GameObject wall)
        {
            _width = width;
            _height = height;
			_pos = pos;
			_wall = wall;
            _cells = new CellState[width, height];
            for(var x=0; x<width; x++)
                for(var y=0; y<height; y++)
                    _cells[x, y] = CellState.Initial;
            _rng = new System.Random(seed);
            VisitCell(_rng.Next(width), _rng.Next(height));
        }
 
        public CellState this[int x, int y]
        {
            get { return _cells[x,y]; }
            set { _cells[x,y] = value; }
        }
 
        public IEnumerable<RemoveWallAction> GetNeighbours(IntVector2 p)
        {
            if (p.x > 0) yield return new RemoveWallAction {Neighbour = new IntVector2(p.x - 1, p.y), Wall = CellState.Left};
            if (p.y > 0) yield return new RemoveWallAction {Neighbour = new IntVector2(p.x, p.y - 1), Wall = CellState.Top};
            if (p.x < _width-1) yield return new RemoveWallAction {Neighbour = new IntVector2(p.x + 1, p.y), Wall = CellState.Right};
            if (p.y < _height-1) yield return new RemoveWallAction {Neighbour = new IntVector2(p.x, p.y + 1), Wall = CellState.Bottom};
        }
 
        public void VisitCell(int x, int y)
        {
			this[x,y] |= CellState.Visited;
            foreach (var p in GetNeighbours(new IntVector2(x, y)).Shuffle(_rng).Where(z => !(this[z.Neighbour.x, z.Neighbour.y].HasFlag(CellState.Visited))))
            {
                this[x, y] -= p.Wall;
                this[p.Neighbour.x, p.Neighbour.y] -= p.Wall.OppositeWall();
                VisitCell(p.Neighbour.x, p.Neighbour.y);
            }
        }

        public void Display()
        {
			var firstLine = string.Empty;
            for (var y = 0; y < _height; y++)
            {
                var sbTop = new StringBuilder();
                var sbMid = new StringBuilder();
                for (var x = 0; x < _width; x++)
                {
                    sbTop.Append(this[x, y].HasFlag(CellState.Top) ? "+--" : "+  ");
                    sbMid.Append(this[x, y].HasFlag(CellState.Left) ? "|  " : "   ");
                }
                if (firstLine == string.Empty)
                    firstLine = sbTop.ToString();
				
				mazeString.Add(sbTop + "+");
				mazeString.Add(sbMid + "|");
				mazeString.Add(sbMid + "|");
            }
			mazeString.Add(firstLine);
			
			// Instantiate the maze
			Draw();
        }
		
		public void Draw()
		{
			// Vars
			int i = 0, j = 0;
			float iA = _pos.x, jA = _pos.z;
			
			Vector3 scaleWall = _wall.transform.localScale;
			float widthWall = scaleWall.x;
			float heightWall = scaleWall.y;
			
			for (i = 0, iA = _pos.x; i < mazeString.Count; i++, iA+=widthWall)
			{
				for (j = 0, jA = _pos.z; j < mazeString[i].Length; j++, jA+=widthWall)
				{
					if (mazeString[i][j] != ' ')
					{ 
						UnityEngine.Object.Instantiate(_wall, new Vector3(iA, -heightWall/2.0f, jA), Quaternion.identity);
						// y original = heightWall/2.0f
					}
				}
			}
			UnityEngine.Object.Instantiate(_wall, new Vector3(iA-widthWall, -heightWall/2.0f, jA), Quaternion.identity);
			// y original heightWall/2.0f
		}		
	}
}

public class mazeSpawner : MonoBehaviour
{
	public GameObject mazeWall;
	public GameObject mazeFloor;
	public GameObject mazeCenter;
	public GameObject player;
	public GameObject monsterSpawner;
	public GameObject mazeGates;
	public int mazeSizeX;
	public int mazeSizeY;
	public List<Vector2> connectMazePos = new List<Vector2>();
	
	void Start ()
	{				
		// Crear la seed
		int iSeed = UnityEngine.Random.Range(0, 99999);
		UnityEngine.Random.InitState(iSeed);
		
		// Me aseguro que la Y sea la mayor, y la X la menor
		if (mazeSizeX > mazeSizeY)
		{
			int minSize = mazeSizeY;
			mazeSizeY = mazeSizeX;
			mazeSizeX = minSize;
		}
		
		// Usaremos 4 maze para crear la ilusion de que se hacen alrededor de la Safe Zone
		List<Vector3> mazePos = new List<Vector3>();
		mazePos.Add(new Vector3(0,0,0));
		mazePos.Add(new Vector3(mazeSizeY*12.0f,0,0));
		mazePos.Add(new Vector3(mazeSizeX*12.0f,0,mazeSizeY*12.0f));
		mazePos.Add(new Vector3(0,0,mazeSizeX*12.0f));
		
		// Instanciar los 4 maze
		ConnectMaze();
		for (int i = 0; i < 4; i++)
		{
			int sizeX = (i%2 == 0) ? mazeSizeX : mazeSizeY;
			int sizeY = (i%2 == 0) ? mazeSizeY : mazeSizeX;
			
			// Paredes
			MazeGeneration.Maze maze = MazeGeneration.Maze.CreateInstance<MazeGeneration.Maze>();
			maze.Init(sizeX, sizeY, mazePos[i], iSeed, mazeWall);
			maze.Display();
			
			// Piso
			mazeFloor.transform.localScale = new Vector3(sizeY*12.0f, 1.0f, sizeX*12.0f);
			mazeFloor.GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(mazeFloor.transform.localScale.x/8.0f, mazeFloor.transform.localScale.z/16.0f);
			UnityEngine.Object.Instantiate(mazeFloor, new Vector3(mazePos[i].x + sizeY*6.0f, 0.0f, mazePos[i].z + sizeX*6.0f), Quaternion.identity);
		}
		
		// Obtener el centro del maze
		int sizeXCenter = (mazeSizeX >= mazeSizeY) ? mazeSizeX : mazeSizeY;
		int sizeYCenter = (mazeSizeX < mazeSizeY) ? mazeSizeX : mazeSizeY;
		Vector3 mazeCenterPos = new Vector3(sizeXCenter*12.0f - Math.Abs(sizeXCenter-sizeYCenter)*6.0f, 0, sizeYCenter*12.0f + Math.Abs(sizeXCenter - sizeYCenter)*6.0f);
		
		// Instanciar la Safe Zone
		mazeCenter.transform.localScale = new Vector3(Math.Abs(sizeXCenter-sizeYCenter)*12.0f, 1.0f, Math.Abs(sizeXCenter-sizeYCenter)*12.0f);
		mazeCenter.GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2(mazeCenter.transform.localScale.x/3.0f, mazeCenter.transform.localScale.z/3.0f);
		UnityEngine.Object.Instantiate(mazeCenter, mazeCenterPos, Quaternion.identity);

		// Instanciar al player en Safe Zone
		GameManager.Instance.goPlayer =  UnityEngine.Object.Instantiate(player, mazeCenterPos, Quaternion.identity) as GameObject;
		GameManager.Instance.SetReferences();
		
		// Instanciar Gates
		Vector3 gateScale = mazeGates.transform.localScale;
		UnityEngine.Object.Instantiate(mazeGates, new Vector3(162.19f, -24.0f, 203.54f), Quaternion.Euler(0, 180, 0));
		UnityEngine.Object.Instantiate(mazeGates, new Vector3(162.19f, -24.0f, 120.06f), Quaternion.Euler(0,   0, 0));
		UnityEngine.Object.Instantiate(mazeGates, new Vector3(120.16f, -24.0f, 162.33f), Quaternion.Euler(0,  90, 0));
		UnityEngine.Object.Instantiate(mazeGates, new Vector3(204.00f, -24.0f, 162.33f), Quaternion.Euler(0, 270, 0));
		
		/// Cambiar a evento
		Wait (8.0f, () => {
			InstanciateStuff();
		});

	}
	
	void Wait(float seconds, Action action){
		StartCoroutine(_wait(seconds, action));
	}
	IEnumerator _wait(float time, Action callback){
		yield return new WaitForSeconds(time);
		callback();
	}
	
	void InstanciateStuff(){

		
		// Instanciar Monster Spwner
		for (int i = 0; i < connectMazePos.Count; i+=2){
			UnityEngine.Object.Instantiate(monsterSpawner, new Vector3(connectMazePos[i].x, 3.0f,connectMazePos[i].y), Quaternion.identity);
		}
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3( 72, 3, 270), Quaternion.identity);
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3( 80, 3,  42), Quaternion.identity);
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3(275, 3,  42), Quaternion.identity);
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3(275, 3, 270), Quaternion.identity);
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3(270, 3, 137), Quaternion.identity);
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3( 80, 3, 187), Quaternion.identity);
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3(165, 3, 245), Quaternion.identity);
		UnityEngine.Object.Instantiate(monsterSpawner, new Vector3(165, 3,  80), Quaternion.identity);
	}
	
	void ConnectMaze()
	{
		// Conectar el maze
		int sizeX = (mazeSizeX >= mazeSizeY) ? mazeSizeX : mazeSizeY;
		int sizeY = (mazeSizeX < mazeSizeY) ? mazeSizeX : mazeSizeY;
		connectMazePos.Add(new Vector2(4.0f, sizeY*12.0f));
		connectMazePos.Add(new Vector2(8.0f, sizeY*12.0f));
		connectMazePos.Add(new Vector2(sizeX*12.0f, 4.0f));
		connectMazePos.Add(new Vector2(sizeX*12.0f, 8.0f));
		connectMazePos.Add(new Vector2(sizeX*12.0f + sizeY*12.0f - 4.0f, sizeX*12.0f));
		connectMazePos.Add(new Vector2(sizeX*12.0f + sizeY*12.0f - 8.0f, sizeX*12.0f));
		connectMazePos.Add(new Vector2(sizeY*12.0f, sizeX*12.0f + sizeY*12.0f - 4.0f));
		connectMazePos.Add(new Vector2(sizeY*12.0f, sizeX*12.0f + sizeY*12.0f - 8.0f));
	}
}