using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static SaveDataManager;
using static UnityEngine.Rendering.DebugUI.Table;
using Random = UnityEngine.Random;

namespace IsaacBongos
{
    public class Matrices : MonoBehaviour, ISaveableMatrix
    {
        [Header("Prefabs")]
        [SerializeField]
        private GameObject salaPocha;
        [SerializeField]
        private GameObject m_ParedHorizontal;
        [SerializeField]
        private GameObject m_ParedVertical;
        [SerializeField]
        private GameObject m_PuertaHorizontal;
        [SerializeField]
        private GameObject m_PuertaVertical;
        [SerializeField]
        private GameObject m_Sala;
        [SerializeField]
        private GameObject m_SalaJefe;
        [SerializeField]
        private GameObject m_SalaTienda;
        [SerializeField]
        private GameObject m_SalaEnemigos;
        [SerializeField]
        private GameObject m_SalaLoot;

        [SerializeField]
        private Transform paredesParent; 

        [Header("Tilemaps")]
        [SerializeField]
        private Tilemap m_tilemapSueloSave;
        [SerializeField]
        private Tilemap m_tilemapSueloEnemigo;
        [SerializeField]
        private Tilemap m_tilemapSueloLoot;
        [SerializeField]
        private Tilemap m_tilemapSueloTienda;
        [SerializeField]
        private Tilemap m_tilemapSueloJefe;
        [SerializeField]
        private Tilemap m_tilemapParedArriba;
        [SerializeField]
        private Tilemap m_tilemapParedDerecha;
        [SerializeField]
        private Tilemap m_tilemapParedAbajo;
        [SerializeField]
        private Tilemap m_tilemapParedIzquierda;
        [SerializeField]
        private Tilemap m_tilemapPuertaArriba;
        [SerializeField]
        private Tilemap m_tilemapPuertaDerecha;
        [SerializeField]
        private Tilemap m_tilemapPuertaAbajo;
        [SerializeField]
        private Tilemap m_tilemapPuertaIzquierda;

        [Header("Tilemaps")]
        [SerializeField]
        private Grid m_Grid;

        [Header("Stats salas")]
        [SerializeField]
        private StatsHabitaciones m_StatsHabitaciones;

        [Header("Pool")]
        [SerializeField]
        private Pool m_Pool;

        [Header("Luces")]
        [SerializeField]
        private GameObject m_Luces;
        
        //Header tipo de sala
        public enum TipoSala
        {
            BOSS, INICIAL, LOOT, ENEMIGOS, TIENDA
        }

        [Header("Variables de la matriz")]
        private int[,] matrix = new int[41, 41];
        private int maximoSalas = 20;
        private int m_PosicionOriginal = 20;
        private float numSalaTienda;
        private float numSalaEnemigos;
        private float numSalaLoot;
        private int numeroDeSala = 0;

        private bool NuevaPartida;

        List<SalaNumero> listaTiposDeSalaAndNumero = new List<SalaNumero>();
        public struct SalaNumero
        {
            public TipoSala tipoSalaValue;
            public int posicionValue;

            public SalaNumero(TipoSala tipoSala, int posicion)
            {
                tipoSalaValue = tipoSala;
                posicionValue = posicion;
            }
        }

        //Evento para la gui
        public static Action<int[,]> matrizAcabada;

        // Start is called before the first frame update
        void Start()
        {
            

        }

        public void Init(bool NuevaPartida)
        {
            this.NuevaPartida = NuevaPartida;
            if (NuevaPartida)
            {
                numSalaTienda = 11.1f * (maximoSalas - 2) / 100;
                numSalaTienda = Mathf.RoundToInt(numSalaTienda);
                numSalaEnemigos = 66.6f * (maximoSalas - 2) / 100;
                numSalaEnemigos = Mathf.RoundToInt(numSalaEnemigos);
                numSalaLoot = 22.2f * (maximoSalas - 2) / 100;
                numSalaLoot = Mathf.RoundToInt(numSalaLoot);
                TipoDeListaOrden();
                RellenarMatriz();
                GeneracionMapa(20, 20);
                matrizAcabada?.Invoke(matrix);
                GenerarSalas();
            }
        }

