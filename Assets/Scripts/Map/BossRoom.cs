using UnityEngine;

public class BossRoom : LevelMapFragment, Observer
{
    [SerializeField] private BossDoor bossDoor = default;
    [SerializeField] private Transform bossSpawnPoint = default;

    public void InitWith(GameObject[] arrayEnemyGO, GameObject[] arrayPrinceGO, int nbChest, GameObject bossGO)
    {
        base.InitWith(arrayEnemyGO, arrayPrinceGO, nbChest);

        bossDoor.Close();
        CreateBoss(bossGO);
    }

    public void Notify()    // called on boss death
    {
        bossDoor.Open();
    }

    private void CreateBoss(GameObject bossGO)
    {
        GameObject bossGameObject = (GameObject) Instantiate(bossGO, bossSpawnPoint.position, Quaternion.identity, objectContainer);
        Boss boss = bossGameObject.GetComponent<Boss>();
        boss.Register(this);
    }
}
