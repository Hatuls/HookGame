using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compressor : MonoBehaviour
{
    
    public int Charges;
    [SerializeField] int maxCharges;
    [SerializeField] float timeToSelfCharge;
    [SerializeField] float pulseStrenght;
    public ParticleSystem PulseParticles;

    Coroutine selfCharge;

    public void ResetCompressor()
    {
        if (selfCharge != null)
        StopCoroutine(selfCharge);
        Charges = maxCharges;
        selfCharge = null;
    }

   public void Pulse()
   {
        if (Charges > 0)
        {

        PulseParticles.Play();
        transform.root.GetComponent<Rigidbody>().AddForce(-transform.forward * pulseStrenght,ForceMode.Impulse);
            Charges--;
            if (Charges <= 0)
            {
               selfCharge=StartCoroutine(SelfCharge());
            }
        }
        else
        {
            Debug.Log("Out of charges");
        }

   }
    IEnumerator SelfCharge()
    {
        yield return new WaitForSeconds(timeToSelfCharge);
        Charges = maxCharges;
    }

    public void Charge(int chargeAmmount)
    {
        Charges += chargeAmmount;
        if(selfCharge!=null)
        StopCoroutine(selfCharge);
        if (Charges > maxCharges) Charges = maxCharges;

    }
}
