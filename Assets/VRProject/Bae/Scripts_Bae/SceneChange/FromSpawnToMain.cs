using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FromSpawnToMain : MonoBehaviour
{
    public string SceneName = "MainScene"; // 변경할 씬 이름
    public float delayBeforeChange = 2f; // 씬 변경 전 대기 시간

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(LoadSceneCo());
        }
    }

    private IEnumerator LoadSceneCo() 
    {
        yield return new WaitForSeconds(delayBeforeChange);
        
        Debug.Log("[FromSpawnToMain] 씬 변경 시작: " + SceneName);
        Debug.Log("[FromSpawnToMain] 대기 시간: " + delayBeforeChange + "초");

        SceneManager.LoadScene(SceneName);
    }
}
