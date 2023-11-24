using m17;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IsaacBongos
{
    public class GameManager : MonoBehaviour, ISaveablePosCamera
    {
        private static GameManager m_Instance;
        public static GameManager Instance
        {
            get { return m_Instance; }
        }

        private bool m_NuevaPartida;
        public bool NuevaPartida => m_NuevaPartida;

        [SerializeField]
        private GameObject m_Jugador1;
        private GameObject m_Jugador1Instance;
        [SerializeField]
        private GameObject m_Jugador2;
        private GameObject m_Jugador2Instance;


        [SerializeField]
        private GameEvent CargarPartida;
        [SerializeField]
        private GameEvent GuardarPartida;

        public static Action<int, int> onCanviSala;
        private int posx;
        private int posy;

        private Camera m_Camera;
        [SerializeField]
        private GameObject m_pool;
        private GameObject pool;

        private void Awake()
        {
            if (m_Instance == null)
                m_Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Alex")
            {
                pool = Instantiate(m_pool);
                m_Jugador1Instance = Instantiate(m_Jugador1);
                m_Jugador1Instance.transform.position = new Vector3(-1, 0, 0);
                m_Jugador1Instance.GetComponent<PJSMB>().Init(true);
                m_Jugador1Instance.GetComponent<PJSMB>().AcabarPartida += AcabarPartida;
                m_Jugador1Instance.GetComponent<SMBShootingState>().Pool = pool.GetComponent<Pool>(); ;
                m_Jugador2Instance = Instantiate(m_Jugador2);
                m_Jugador2Instance.GetComponent<PJSMB>().AcabarPartida += AcabarPartida;
                m_Jugador2Instance.transform.position = new Vector3(1, 0, 0);
                m_Jugador2Instance.GetComponent<PJSMB>().Init(false);
                m_Jugador2Instance.GetComponent<SMBShootingState>().Pool = pool.GetComponent<Pool>();
                m_Camera = Camera.main;
                posx = 20;
                posy = 20;
                FindObjectOfType<Matrices>().Init(m_NuevaPartida);
                if (!m_NuevaPartida)
                    CargarPartida.Raise();
            }
        }

        public void GuardarJuego()
        {
            GuardarPartida.Raise();
        }

        public void AcabarPartida()
        {
            SceneManager.LoadScene("Inicio");
        }

        public void CargarJuego()
        {
            m_NuevaPartida = false;
            SceneManager.LoadScene("Alex");

        }

        public void IniciarJuego()
        {
            m_NuevaPartida = true;
            SceneManager.LoadScene("Alex");
        }

        public void CambiarPosicionCamara(int _numerin)
        {
            Vector3 nuevaPosicionCamara;
            switch (_numerin)
            {
                case 1:
                    nuevaPosicionCamara = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y + 10, m_Camera.transform.position.z);
                    posy++;
                    onCanviSala?.Invoke(posx, posy);
                    m_Jugador1Instance.transform.position = new Vector3(m_Jugador1Instance.transform.position.x, m_Jugador1Instance.transform.position.y + 2, m_Jugador1Instance.transform.position.z);
                    m_Jugador2Instance.transform.position = new Vector3(m_Jugador2Instance.transform.position.x, m_Jugador2Instance.transform.position.y + 2, m_Jugador2Instance.transform.position.z);
                    break;
                case 2:
                    nuevaPosicionCamara = new Vector3(m_Camera.transform.position.x + 18, m_Camera.transform.position.y, m_Camera.transform.position.z);
                    posx++;
                    onCanviSala?.Invoke(posx, posy);
                    m_Jugador1Instance.transform.position = new Vector3(m_Jugador1Instance.transform.position.x + 2, m_Jugador1Instance.transform.position.y, m_Jugador1Instance.transform.position.z);
                    m_Jugador2Instance.transform.position = new Vector3(m_Jugador2Instance.transform.position.x + 2, m_Jugador2Instance.transform.position.y, m_Jugador2Instance.transform.position.z);
                    break;
                case 3:
                    nuevaPosicionCamara = new Vector3(m_Camera.transform.position.x, m_Camera.transform.position.y - 10, m_Camera.transform.position.z);
                    posy--;
                    onCanviSala?.Invoke(posx, posy);
                    m_Jugador1Instance.transform.position = new Vector3(m_Jugador1Instance.transform.position.x, m_Jugador1Instance.transform.position.y - 2, m_Jugador1Instance.transform.position.z);
                    m_Jugador2Instance.transform.position = new Vector3(m_Jugador2Instance.transform.position.x, m_Jugador2Instance.transform.position.y - 2, m_Jugador2Instance.transform.position.z);
                    break;
                case 4:
                    nuevaPosicionCamara = new Vector3(m_Camera.transform.position.x - 18, m_Camera.transform.position.y, m_Camera.transform.position.z);
                    posx--;
                    onCanviSala?.Invoke(posx, posy);
                    m_Jugador1Instance.transform.position = new Vector3(m_Jugador1Instance.transform.position.x - 2, m_Jugador1Instance.transform.position.y, m_Jugador1Instance.transform.position.z);
                    m_Jugador2Instance.transform.position = new Vector3(m_Jugador2Instance.transform.position.x - 2, m_Jugador2Instance.transform.position.y, m_Jugador2Instance.transform.position.z);
                    break;
                default:
                    nuevaPosicionCamara = Vector3.zero;
                    break;
            }
            StartCoroutine(MoverCamara(nuevaPosicionCamara));
        }

        private IEnumerator MoverCamara(Vector3 posicionFinal)
        {
            while (Vector3.Distance(m_Camera.transform.position, posicionFinal) > 0.001f)
            {
                float step = 50 * Time.deltaTime;
                m_Camera.transform.position = Vector3.MoveTowards(m_Camera.transform.position, posicionFinal, step);
                yield return new WaitForEndOfFrame();
            }
            m_Camera.transform.position = posicionFinal;
        }

        public SaveGame.PosCamera Save()
        {
            SaveGame.PosCamera posCamera = new SaveGame.PosCamera(posx, posy);
            return posCamera;
        }

        public void Load(SaveGame.PosCamera posCamera)
        {
            m_Camera.transform.position = new Vector3((posCamera.cameraPosX - 20) * 18, (posCamera.cameraPosY - 20) * 10, -10);
            posx= posCamera.cameraPosX;
            posy= posCamera.cameraPosY;
        }
    }
}