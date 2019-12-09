using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    public int BushGroupId { get { return bushGroup.Id; } }
    public bool IsContainingPlayer { get; private set; } = false;

    [HideInInspector]
    public Point pos;       // the logic position

    [Header("Setup")]
    //[SerializeField]
    //private Transform partToMove = default;
    [SerializeField]
    private Renderer rendererBush = default;

    // ---- INTERN ----
    //private Vector3 initialPos;
    private BushGroup bushGroup;
    //                  unit,        needToHide
    private Dictionary<int, HiddableUnit> dicHiddable = new Dictionary<int, HiddableUnit> ();
    private bool needToHideUnits = true;

    private Color defaultColor;

    void Start()
    {
        //initialPos = partToMove.localPosition;
        defaultColor = rendererBush.material.color;
    }

    public void SetBushGroup(BushGroup bg)
    {
        bushGroup = bg;
    }

    void LateUpdate()
    {
        List<int> keyToRemove = new List<int>();
        foreach (KeyValuePair<int, HiddableUnit> entry in dicHiddable)
        {
            if(needToHideUnits && entry.Value)
            {
                entry.Value.Hide();
            }
            else if(!entry.Value)
            {
                keyToRemove.Add(entry.Key);
            }
        }

        foreach(int key in keyToRemove)
        {
            dicHiddable.Remove(key);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        HiddableUnit unit = other.GetComponent<HiddableUnit>();
        if(unit)
        {
            unit.Hide();
            dicHiddable.Add(unit.Id, unit);
        }
        else if (other.tag == "Player")
        {
            IsContainingPlayer = true;
            PlayerVision playerVision = other.GetComponent<PlayerVision>();
            //playerVision.RevelInBushes(bushGroup);
        }
        //StartCoroutine(Shake(0.2f, 0.07f));
    }

    void OnTriggerExit(Collider other)
    {
        HiddableUnit unit = other.GetComponent<HiddableUnit>();
        if (unit)
        {
            dicHiddable.Remove(unit.Id);
        }
        else if (other.tag == "Player")
        {
            IsContainingPlayer = false;
            PlayerVision playerVision = other.GetComponent<PlayerVision>();
            //playerVision.StopRevelingInBushes();
        }
    }

    public void StopHiddingUnits()
    {
        needToHideUnits = false;
        Color newCol = defaultColor;
        newCol.a = 0.5f;
        rendererBush.material.color = newCol;
    }

    public void HideUnits()
    {
        needToHideUnits = true;
        rendererBush.material.color = defaultColor;
    }

    public bool Contains(HiddableUnit unit)
    {
        return dicHiddable.ContainsKey(unit.Id);
    }

    //private IEnumerator Shake(float duration, float translationMagnitude)
    //{
    //    float time = 0f;
    //    while (time < duration)
    //    {
    //        float x = UnityEngine.Random.Range(-1f, 1f) * translationMagnitude;
    //        float z = UnityEngine.Random.Range(-1f, 1f) * translationMagnitude;


    //        partToMove.localPosition = new Vector3(x, 0f, z) + initialPos;

    //        time += Time.deltaTime;
    //        yield return null;
    //    }

    //    partToMove.localPosition = initialPos;
    //}
}
