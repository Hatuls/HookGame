using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compressor : MonoBehaviour
{
    
    public int Charges;
    [SerializeField] int maxCharges;
    [SerializeField] float PulseStrenght;
    public ParticleSystem PulseParticles;
   public void Pulse()
   {
        if (Charges > 0)
        {

        PulseParticles.Play();
        transform.root.GetComponent<Rigidbody>().AddForce(-transform.forward * PulseStrenght,ForceMode.Impulse);
            Charges--;
        }
        else
        {
            Debug.Log("Out of charges");
        }
   }
    public void Charge(int chargeAmmount)
    {
        Charges += chargeAmmount;
        if (Charges > maxCharges) Charges = maxCharges;

    }
}
