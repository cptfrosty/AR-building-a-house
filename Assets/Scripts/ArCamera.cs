using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArCamera : MonoBehaviour
{
    [SerializeField] private GameObject planeMarkerPrefab;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private Button btn_toPut; //Кнопка UI с текстом "Поставить"
    [SerializeField] private Button btn_toDel; //Кнопка UI с текстом "Удалить"
    [SerializeField] private Button btnSpawnMaket; //Кнопка UI с текстом "Создать макет"
    [SerializeField] private GameObject menuBlocks; //Меню блоков

    [SerializeField] SceneManager sceneManager; //Управляющая сценой
    [SerializeField] GameObject lastGameObject; //Последний выделенный игровой объект

    //bool objectToInteractWith = false; //Объект для взаимодействия

    private ARRaycastManager arRaycastManager;

    private GameObject ActiveBlock;

    void Start()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        planeMarkerPrefab.SetActive(false);
        sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManager>();

        menuBlocks.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ShowMarker();
        RaycastCheck();
    }

    //Чтобы был маркер обязательно поставь на AR Session Origin AR PLANE MANAGER
    void ShowMarker()
    {
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);


        if (hits.Count > 0)
        {
            Debug.Log(hits[0].hitType);
            planeMarkerPrefab.transform.position = hits[0].pose.position;
            planeMarkerPrefab.SetActive(true);
        }
    }

    //Заспавнить объект [Через кнопку]
    public void SpawnObject()
    {
        sceneManager.SpawnBlock(lastGameObject.transform);
    }

    //Удалить объект [Через кнопку]
    public void DeleteObject()
    {
        Destroy(lastGameObject);
    }

    //Выбрать блок
    public void ChoiseBlock(int id)
    {
        sceneManager.ChoiceBlock(id);
        menuBlocks.SetActive(false);
    }

    public void ShowBlocks()
    {
        menuBlocks.SetActive(true);
    }

    //Создание макета
    public void CreatePlaces()
    {
        bool isSpawn = sceneManager.SpawnPlaces(planeMarkerPrefab.transform);

        if (!isSpawn) btnSpawnMaket.gameObject.SetActive(false);
    }

    //Райкаст
    void RaycastCheck()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        Debug.DrawRay(transform.position, ray.direction * 100);

        if(Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.collider.tag);

            if (!btnSpawnMaket.gameObject.activeSelf) //Если кнопка постройки не активна, то доступны другие кнопки
            {

                if (hit.collider.tag == "Block")
                {
                    btn_toDel.gameObject.SetActive(true);
                    btn_toPut.gameObject.SetActive(false);
                    lastGameObject = hit.collider.gameObject;
                    //objectToInteractWith = true;
                }

                if (hit.collider.tag == "Place")
                {
                    btn_toDel.gameObject.SetActive(false);
                    btn_toPut.gameObject.SetActive(true);
                    lastGameObject = hit.collider.gameObject;
                    //objectToInteractWith = true;
                }
            }
        }
        else
        {
            btn_toDel.gameObject.SetActive(false);
            btn_toPut.gameObject.SetActive(false);
        }
    }
}
