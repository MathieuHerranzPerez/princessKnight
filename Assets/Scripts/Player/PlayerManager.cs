
public class PlayerManager : ResetableManager
{
    public override void ResetScene()
    {
        Destroy(gameObject);
    }
}
