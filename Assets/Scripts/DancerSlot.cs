using UnityEngine;

[System.Serializable]
public class DancerSlot
{
    public Transform spawnPoint;
    public GameObject birdPrefab;

    private GameObject spawnedBird;

    public void Activate(string danceTrigger)
    {
        if (spawnPoint == null || birdPrefab == null) return;

        if (spawnedBird != null)
            Object.Destroy(spawnedBird);

        spawnedBird = Object.Instantiate(birdPrefab, spawnPoint.position, spawnPoint.rotation, spawnPoint);

        Animator anim = spawnedBird.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger(danceTrigger);
    }

    public void Deactivate(string idleTrigger)
    {
        if (spawnedBird == null) return;

        Animator anim = spawnedBird.GetComponent<Animator>();
        if (anim != null) anim.SetTrigger(idleTrigger);

        Object.Destroy(spawnedBird);
        spawnedBird = null;
    }
}
