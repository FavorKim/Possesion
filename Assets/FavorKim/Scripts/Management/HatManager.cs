using UnityEngine;

// 모자에 부착하여, 관련한 함수들을 정의하는 매니저 클래스
public class HatManager : MonoBehaviour
{
    [SerializeField] GameObject hatPref; // 던지는 모자
    [SerializeField] Transform hatTransform;
    GameObject hat;
    [SerializeField] GameObject hatImg; // 기본적으로 착용하는 모자
    [SerializeField] GameObject hitParticlePref;
    ParticleSystem hitParticle;
    [SerializeField]AudioSource sfx;

    public GameObject GetHatImg() { return hatImg; }


    private void Awake()
    {
        hat = Instantiate(hatPref);
        hat.SetActive(false);
        hitParticle = Instantiate(hitParticlePref).GetComponent<ParticleSystem>();
        sfx = hitParticle.gameObject.GetComponent<AudioSource>();
        hitParticle.Stop();
    }

    public void ThrowHat(Vector3 dir)
    {
        if (hat.activeSelf) return;

        hat.transform.position = hatTransform.position;
        hat.transform.rotation = hatTransform.parent.rotation;
        hat.transform.LookAt(dir);
        hatImg.SetActive(false);
        hat.SetActive(true);
    }

    public void SetHitParticle(Vector3 pos)
    {
        hitParticle.transform.position = pos;
        sfx.Play();
        hitParticle.Play();
    }
}
