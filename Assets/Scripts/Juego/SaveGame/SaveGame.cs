using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static IsaacBongos.SaveGame;

namespace IsaacBongos
{
    [Serializable]
    public class SaveGame
    {
        [Serializable]
        public struct SalasData
        {
            public Vector3 transformSala;
            public spawnedInfo[] objects;

            public SalasData(Vector3 _transform, spawnedInfo[] _objects)
            {
                transformSala = _transform;
                objects = _objects;
            }
        }

        [Serializable]
        public struct spawnedInfo
        {
            public Vector3 transformSpawned;
            public int numberOfSpawn;

            public spawnedInfo(Vector3 _transform, int _numberOfSpawn)
            {
                transformSpawned = _transform;
                numberOfSpawn = _numberOfSpawn;
            }
        }

        [Serializable]
        public struct ItemSlotInfo
        {
            public string item;
            public int amount;

            public ItemSlotInfo(string _item, int _amount)
            {
                item = _item;
                amount = _amount;
            }
        }

        [Serializable]
        public struct playerSave
        {
            public Vector3 posicionPlayer;
            public ItemSlotInfo[] items;
            public bool jugador1;
            public int dinero;
            public float vida;

            public playerSave(Vector3 _posicionPlayer, ItemSlotInfo[] backpack, bool _jugador1, int _dinero, float _vida)
            {
                posicionPlayer = _posicionPlayer;
                items = new ItemSlotInfo[backpack.Length];
                jugador1 = _jugador1;
                dinero = _dinero;
                vida = _vida;

                for(int i = 0; i < backpack.Length;i++)
                    items[i] = backpack[i];
            }
        }

        [Serializable]
        public struct miniMapData
        {
            public int posicionPlayerX;
            public int posicionPlayerY;
            public int[] matrizGUI;

            public miniMapData(int _posicionPlayerX, int _posicionPlayerY, int[] _matrizGUI)
            {
               posicionPlayerX = _posicionPlayerX;
               posicionPlayerY = _posicionPlayerY;
               matrizGUI = _matrizGUI;
            }
        }

        [Serializable]
        public struct PosCamera
        {
            public int cameraPosX;
            public int cameraPosY;

            public PosCamera(int _cameraPosX, int _cameraPosY)
            {
                cameraPosX = _cameraPosX;
                cameraPosY = _cameraPosY;
            }
        }

        public PosCamera m_PosCamera;

        public playerSave[] m_PlayersData;

        public SalasData[] m_Objects;

        public SalasData[] m_Enemies;

        public SalasData m_Jefe;

        public miniMapData m_MiniMapData;

        public int[] matriz;
        public void PopulateData(ISaveableMatrix _matrixData)
        {
            matriz = _matrixData.Save();
        }
        public void PopulateDataMiniMap(ISaveableMiniMap _matrixData)
        {
            m_MiniMapData = _matrixData.Save();
        }
        public void PopulateDataEnemies(ISaveableSalaData[] _matrixData)
        {

            m_Enemies = new SalasData[_matrixData.Length];
            for (int i = 0; i < _matrixData.Length; i++)
                m_Enemies[i] = _matrixData[i].Save();

        }
        public void PopulateDataObjects(ISaveableSalaData[] _matrixData)
        {

            m_Objects = new SalasData[_matrixData.Length];
            for (int i = 0; i < _matrixData.Length; i++)
                m_Objects[i] = _matrixData[i].Save();

        }
        public void PopulateDataPlayer(ISaveablePlayer[] _player)
        {
            m_PlayersData = new playerSave[_player.Length];
            for (int i = 0; i < _player.Length; i++)
                m_PlayersData[i] = _player[i].Save();
        }

        public void PopulateDataJefe(ISaveableSalaData _boss)
        {
            m_Jefe = _boss.Save();
        }

        public void PopulateDataPosCamera(ISaveablePosCamera _posCamera)
        {
            m_PosCamera = _posCamera.Save();
        }
    }

    public interface ISaveableSalaData
    {
        public SalasData Save();
        public void Load(SalasData _salaData);
    }

    public interface ISaveableMatrix
    {
        public int[] Save();
        public void Load(int[] _matrixData);
    }

    public interface ISaveablePlayer
    {
        public playerSave Save();

        public void Load(playerSave playerData);
    }

    public interface ISaveableMiniMap
    {
        public miniMapData Save();

        public void Load(miniMapData playerData);
    }

    public interface ISaveablePosCamera
    {
        public PosCamera Save();

        public void Load(PosCamera posCamera);
    }

}