        //Funcion para hacer una lista con el orden de las salas que habran en el mapa
        private void TipoDeListaOrden()
        {
            List<int> listaNumerosFinales = new List<int>();

            while (numSalaEnemigos > 0)
            {
                int rnd = Random.Range(1, maximoSalas - 1);
                int num = listaNumerosFinales.FirstOrDefault<int>(n => n == rnd);
                if (num == 0)
                {
                    listaNumerosFinales.Add(rnd);
                    listaTiposDeSalaAndNumero.Add(new SalaNumero(TipoSala.ENEMIGOS, rnd));
                    numSalaEnemigos--;
                }
            }

            while (numSalaLoot > 0)
            {
                int rnd = Random.Range(1, maximoSalas - 1);
                int num = listaNumerosFinales.FirstOrDefault<int>(n => n == rnd);
                if (num == 0)
                {
                    listaTiposDeSalaAndNumero.Add(new SalaNumero(TipoSala.LOOT, rnd));
                    listaNumerosFinales.Add(rnd);
                    numSalaLoot--;
                }
            }

            while (numSalaTienda > 0)
            {
                int rnd = Random.Range(1, maximoSalas - 1);
                int num = listaNumerosFinales.FirstOrDefault<int>(n => n == rnd);
                if (num == 0)
                {
                    listaTiposDeSalaAndNumero.Add(new SalaNumero(TipoSala.TIENDA, rnd));
                    listaNumerosFinales.Add(rnd);
                    numSalaTienda--;
                }
            }
            listaTiposDeSalaAndNumero = listaTiposDeSalaAndNumero.OrderBy(x => x.posicionValue).ToList();
        }

