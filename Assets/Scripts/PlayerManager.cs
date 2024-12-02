using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    public List<ItemWeapon> weapons = new List<ItemWeapon>();
    public List<GameObject> playerPrefabs = new List<GameObject>();
    public Transform spawnPoint;

    private int indexPlayer;
    private int indexWeapon;

    private void Awake()
    {
        Instance = this;
        indexPlayer = FileReadWrite.Instance.player.selectPlayer;
        indexWeapon = FileReadWrite.Instance.player.idItemWeapon;
    }
    private void Start()
    {
        SpawnPLayer();
    }

    public void SpawnPLayer ()
    {
        GameObject newPlayer = Instantiate(playerPrefabs[indexPlayer],spawnPoint.position,Quaternion.identity);
        newPlayer.name = "Player";
        Weapon weapon = newPlayer.transform.Find("Weapon").GetComponent<Weapon>();
        if (indexWeapon == -1)
        {
            weapon.gameObject.transform.Find("Gun").gameObject.SetActive(false);
            weapon.gameObject.transform.Find("Melee-1").gameObject.SetActive(true);
            return;
        }
        weapon.itemWeapon = weapons[indexWeapon];
    }
}
