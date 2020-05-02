using System.Collections.Generic;
using UnityEngine;

public class BushGroup : MonoBehaviour
{
    //public int Id { get { return id; } }

    // ---- INTERN ----
    //private int id;
    private Dictionary<Point, Bush> dicBush = new Dictionary<Point, Bush>();

    void Awake()
    {
        //id = (int) (transform.position.x * transform.position.y * 100);

        Vector2 refPos = new Vector2(transform.GetChild(0).position.x, transform.GetChild(0).position.z);
        foreach (Transform child in transform)
        {
            Bush bush = child.GetComponent<Bush>();

            // Init bush pos
            int diffX = (int) (child.position.x - refPos.x);
            int diffZ = (int)(child.position.z - refPos.y);
            bush.pos = new Point(diffX, diffZ);

            bush.SetBushGroup(this);
            dicBush.Add(bush.pos, bush);
        }
    }

    void Update()
    {
        foreach (KeyValuePair<Point, Bush> entry in dicBush)
        {
            entry.Value.HideUnits();
        }

        foreach (KeyValuePair<Point, Bush> entry in dicBush)
        {
            if(entry.Value.IsContainingPlayer)
            {
                foreach(Bush bush in SearchBushInRange(entry.Value, PlayerVision.InBushViewRange))
                {
                    bush.StopHiddingUnits();
                }
            }
        }
    }

    //public void TryToRevel(List<HiddableUnit> listToRevel)
    //{
    //    foreach(HiddableUnit unit in listToRevel)
    //    {
    //        foreach (KeyValuePair<Point, Bush> entry in dicBush)
    //        {
    //            if (entry.Value.Contains(unit))
    //            {
    //                entry.Value.DoNotHide(unit);
    //                break;
    //            }
    //        }
    //    }
    //}

    private List<Bush> SearchBushInRange(Bush start, int range)
    {
        List<Bush> retValue = new List<Bush>();

        int nbXIter = 0;
        int ctrlIncrNbIter = 1;
        for (int x = start.pos.x - range; x <= start.pos.x + range; ++x)
        {
            for (int z = nbXIter + start.pos.z; z >= start.pos.z - nbXIter; --z)
            {
                Bush b = GetBush(new Point(x, z));
                if (b != null)
                    retValue.Add(b);
            }
            if (nbXIter == range)
                ctrlIncrNbIter *= -1;

            nbXIter += ctrlIncrNbIter;
        }

        return retValue;
    }

    private Bush GetBush(Point p)
    {
        return dicBush.ContainsKey(p) ? dicBush[p] : null;
    }
}