        //Rellenar la matriz de ceros
        private void RellenarMatriz()
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    matrix[row, col] = 0;
                }
            }
        }
        //Se le pasa una posicion de la matriz y este le asigna una sala, despues determina si crear salas alrededor y se vuelve a llamar si se cumplen los casos
        private void GeneracionMapa(int row, int col)
        {
            if (numeroDeSala == 0)
                matrix[row, col] = 1;

            else if (numeroDeSala == 19)
                matrix[row, col] = 5;

            else
            {
                matrix[row, col] = getTipoSala(numeroDeSala);
            }

            numeroDeSala++;
            maximoSalas--;
            List<int> SalasAlrededor;
            GetSalasAlrededor(out SalasAlrededor);
            CambiarMatriz(SalasAlrededor, row, col);
        }
        //Funcion que devuelve un numero en funcion de la sala que es, este numero se escoge por la lista de los tipos de sala
        private int getTipoSala(int numeroDeSala)
        {
            switch (listaTiposDeSalaAndNumero[numeroDeSala - 1].tipoSalaValue)
            {
                case TipoSala.ENEMIGOS:
                    return 2;
                case TipoSala.TIENDA:
                    return 3;
                case TipoSala.LOOT:
                    return 4;
                default:
                    return 0;
            }
        }
        //Devuelve una lista con las salas alrededor
        private void GetSalasAlrededor(out List<int> puertas)
        {
            puertas = new List<int>();
            int numeroPuertas = Random.Range(1, 5);

            do
            {
                int puerta = Random.Range(1, 5);
                int encontrado = puertas.FirstOrDefault<int>(n => n == puerta);

                if (encontrado == 0)
                {
                    puertas.Add(puerta);
                    numeroPuertas--;
                }
            } while (numeroPuertas > 0);

            puertas.Shuffle();
        }
        //Cambia la matriz con la lista de salas de alrededor y con la posicion actual en la que estas
        private void CambiarMatriz(List<int> salasAlrededor, int row, int col)
        {
            foreach (int sala in salasAlrededor)
            {
                if (maximoSalas > 0)
                {
                    switch (sala)
                    {
                        case 1:
                            ponerSalaEnUno(row, col - 1);
                            break;
                        case 2:
                            ponerSalaEnUno(row + 1, col);
                            break;
                        case 3:
                            ponerSalaEnUno(row, col + 1);
                            break;
                        case 4:
                            ponerSalaEnUno(row - 1, col);
                            break;
                    }
                }
            }
        }
        //Mira si la posicion que se le pasa es diferente de cero, si es cero vuelve a la funcion de generar el mapa
        private void ponerSalaEnUno(int row, int col)
        {
            if (matrix[row, col] != 0)
                return;

            else
            {
                GeneracionMapa(row, col);
            }
        }
        //Recorre la matriz, donde no sea 0 instancia un tipo de sala
        private void GenerarSalas()
        {
            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    if (matrix[row, col] != 0)
                        instanciarSala(row, col, matrix[row, col]);
                }
            }
        }
        //Instancia un tipo de sala en funcion del numerin que se le pasa
        private void instanciarSala(int row, int col, int tipoSala)
        {
            int posicionX = ((m_PosicionOriginal - col) * m_StatsHabitaciones.habitacionPosicionX);
            int posicionY = ((m_PosicionOriginal - row) * m_StatsHabitaciones.habitacionPosicionY);
            GameObject sala;
            Tilemap SueloSala;
            GameObject luz;
            switch (tipoSala)
            {
                case 1:
                    sala = Instantiate(m_Sala, paredesParent);
                    SueloSala = Instantiate(m_tilemapSueloSave);
                    luz = Instantiate(m_Luces, sala.transform);
                    break;
                case 2:
                    sala = Instantiate(m_SalaEnemigos, paredesParent);
                    luz = Instantiate(m_Luces, sala.transform);
                    sala.GetComponent<SalaEnemigos>().Pool = m_Pool;
                    SueloSala = Instantiate(m_tilemapSueloEnemigo);
                    if (NuevaPartida)
                    {

                        sala.GetComponent<SalaEnemigos>().Pool = m_Pool;
                        sala.GetComponent<SalaEnemigos>().Init();
                    }
                    else
                        sala.GetComponent<SalaEnemigos>().Pool = m_Pool;
                    break;
                case 3:
                    sala = Instantiate(m_SalaTienda, paredesParent);
                    luz = Instantiate(m_Luces, sala.transform);
                    SueloSala = Instantiate(m_tilemapSueloTienda);
                    break;
                case 4:
                    sala = Instantiate(m_SalaLoot, paredesParent);
                    luz = Instantiate(m_Luces, sala.transform);
                    SueloSala = Instantiate(m_tilemapSueloLoot);
                    if (NuevaPartida)
                        sala.GetComponent<SalaLoot>().Init();
                    break;
                case 5:
                    sala = Instantiate(m_SalaJefe, paredesParent);
                    luz = Instantiate(m_Luces, sala.transform);
                    sala.GetComponent<SalaBoss>().Pool = m_Pool;
                    SueloSala = Instantiate(m_tilemapSueloJefe);
                    if (NuevaPartida)
                    {
                        sala.GetComponent<SalaBoss>().Pool = m_Pool;
                        sala.GetComponent<SalaBoss>().Init();
                    }
                    break;
                default:
                    sala = null;
                    SueloSala = null;
                    luz = null;
                    break;
            }

            luz.transform.position = Vector3.zero;
            SueloSala.transform.parent = m_Grid.transform;
            SueloSala.transform.position = new Vector3(posicionX, posicionY, 0);
            sala.transform.position = new Vector3(posicionX, posicionY, 0);
            ColocarPuertas(sala, row, col);
        }
        //Coloca puertas en funcion de si la posicion que se le pasa tiene diferentes numeros a 0 alrededor en la matriz
        private void ColocarPuertas(GameObject sala, int row, int col)
        {
            //ARRIBA
            if (matrix[row - 1, col] != 0)
            {
                Tilemap PuertaSala = Instantiate(m_tilemapPuertaArriba);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_PuertaHorizontal);
                puerta.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y + m_StatsHabitaciones.paredPosicionY, sala.transform.position.z);
                puerta.transform.localEulerAngles = new Vector3(0, 0, 180);
                puerta.transform.parent = sala.transform;
                puerta.GetComponentInChildren<ScriptPuerta>().Init(1);
            }
            else
            {
                Tilemap PuertaSala = Instantiate(m_tilemapParedArriba);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_ParedHorizontal);
                puerta.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y + m_StatsHabitaciones.paredPosicionY, sala.transform.position.z);
                puerta.transform.parent = sala.transform;
            }
            //DERECHA
            if (matrix[row, col - 1] != 0)
            {
                Tilemap PuertaSala = Instantiate(m_tilemapPuertaDerecha);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_PuertaVertical);
                puerta.transform.position = new Vector3(sala.transform.position.x + m_StatsHabitaciones.paredPosicionX, sala.transform.position.y, sala.transform.position.z);
                puerta.transform.localEulerAngles = new Vector3(0, 0, 180);
                puerta.transform.parent = sala.transform;
                puerta.GetComponentInChildren<ScriptPuerta>().Init(2);
            }
            else
            {
                Tilemap PuertaSala = Instantiate(m_tilemapParedDerecha);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_ParedVertical);
                puerta.transform.position = new Vector3(sala.transform.position.x + m_StatsHabitaciones.paredPosicionX, sala.transform.position.y, sala.transform.position.z);
                puerta.transform.parent = sala.transform;
            }
            //ABAJO
            if (matrix[row + 1, col] != 0)
            {
                Tilemap PuertaSala = Instantiate(m_tilemapPuertaAbajo);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_PuertaHorizontal);
                puerta.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y - m_StatsHabitaciones.paredPosicionY, sala.transform.position.z);
                puerta.transform.parent = sala.transform;
                puerta.GetComponentInChildren<ScriptPuerta>().Init(3);
            }
            else
            {
                Tilemap PuertaSala = Instantiate(m_tilemapParedAbajo);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_ParedHorizontal);
                puerta.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y - m_StatsHabitaciones.paredPosicionY, sala.transform.position.z);
                puerta.transform.parent = sala.transform;
            }
            //IZQUIERDA
            if (matrix[row, col + 1] != 0)
            {
                Tilemap PuertaSala = Instantiate(m_tilemapPuertaIzquierda);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_PuertaVertical);
                puerta.transform.position = new Vector3(sala.transform.position.x - m_StatsHabitaciones.paredPosicionX, sala.transform.position.y, sala.transform.position.z);
                puerta.transform.parent = sala.transform;
                puerta.GetComponentInChildren<ScriptPuerta>().Init(4);
            }
            else
            {
                Tilemap PuertaSala = Instantiate(m_tilemapParedIzquierda);
                PuertaSala.transform.position = new Vector3(sala.transform.position.x, sala.transform.position.y, sala.transform.position.z);
                PuertaSala.transform.parent = m_Grid.transform;
                GameObject puerta = Instantiate(m_ParedVertical);
                puerta.transform.position = new Vector3(sala.transform.position.x - m_StatsHabitaciones.paredPosicionX, sala.transform.position.y, sala.transform.position.z);
                puerta.transform.parent = sala.transform;
            }
        }
        public void FromSaveData(SaveGame data)
        {
            
            Load(data.matriz);
            matrizAcabada?.Invoke(matrix);
            GenerarSalas();
        }
        public int[] Save()
        {

            int[] matrixGuardar;
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            matrixGuardar = new int[rows * cols];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrixGuardar[index] = matrix[i, j];
                    index++;
                }
            }

            return matrixGuardar;
        }

        public void Load(int[] _matrixData)
        {
            int[,] reconstructedMatrix = new int[41, 41];

            int index = 0;
            for (int i = 0; i < 41; i++)
            {
                for (int j = 0; j < 41; j++)
                {
                    reconstructedMatrix[i, j] = _matrixData[index];
                    index++;
                }
            }
            matrix = reconstructedMatrix;
        }
    }
}