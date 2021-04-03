using UnityEngine;

public class VolumeBoxesSpawner : MonoBehaviour{

    public static VolumeBoxesSpawner Instance;
    int currentLevel;
    [SerializeField] int AmountOfBoxes;
    [SerializeField] float distanceBetweenBoxes;
    [SerializeField] GameObject prefab;
   [SerializeField] Vector3 leftStartPos, rightStartPos;
    [SerializeField] Transform buildingHolder;
    private void Awake()
    {
        Instance = this;
    }
    public void Init() { SpawnBuilding(); }

   public void SpawnBuilding() {
        distanceBetweenBoxes += prefab.transform.GetChild(0).localScale.x;
        for (int i = 0; i < AmountOfBoxes; i++)
        {
            var leftBuilding = Instantiate(prefab, leftStartPos + Vector3.forward * i * distanceBetweenBoxes, Quaternion.identity, buildingHolder);
            var rightBuilding = Instantiate(prefab, rightStartPos + Vector3.forward * i * distanceBetweenBoxes, Quaternion.identity, buildingHolder);

            rightBuilding.GetComponent<VolumeBox>().GetSetBand = Random.Range(0, 7);
            leftBuilding.GetComponent<VolumeBox>().GetSetBand = Random.Range(0, 7);
        }
    
    }
}