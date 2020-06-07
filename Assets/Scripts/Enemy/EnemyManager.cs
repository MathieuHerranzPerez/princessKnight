using UnityEngine;

public class EnemyManager : ResetableManager
{
    public static EnemyManager Instance;

    public Transform EnemyContainer { get { return enemyContainer; } }

    [SerializeField] private Transform enemyContainer = default;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
    }

    public override void ResetScene()
    {
        foreach(Transform child in EnemyContainer)
        {
            Destroy(child.gameObject);
        }
    }
}
