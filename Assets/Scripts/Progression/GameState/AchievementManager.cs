using UnityEngine;
using UnityEngine.SceneManagement;
public class AchievementManager : MonoBehaviour,IDataPersistence
{
    public AchievementObject[] Achievements;

    private void OnEnable()
    {
        
        AchievementObject.OnAchievementAccomplished += TryUnlockAllAchievements;
        SceneManager.activeSceneChanged += OnSceneLoaded;
        PersistenceDataManager.OnRequestAchievementObjectStates += GetAchievementObjectStates;
    }
    private void OnDisable()
    {
        ResetAllAchievements();
        AchievementObject.OnAchievementAccomplished -= TryUnlockAllAchievements;
        SceneManager.activeSceneChanged -= OnSceneLoaded;
        PersistenceDataManager.OnRequestAchievementObjectStates -= GetAchievementObjectStates;

    }
    private void Awake()
    {
       
    }
    private void Start()
    {
      

    }
    private AchievementObject.AchievementStates[] GetAchievementObjectStates() 
    {
        AchievementObject.AchievementStates[] newState = new AchievementObject.AchievementStates[Achievements.Length];
        for (int i = 0; i < Achievements.Length; i++) 
        {
            newState[i] = new AchievementObject.AchievementStates();
            newState[i] = Achievements[i].RequiredAchievementsToUnlock.Length == 0 ?
                AchievementObject.AchievementStates.Unlocked :
                AchievementObject.AchievementStates.Locked;
        }
        return newState;
    }

    public void SaveData(ref GameData data) 
    {
        for (int i = 0; i < Achievements.Length; i++)
        {
            data.AchievementStates[i]= Achievements[i].State;
        }
    }

    public void LoadData(GameData data) 
    {
        for (int i = 0; i < Achievements.Length; i++)
        {
            Achievements[i].State = data.AchievementStates[i];
        }
    }
    private void OnSceneLoaded(Scene s1, Scene s2) 
    {
        TryUnlockAllAchievements(null);
    }

    private void TryUnlockAllAchievements(AchievementObject target) 
    {
        foreach (AchievementObject a in Achievements) 
            a.TryUnlock();
    }

    private void ResetAllAchievements() 
    {

        foreach (AchievementObject a in Achievements) 
        {
            a.State = a.RequiredAchievementsToUnlock.Length == 0?
                AchievementObject.AchievementStates.Unlocked :
                AchievementObject.AchievementStates.Locked;
        }
            
    }

}
