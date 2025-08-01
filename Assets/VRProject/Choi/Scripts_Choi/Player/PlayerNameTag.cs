using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField] private Text nicknameText;
    [SerializeField] private Text titleText;

    //지금은 업적을 저장을 안해서 제가 바로 할당을 합니다
    [SerializeField] private TitleDropdownController titleDropdownController;

    private Camera mainCam;

    private void Start()
    {
        if (photonView.IsMine)
        {
            gameObject.SetActive(false);
            return;
        }
        mainCam = Camera.main;

        var user = CurrentUserManager.Instance.CurrentUserData;
        nicknameText.text = user.nickname;

        //나중에 칭호 서버에 저장하면 불러와서 쓰기(지금은 로컬에 선택한것 띄우기)
        titleText.text = titleDropdownController.CurrentTitle; 
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
