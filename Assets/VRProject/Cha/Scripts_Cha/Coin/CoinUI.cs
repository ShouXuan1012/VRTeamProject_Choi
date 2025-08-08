using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class CoinUI : MonoBehaviour
{ 
    
    public Text coinText;
    public ParticleSystem particle; // 파티클 시스템

    public AudioSource coinAudio; // 코인 추가 사운드
    public AudioClip[] coinSounds; // 코인 사운드 클립 배열
    //private void Start()
    //{
    //    if(CoinManager.Instance != null)
    //    {
    //        CoinManager.Instance.OnCoinChanged += UpdateCoinText;
    //        UpdateCoinText(CoinManager.Instance.CurrentCoins);
    //    }
    //}
    //
    //void OnEnable()
    //{
    //    if (CoinManager.Instance != null)
    //    {
    //        CoinManager.Instance.OnCoinChanged += UpdateCoinText;
    //        UpdateCoinText(CoinManager.Instance.CurrentCoins);
    //    }
    //          
    //}
    //void OnDisable()
    //{
    // if(CoinManager.Instance!= null)
    //    {
    //        CoinManager.Instance.OnCoinChanged -=UpdateCoinText;
    //    }
    //}

    public void UpdateCoinText(int coins)
    {
        if (coinText == null)
        {
            Debug.LogWarning("[CoinUI] coinText가 null입니다.");
            return;
        }
        coinText.text = $"{coins:N0}₩";
       
    }

    public IEnumerator PlayForSeconds(float duration)
    {
        particle.Play();              // 재생 시작
        yield return new WaitForSeconds(duration);
        particle.Stop();             // 재생 멈춤 (남은 파티클은 끝까지 사라짐)
    }

    public void PlayCoinSound(int index)
    {
        coinAudio.PlayOneShot(coinSounds[index]);
    }
}