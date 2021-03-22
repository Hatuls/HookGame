using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellCharger : MonoBehaviour
{
    
    [SerializeField] int ChargeAmmount;
    [SerializeField] float DistructionTime;
    [SerializeField] short ParticleAmount;
    ParticleSystem ps;

    private void Start()
    {
        
        ps = GetComponent<ParticleSystem>();
    }

    public int TakeCharge()
    {
        var em = ps.emission;
        em.enabled = true;

        em.rateOverTime = 20.0f;

        em.SetBursts(
            new ParticleSystem.Burst[]
            {
                new ParticleSystem.Burst(0, ParticleAmount)
               
            }
            );

        StartCoroutine(SelfDestruct());
        return ChargeAmmount;
    }
    IEnumerator SelfDestruct()
    {
        ps.Play();
        yield return new WaitForSeconds(DistructionTime);
        gameObject.SetActive(false);
    }
}
