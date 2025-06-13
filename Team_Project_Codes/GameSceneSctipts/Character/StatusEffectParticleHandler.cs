using System;
using System.Collections;
using UnityEngine;

public class StatusEffectParticleHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem deathParticle;
    [SerializeField] private ParticleSystem[] statusEffectParticle;
    [SerializeField] private ParticleSystem[] dotDamageParticle;

    // 사망 파티클 재생
    public void PlayDeathParticle()
    {
        if(deathParticle != null)
        {
            if (!deathParticle.gameObject.activeSelf)
            {
                deathParticle.gameObject.SetActive(true);
                deathParticle.Play();
            }
            else
            {
                deathParticle.Play();
            }
        }
    }
    // 도트 데미지 파티클 재생
    public void PlayParticle(StatusEffectType statusEffectType) 
    {
        PlayDotDamageParticleCoroutine(statusEffectType);
    }
    public void PlayDotDamageParticleCoroutine(StatusEffectType statusEffectType)
    {
        switch (statusEffectType)
        {
            case StatusEffectType.Bleed:
                if (!dotDamageParticle[0].gameObject.activeSelf)
                {
                    dotDamageParticle[0].gameObject.SetActive(true);
                    dotDamageParticle[0].Play();
                }
                else
                {
                    dotDamageParticle[0].Play();
                }
                break;
            case StatusEffectType.Poison:
                if (!dotDamageParticle[1].gameObject.activeSelf)
                {
                    dotDamageParticle[1].gameObject.SetActive(true);
                    dotDamageParticle[1].Play();
                }
                else
                {
                    dotDamageParticle[1].Play();
                }
                break;
            case StatusEffectType.Burn:
                if (!dotDamageParticle[2].gameObject.activeSelf)
                {
                    dotDamageParticle[2].gameObject.SetActive(true);
                    dotDamageParticle[2].Play();
                }
                else
                {
                    dotDamageParticle[2].Play();
                }
                break;
        }
    }

    public void PlayParticle(Character_State previousState, Character_State currentState)
    {
        if(previousState != currentState)
        {
            StopStateParticle(previousState);
            StartCoroutine(PlayParticleCoroutine(currentState));
        }
    }


    public IEnumerator PlayParticleCoroutine(Character_State currentState)
    {
        yield return null;
        switch (currentState)
        {
            case Character_State.Stun:
                if (!statusEffectParticle[0].gameObject.activeSelf)
                {
                    statusEffectParticle[0].gameObject.SetActive(true);
                    statusEffectParticle[0].Play();
                }
                else
                {
                    statusEffectParticle[0].Play();
                }
                break;
            case Character_State.Freeze:
                if (!statusEffectParticle[1].gameObject.activeSelf)
                {
                    statusEffectParticle[1].gameObject.SetActive(true);
                    statusEffectParticle[1].Play();
                }
                else
                {
                    statusEffectParticle[1].Play();
                }
                yield return new WaitForSeconds(1f);
                statusEffectParticle[1].Pause();
                break;
            case Character_State.Electrocute:
                if (!statusEffectParticle[2].gameObject.activeSelf)
                {
                    statusEffectParticle[2].gameObject.SetActive(true);
                    statusEffectParticle[2].Play();
                }
                else
                {
                    statusEffectParticle[2].Play();
                }
                break;
        }
    }
    public void StopStateParticle(Character_State previousState)
    {
        switch (previousState)
        {
            case Character_State.Stun:
                statusEffectParticle[0].gameObject.SetActive(false);
                break;
            case Character_State.Freeze:
                float totalDuration = statusEffectParticle[1].main.startLifetime.constant;

                float targetTime = totalDuration * 0.8f;
                //statusEffectParticle[index].time = targetTime;
                statusEffectParticle[1].Simulate(targetTime, false, false);
                statusEffectParticle[1].Play();
                break;
            case Character_State.Electrocute:
                statusEffectParticle[2].gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }
}
