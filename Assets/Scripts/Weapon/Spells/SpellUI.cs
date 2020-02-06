using UnityEngine;
using UnityEngine.UI;

public class SpellUI : MonoBehaviour
{
    [SerializeField] private SpellUIContainer[] arraySpellUIContainer = new SpellUIContainer[2];

    // ---- INTERN ----


    public void InitWithWeaponSpell(WeaponSpell[] weaponSpells)
    {
        if (weaponSpells.Length > arraySpellUIContainer.Length)
        {
            return;
        }

        // remove the current spells
        foreach(SpellUIContainer spellCont in arraySpellUIContainer)
        {
            spellCont.Reset();
        }

        // init the UI
        for (int i = 0; i < weaponSpells.Length; ++i)
        {
            arraySpellUIContainer[i].Init(weaponSpells[i]);
        }
    }

    [System.Serializable]
    private class SpellUIContainer : Observer
    {
        public Image image = default;
        public Button button = default;
        public Image greySpellOverlayCountdown = default;

        [HideInInspector] public WeaponSpell weaponSpell;

        public void Init(WeaponSpell weaponSpell)
        {
            this.weaponSpell = weaponSpell;
            weaponSpell.Register(this);
            image.sprite = weaponSpell.Sprite;

            button.gameObject.SetActive(true);
            button.onClick.AddListener(this.weaponSpell.UseEffect);
        }

        public void Notify()
        {
            greySpellOverlayCountdown.fillAmount = weaponSpell.CurrentCouldown / weaponSpell.TotalCouldown;
        }

        public void Reset()
        {
            image.sprite = null;
            button.onClick.RemoveAllListeners();
            weaponSpell = null;
            button.gameObject.SetActive(false);
        }
    }
}
