using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }
    }

    public Player player;

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 60;
    }
}
