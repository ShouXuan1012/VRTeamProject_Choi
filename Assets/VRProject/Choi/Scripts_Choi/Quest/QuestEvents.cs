using System;

public static class QuestEvents
{
    public static event Action OnBusBoarded;
    public static event Action OnFoodPurchased;
    public static event Action OnPhotoTaken;
    public static event Action OnMuseumEntered;
    public static event Action OnBuskingDonated;
    public static event Action OnBasketballScored10;

    public static void BusBoarded() => OnBusBoarded?.Invoke();
    public static void FoodPurchased() => OnFoodPurchased?.Invoke();
    public static void PhotoTaken() => OnPhotoTaken?.Invoke();
    public static void MuseumEntered() => OnMuseumEntered?.Invoke();
    public static void BuskingDonated() => OnBuskingDonated?.Invoke();
    public static void BasketballScored10() => OnBasketballScored10?.Invoke();
}
