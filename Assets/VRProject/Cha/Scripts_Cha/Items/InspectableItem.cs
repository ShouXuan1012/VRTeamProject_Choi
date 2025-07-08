using Photon.Voice.Unity;
using UnityEngine;


public class InspectableItem : MonoBehaviour
{
    public GameObject infoPanel;
    public int price;
    //public AudioClip useSound;
    //public GameObject usevfx;

    public void ShowInfo()
    {
        infoPanel.SetActive(true);
    }
    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
    public void TryPurchase()
    {
        PurchasePopup.Instance.Show(() =>
        {
            if (CoinManager.Instance.UseCoins(price))
            { //if (useSound != null)
                   // AudioManager.Instance.Play(useSound.name);
                //if (usevfx != null)
                    //Instantiate(usevfx, transform.position, Quaternion.identity);

            }
            else
            { Debug.Log("코인이 부족합니다."); }

        },this);
    }
}
