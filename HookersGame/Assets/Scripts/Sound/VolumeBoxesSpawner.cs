using UnityEngine;

public class VolumeBoxesSpawner : MonoBehaviour{

    public static VolumeBoxesSpawner Instance;
    int currentLevel;
    [SerializeField] int AmountOfBoxes;
    [SerializeField] float distanceBetweenBoxes;
    [SerializeField] GameObject prefab;
   [SerializeField] Vector3 leftStartPos, rightStartPos;
    [SerializeField] Transform buildingHolder;
    [SerializeField] bool toInvert;
    private void Awake()
    {
        Instance = this;
    }
    public void Init() { SpawnBuilding(); }

   public void SpawnBuilding() {
        bool invert = true;
        
        distanceBetweenBoxes += prefab.transform.GetChild(0).localScale.x;
        for (int i = 0; i < AmountOfBoxes; i++)
        {
            var leftBuilding = Instantiate(prefab, leftStartPos + Vector3.forward * i * distanceBetweenBoxes, Quaternion.identity, buildingHolder);
            var rightBuilding = Instantiate(prefab, rightStartPos + Vector3.forward * i * distanceBetweenBoxes, Quaternion.identity, buildingHolder);

            
            
            
            if (toInvert == false)
            {
                rightBuilding.GetComponent<VolumeBox>().GetSetBand = i % 8;
                leftBuilding.GetComponent<VolumeBox>().GetSetBand = i % 8;
            }
            else
            {

               
                if (invert)
                {
                    rightBuilding.GetComponent<VolumeBox>().GetSetBand = i % 8;
                    leftBuilding.GetComponent<VolumeBox>().GetSetBand = i % 8;
                    if (i % 8 == 0)
                        invert = false;
                }
                else
                {
                    
                    rightBuilding.GetComponent<VolumeBox>().GetSetBand = 8 - (i % 8);
                    leftBuilding.GetComponent<VolumeBox>().GetSetBand = 8 - (i % 8);
                    if (8- (i % 8) == 1)
                        invert = true;
                }


            }
           

          //  rightBuilding.GetComponent<VolumeBox>().GetSetBand = Random.Range(0, 7);
          //   leftBuilding.GetComponent<VolumeBox>().GetSetBand = Random.Range(0, 7);
        }
    
    }
}