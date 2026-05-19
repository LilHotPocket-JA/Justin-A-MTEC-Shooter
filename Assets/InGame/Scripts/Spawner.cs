using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject[] enemyPr;
    private float spawnRate = 30;
    private float TSLSpawn;
    public Transform spawnPoint;
    public Transform[] wayP;
    public Transform play;

    bool tet = false;

    private void Start()
    {
        spawnRate = Random.Range(0, 180);
        TSLSpawn = spawnRate;
    }
    public void Update()
    {
        if (TSLSpawn < spawnRate)
        {
            TSLSpawn += Time.deltaTime;
        }

        else
        {
         SpawnOpps();
         TSLSpawn = 0;
        }
    }

    private void SpawnOpps()
    {
        int index = Random.Range(0, enemyPr.Length);
        GameObject enemy = Instantiate(enemyPr[index], spawnPoint.position, Quaternion.identity);
        Enemy eS = enemy.GetComponent<Enemy>();

       for (int i = 0; i < eS.waypoints.Length; i++)
        {
         eS.waypoints[i] = wayP[i];
        }

       eS.speed = Random.Range(5, 10);
        eS.timeUration = Random.Range(1, 5);
        eS.player = play;

        if (enemy != null) 
        {
            tet = true; 
        }
    }


}
