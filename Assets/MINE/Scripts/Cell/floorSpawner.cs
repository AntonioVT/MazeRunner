using UnityEngine;
using System.Collections;

public class floorSpawner : MonoBehaviour {
	/*
Four rules were applied to each cell in every step of the simulation:

If a living cell has less than two living neighbours, it dies.
If a living cell has two or three living neighbours, it stays alive.
If a living cell has more than three living neighbours, it dies.
If a dead cell has exactly three living neighbours, it becomes alive.
*/
	public GameObject border, filled, treasure;
	
	int chanseAlive = 45; // Porcentaje del 1 al 100.
	public int caveSizeX;
	public int caveSizeY;
	int iterations = 6;
	
	bool[,] cave;// = new bool[caveSizeX, caveSizeY];
	bool[,] newCave;// = new bool[caveSizeX, caveSizeY];
	
	int deathLimit = 3;
	int birthLimit = 4;
	
	// Treasure
	int[] xT;
	int[] yT;
	
	//Crea la cueva original, sobre la cual vamos a trabajar.
	void createCave()
	{
		for(int j=0; j<caveSizeY; j++)
		{
			for(int i=0; i<caveSizeX; i++)
			{
				if ( Random.Range(1, 100) <= chanseAlive)
					cave[i, j] = true;
				else cave[i, j] = false;
			}
		}
	}
	
	//Returns the number of cells in a ring around (x,y) that are alive.
	int countAliveNeighbours(int x, int y)
	{
		int cant = 0;
		for(int i=-1; i<2; i++)
		{
			for(int j=-1; j<2; j++)
			{
				int neighbour_x = x+i;
				int neighbour_y = y+j;
				//If we're looking at the middle point
				if(i == 0 && j == 0)
				{
					//Do nothing, we don't want to add ourselves in!
				}
				//In case the index we're looking at it off the edge of the map
				else if(neighbour_x < 0 || neighbour_y < 0 || neighbour_x >= caveSizeX|| neighbour_y >= caveSizeY)
				{
					cant += 1;
				}
				//Otherwise, a normal check of the neighbour
				else if(cave[neighbour_x, neighbour_y])
				{
					cant += 1;
				}
			}
		}
		return cant;
	}
	
	void doSimulationStep()
	{
		//Loop over each row and column of the map
		for(int x=0; x<caveSizeX; x++)
		{
			for(int y=0; y<caveSizeY; y++)
			{
				int nbs = countAliveNeighbours(x, y);
				//The new value is based on our simulation rules
				//First, if a cell is alive but has too few neighbours, kill it.
				if(cave[x, y])
				{
					if(nbs < deathLimit)
					{
						newCave[x, y] = false;
					}
					else
					{
						newCave[x, y] = true;
					}
				} //Otherwise, if the cell is dead now, check if it has the right number of neighbours to be 'born'
				else
				{
					if(nbs > birthLimit)
					{
						newCave[x, y] = true;
					}
					else
					{
						newCave[x, y] = false;
					}
				}
			}
		}
		//return newCave;
	}
	
	//Copia de una matriz a otra.
	void copyCave()
	{
		for(int j=0; j<caveSizeY; j++)
		{
			for(int i=0; i<caveSizeX; i++)
			{
				cave[i, j] = newCave[i, j];
			}
		}
	}
	
	void PlaceTreasure()
	{
		int treasureHiddenLimit = 5;
		for(int x=0; x<caveSizeX; x++)
		{
			for(int y=0; y<caveSizeY; y++)
			{
				if(!cave[x,y])
				{
					int nbs = countAliveNeighbours(x,y);
					if (nbs >= treasureHiddenLimit)
					{
						Instantiate(treasure, new Vector3(x,y,0),Quaternion.identity);
					}
				}
			}
		}
	}
	
	//Muestra el contorno de la cueva.
	void showCaveLite()
	{
		//Marco Superior
		for (int i=-1; i<=caveSizeX; i++)
			Instantiate(border, new Vector3(i,-1,0),Quaternion.identity);//cout<<"#"; //Solido
		//Desplegado de la Cueva
		for(int j=0; j<caveSizeY; j++)
		{
			//Marco Izquierda
			Instantiate(border, new Vector3(-1,j,0),Quaternion.identity);//cout<<"#"; //Solido
			//Contenido
			for(int i=0; i<caveSizeX; i++)
			{
				if (cave[i, j])
					if (countAliveNeighbours(i,j) != 8) // Algunos casos funcionan para < 7
						Instantiate(border, new Vector3(i,j,0),Quaternion.identity);//cout<<"#"; //Solido
				else Instantiate(filled, new Vector3(i,j,0),Quaternion.identity);//cout<<"-"; //Solido, pero no es contorno.
			}
			//Marco Derecha
			Instantiate(border, new Vector3(caveSizeX,j,0),Quaternion.identity);//cout<<"#"; //Solido
		}
		//Marco Inferior
		for (int i=-1; i<=caveSizeX; i++)
			Instantiate(border, new Vector3(i,caveSizeY,0),Quaternion.identity);//cout<<"#"; //Solido
	}
	
	//Realiza de manera automatica el numero de iteaciones del automata celular.
	void autoIterations(int iter)
	{
		for (int i=1; i<=iter; i++)
		{
			doSimulationStep();
			copyCave();
		}
	}
	
	// Use this for initialization
	void Start () {
		// Plantamos una semilla random.
		//using System;
		//Random.seed = (int)System.DateTime.Now.Ticks;
		cave = new bool[caveSizeX, caveSizeY];
		newCave = new bool[caveSizeX, caveSizeY];

		// Creamos una cueva random.
		createCave();
		
		//Realiza de manera automatica el numero de iteaciones del automata celular.
		autoIterations(iterations);
		
		//Despliega la cueva, donde # es solido (pared).
		showCaveLite();
		
		//Buscar lugares para nuestros spawns / hoyo negro.
		PlaceTreasure();
	}
}