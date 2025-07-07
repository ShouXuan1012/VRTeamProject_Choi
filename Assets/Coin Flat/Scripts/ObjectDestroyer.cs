using UnityEngine;

namespace ObjectDestroy // Замените "MyNamespace" на ваше пространство имен
{
    public class ObjectDestroyer : MonoBehaviour
    {
        public float destroyDelay = 3f; // Задержка перед уничтожением объекта

        void Start()
        {
            Destroy(gameObject, destroyDelay); // Уничтожаем объект через заданное время
        }
    }
}