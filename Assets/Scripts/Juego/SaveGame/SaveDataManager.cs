using IsaacBongos;
using m17;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class SaveDataManager : MonoBehaviour
{
    private const string saveFileName = "savegame.json";

    public void SaveData()
    {

        ISaveableMatrix matrix = FindObjectOfType<Matrices>();
        ISaveableSalaData[] salasEnemigoData = FindObjectsByType<SalaEnemigos>(FindObjectsSortMode.None);
        ISaveableSalaData[] salasLootData = FindObjectsByType<SalaLoot>(FindObjectsSortMode.None);
        ISaveablePlayer[] playersData = FindObjectsByType<PJSMB>(FindObjectsSortMode.None);
        ISaveableMiniMap miniMapData = FindObjectOfType<MatricesGui>();
        ISaveablePosCamera posCamera = FindObjectOfType<GameManager>();
        ISaveableSalaData salaEnemigo = FindObjectOfType<SalaBoss>();
        SaveGame data = new SaveGame();
        data.PopulateData(matrix);
        data.PopulateDataEnemies(salasEnemigoData);
        data.PopulateDataObjects(salasLootData);
        data.PopulateDataPlayer(playersData);
        data.PopulateDataMiniMap(miniMapData);
        data.PopulateDataPosCamera(posCamera);
        data.PopulateDataJefe(salaEnemigo);
        string jsonData = JsonUtility.ToJson(data);

        try
        {
            Debug.Log("Saving: ");
            Debug.Log(jsonData);

            File.WriteAllText(saveFileName, jsonData);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error while trying to save {Path.Combine(Application.persistentDataPath, saveFileName)} with exception {e}");
        }
    }

    public void LoadData()
    {
        try
        {
            print("cargar");
            string jsonData = File.ReadAllText(saveFileName);
            print(jsonData);
            SaveGame data = new SaveGame();
            JsonUtility.FromJsonOverwrite(jsonData, data);

            FindObjectOfType<Matrices>().FromSaveData(data);
            SalaEnemigos[] salasEnemigos = FindObjectsByType<SalaEnemigos>(FindObjectsSortMode.None);
           
            foreach(SalaEnemigos salita in salasEnemigos)
            {
                salita.Init(data);
            }

            SalaBoss salasBoss = FindObjectOfType<SalaBoss>();
            salasBoss.Init();

            SalaLoot[] salasLoot = FindObjectsByType<SalaLoot>(FindObjectsSortMode.None);
            for (int i = 0; i < salasLoot.Length; i++)
            {
                
                foreach(SaveGame.SalasData salita in data.m_Objects)
                {
                    if (salita.transformSala == salasLoot[i].transform.position)
                        salasLoot[i].Load(salita);
                }
                    
            }

            MatricesGui matrizGui = FindObjectOfType<MatricesGui>();

            matrizGui.Load(data.m_MiniMapData);

            GameManager gameManager = GameManager.Instance;

            gameManager.Load(data.m_PosCamera);

            PJSMB[] jugadores = FindObjectsByType<PJSMB>(FindObjectsSortMode.None);
            for (int i = 0; i < jugadores.Length; i++)
            {
                foreach(SaveGame.playerSave playerData in data.m_PlayersData)
                {
                    if(playerData.jugador1 == jugadores[i].Jugador1)
                    {
                        jugadores[i].Load(playerData);
                        continue;
                    }
                }
            }

           

        }
        catch (Exception e)
        {
            Debug.LogError($"Error while trying to load {Path.Combine(Application.persistentDataPath, saveFileName)} with exception {e}");
        }
    }
}
