using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CoinSpawn // Замените "MyNamespace" на ваше пространство имен
{
    public class CoinSpawner : MonoBehaviour
    {
        public List<GameObject> coinPrefabs; // Список префабов монет
        public List<float> startDelays; // Список задержек перед появлением каждой монеты
        public float spawnRadius; // Радиус появления
        public RectTransform targetPoint; // Объект, к которому полетят монеты
        public float minSpeed; // Минимальная скорость полёта монет
        public float maxSpeed; // Максимальная скорость полёта монет
        public float minStartFlightDelay; // Минимальная задержка перед началом полёта
        public float maxStartFlightDelay; // Максимальная задержка перед началом полёта
        public GameObject vfxPrefab; // Префаб VFX
        public Animator targetAnimator; // Ссылка на компонент анимации
        public Canvas canvas; // Ссылка на Canvas, к которому привязаны UI элементы
        public Canvas canvasVfx; // Ссылка на Canvas, к которому привязаны UI элементы

        void Start()
        {
            StartCoroutine(SpawnCoins());
        }

        IEnumerator SpawnCoins()
        {
            WaitForSeconds delay = new WaitForSeconds(1f); // Создаем объект для задержки

            for (int i = 0; i < coinPrefabs.Count; i++)
            {
                yield return new WaitForSeconds(startDelays[i]); // Задержка перед появлением монеты

                GameObject coinPrefab = coinPrefabs[i]; // Выбранный префаб монеты

                Vector3 spawnOffset = Random.insideUnitSphere * spawnRadius; // Случайное смещение внутри радиуса
                Vector3 spawnPosition = transform.position + spawnOffset; // Позиция появления монеты

                GameObject coin = Instantiate(coinPrefab, spawnPosition, Quaternion.identity, canvas.transform); // Создаем монету как дочерний объект Canvas

                // Направляем монету к точке назначения
                Vector3 direction = (targetPoint.position - spawnPosition).normalized;
                float speed = Random.Range(minSpeed, maxSpeed); // Случайная скорость монеты
                float startFlightDelay = Random.Range(minStartFlightDelay, maxStartFlightDelay); // Случайная задержка перед началом полёта монеты
                StartCoroutine(MoveCoin(coin.transform as RectTransform, spawnPosition, targetPoint.position, speed, startFlightDelay));
            }
        }

        IEnumerator MoveCoin(RectTransform coinTransform, Vector3 startPos, Vector3 endPos, float speed, float startFlightDelay)
        {
            yield return new WaitForSeconds(startFlightDelay); // Задержка перед началом полёта монеты

            float journeyLength = Vector3.Distance(startPos, endPos);
            float startTime = Time.time;
            while (Vector3.Distance(coinTransform.position, endPos) > 0.1f)
            {
                float distCovered = (Time.time - startTime) * speed;
                float journeyFraction = distCovered / journeyLength;
                coinTransform.position = Vector3.Lerp(startPos, endPos, journeyFraction); // Используем линейную интерполяцию для траектории полёта
                yield return null;
            }

            // Создаем эффект визуального воспроизведения при достижении конечной точки
            GameObject vfx = Instantiate(vfxPrefab, canvasVfx.transform);

            // Позиционируем эффект на месте монеты
            vfx.transform.position = endPos;

            // Проигрываем анимацию на объекте
            if (targetAnimator != null)
            {
                targetAnimator.CrossFade("CoinDestroyed", 0f); // Запускаем анимацию с начала
            }

            // Уничтожаем монету, когда она достигает конечной точки
            Destroy(coinTransform.gameObject);
        }
    }
}