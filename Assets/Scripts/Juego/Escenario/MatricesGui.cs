using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace IsaacBongos
{
    public class MatricesGui : MonoBehaviour, ISaveableMiniMap
    {
        private int[,] matrix;
        private int[,] matrixTapada = new int[41, 41];
        [SerializeField]
        private Image imagenSala;
        [SerializeField]
        private Image imagenSalaActual;
        private Image m_imagenSalaActualMapa;
        [SerializeField]
        private GameObject minimapa;
        private int posPlayerX;
        private int posPlayerY;

        List<ImagePosXPosY> listaImagen = new List<ImagePosXPosY>();

        private struct ImagePosXPosY
        {
            public int posXValue;
            public int posYValue;
            public Image ImageGuiValue;

            public ImagePosXPosY(int posX, int posY, Image ImageGui)
            {
                posXValue = posX;
                posYValue = posY;
                ImageGuiValue = ImageGui;
            }

        }

        private void Awake()
        {
            Matrices.matrizAcabada += ObtenerMatriz;
            GameManager.onCanviSala += DestaparMatriz;
            ColocarSalaActual();
        }

        private void ColocarSalaActual()
        {
            m_imagenSalaActualMapa = Instantiate(imagenSalaActual);
            m_imagenSalaActualMapa.transform.SetParent(minimapa.transform, false);
            m_imagenSalaActualMapa.transform.localPosition = new Vector3(0, 0, 0);
        }

        private void getPosicionPlayerInicial()
        {
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    if (matrix[y, x] == 1)
                    {
                        posPlayerX = x;
                        posPlayerY = y;
                    }
                }
            }
        }

        private void SalasDeAlLado()
        {
            for (int x = 0; x < matrixTapada.GetLength(0); x++)
            {
                for (int y = 0; y < matrixTapada.GetLength(1); y++)
                {
                    if (matrixTapada[y, x] == 9)
                    {
                        Image imagen = Instantiate(imagenSala);
                        imagen.transform.SetParent(minimapa.transform, false);
                        imagen.transform.localPosition = new Vector3((x - 20) * -60, (y - 20) * -60, 0);
                        listaImagen.Add(new ImagePosXPosY(x, y, imagen));
                    }
                }
            }
            DestaparMatriz(posPlayerX, posPlayerY);
        }



        private void ObtenerMatriz(int[,] obj)
        {
            matrix = obj;
            getPosicionPlayerInicial();
            TaparMatriz();
        }

        private void TaparMatriz()
        {
            for (int y = 0; y < matrixTapada.GetLength(0); y++)
            {
                for (int x = 0; x < matrixTapada.GetLength(1); x++)
                {

                    if (matrix[y, x] != 0)
                    {
                        matrixTapada[y, x] = 9;
                    }
                    else
                    {
                        matrixTapada[y, x] = 0;
                    }
                }
            }
            SalasDeAlLado();
        }

        private void DestaparMatriz(int posX, int posY)
        {
            int posXFinal = 20 + (20 - posX);
            int posYFinal = 20 + (20 - posY);
            matrixTapada[posX, posY] = matrix[posYFinal, posXFinal];
            posPlayerX= posXFinal;
            posPlayerY= posYFinal;
            ImagePosXPosY jeje = listaImagen.FirstOrDefault(x => x.posXValue == posXFinal && x.posYValue == posYFinal);

            switch (matrix[posYFinal, posXFinal])
            {
                case 1:
                    jeje.ImageGuiValue.color = Color.blue;
                    break;
                case 2:
                    jeje.ImageGuiValue.color = Color.magenta;
                    break;
                case 3:
                    jeje.ImageGuiValue.color = Color.green;
                    break;
                case 4:
                    jeje.ImageGuiValue.color = Color.yellow;
                    break;
                case 5:
                    jeje.ImageGuiValue.color = Color.red;
                    break;
            }
            m_imagenSalaActualMapa.transform.localPosition = new Vector3((posXFinal - 20) * -60, (posYFinal - 20) * -60, 0); ;
        }

        public SaveGame.miniMapData Save()
        {
            int[] matrixGuardar;
            int rows = matrixTapada.GetLength(0);
            int cols = matrixTapada.GetLength(1);
            matrixGuardar = new int[rows * cols];

            int index = 0;
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrixGuardar[index] = matrixTapada[i, j];
                    index++;
                }
            }
            SaveGame.miniMapData miniMapData = new SaveGame.miniMapData(posPlayerX, posPlayerY, matrixGuardar);
            return miniMapData;
        }

        public void Load(SaveGame.miniMapData playerData)
        {
           
            int[,] reconstructedMatrix = new int[41, 41];

            int index = 0;
            for (int i = 0; i < 41; i++)
            {
                for (int j = 0; j < 41; j++)
                {
                    reconstructedMatrix[i, j] = playerData.matrizGUI[index];
                    index++;
                }
            }
            matrixTapada = reconstructedMatrix;
            for (int i = 0; i < 41; i++)
            {
                for (int j = 0; j < 41; j++)
                {
                    if (matrixTapada[i, j] != 9 && matrixTapada[i, j] != 0)
                        DestaparMatriz(i,j);
                }
            }
            posPlayerX = playerData.posicionPlayerX;
            posPlayerY = playerData.posicionPlayerY;

            m_imagenSalaActualMapa.transform.localPosition = new Vector3((posPlayerX - 20) * -60, (posPlayerY - 20) * -60, 0); ;

           
        }

        private void OnDestroy()
        {
            Matrices.matrizAcabada -= ObtenerMatriz;
            GameManager.onCanviSala -= DestaparMatriz;
        }
    }
}