using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FirestoreDBManager
{
    private static FirestoreDBManager _instance;
    public static FirestoreDBManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new FirestoreDBManager();
            }
            return _instance;
        }
    }

    private FirebaseFirestore db;

    private FirestoreDBManager()
    {
        db = FirebaseFirestore.DefaultInstance;
    }

    // 문서 저장
    public async Task<bool> TrySetDocumentAsync<T>(string collection, string document, T data)
    {
        try
        {
            DocumentReference docRef = db.Collection(collection).Document(document);
            await docRef.SetAsync(data);

            Debug.Log($"[Firestore] 문서 저장 완료: {collection}/{document}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Firestore] 문서 저장 실패: {collection}/{document}, 오류: {ex.Message}");
            return false;
        }
    }

    // 필드 업데이트
    public async Task<bool> TryUpdateFieldAsync(string collection, string document, string fieldPath, object value)
    {
        try
        {
            DocumentReference docRef = db.Collection(collection).Document(document);
            await docRef.UpdateAsync(fieldPath, value);

            Debug.Log($"[Firestore] 필드 업데이트 완료: {collection}/{document} → {fieldPath}");
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Firestore] 필드 업데이트 실패: {collection}/{document} → {fieldPath}, 오류: {ex.Message}");
            return false;
        }
    }

    // 문서 가져오기
    public async Task<T> GetDocumentAsync<T>(string collection, string document)
    {
        DocumentReference docRef = db.Collection(collection).Document(document);
        DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

        if (snapshot.Exists)
        {
            return snapshot.ConvertTo<T>();
        }
        else
        {
            Debug.LogWarning($"[Firestore] 문서 없음: {collection}/{document}");
            return default;
        }
    }

    // 컬렉션 가져오기
    public async Task<T[]> GetCollectionAsync<T>(string collection)
    {
        CollectionReference colRef = db.Collection(collection);
        QuerySnapshot snapshot = await colRef.GetSnapshotAsync();

        List<T> resultList = new List<T>();
        foreach (DocumentSnapshot document in snapshot.Documents)
        {
            if (!document.Exists) continue;

            T data = document.ConvertTo<T>();
            resultList.Add(data);
        }
        Debug.Log($"[Firestore] 컬렉션 가져오기 완료: {collection}, 문서 수: {resultList.Count}");
        return resultList.ToArray();
    }

    // 문서 존재 여부 확인
    public async Task<bool> DocumentExistsAsync(string collection, string document)
    {
        try
        {
            DocumentReference docRef = db.Collection(collection).Document(document);
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            bool exists = snapshot.Exists;
            Debug.Log($"[Firestore] 문서 존재 여부: {collection}/{document} - {exists}");
            return snapshot.Exists;
        }
        catch (Exception ex)
        {
            Debug.LogError($"[Firestore] 문서 존재 여부 확인 실패: {collection}/{document}, 오류: {ex.Message}");
            return false;
        }
    }
}
