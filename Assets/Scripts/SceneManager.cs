using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField] List<GameObject> blocksPref;
    [SerializeField] List<GameObject> places;
    int countPlaces;

    [SerializeField] int idChoseBlock = 0;

    private void Start()
    {
        countPlaces = 0;
    }

    public void SpawnBlock(Transform place)
    {
        Instantiate(blocksPref[idChoseBlock], place.transform.position, Quaternion.Euler(0, place.rotation.y, place.rotation.z));
    }

    public bool SpawnPlaces(Transform spawnPos)
    {
        if (countPlaces < places.Count)
        {
            Instantiate(places[countPlaces], spawnPos.transform.position, Quaternion.identity);
            countPlaces++;
        }

        //Возвращает, может ещё спавнить или нет
        if (countPlaces == places.Count) return false;
        else return true;
    }

    public void ChoiceBlock(int id)
    {
        idChoseBlock = id;
    }
}
