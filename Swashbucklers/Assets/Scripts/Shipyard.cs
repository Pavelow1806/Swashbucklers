using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shipyard : MonoBehaviour
{
    public GameObject Sails;
    private Text SailLevel;
    private Image SailProgress;
    private Text SailCost;
    public GameObject CBDamage;
    private Text CBDamageLevel;
    private Image CBDamageProgress;
    private Text CBDamageCost;
    public GameObject Hull;
    private Text HullLevel;
    private Image HullProgress;
    private Text HullCost;
    public GameObject Storage;
    private Text StorageLevel;
    private Image StorageProgress;
    private Text StorageCost;
    public GameObject Quality;
    private Text QualityLevel;
    private Image QualityProgress;
    private Text QualityCost;
    public AudioSource UpgradeStuff;
    public AudioSource Click;
    public Sprite GreenCircle;
    public Sprite GoldCircle;
    // Start is called before the first frame update
    void Start()
    {
        SailLevel = Sails.transform.Find("Level").GetComponent<Text>();
        SailProgress = Sails.transform.Find("Upgrade").Find("Progress").GetComponent<Image>();
        SailCost = Sails.transform.Find("Cost").Find("Cost").GetComponent<Text>();
        CBDamageLevel = CBDamage.transform.Find("Level").GetComponent<Text>();
        CBDamageProgress = CBDamage.transform.Find("Upgrade").Find("Progress").GetComponent<Image>();
        CBDamageCost = CBDamage.transform.Find("Cost").Find("Cost").GetComponent<Text>();
        HullLevel = Hull.transform.Find("Level").GetComponent<Text>();
        HullProgress = Hull.transform.Find("Upgrade").Find("Progress").GetComponent<Image>();
        HullCost = Hull.transform.Find("Cost").Find("Cost").GetComponent<Text>();
        StorageLevel = Storage.transform.Find("Level").GetComponent<Text>();
        StorageProgress = Storage.transform.Find("Upgrade").Find("Progress").GetComponent<Image>();
        StorageCost = Storage.transform.Find("Cost").Find("Cost").GetComponent<Text>();
        QualityLevel = Quality.transform.Find("Level").GetComponent<Text>();
        QualityProgress = Quality.transform.Find("Upgrade").Find("Progress").GetComponent<Image>();
        QualityCost = Quality.transform.Find("Cost").Find("Cost").GetComponent<Text>();

        //Display the current upgrade level for each category
        SailLevel.text = PlayerPrefs.GetInt("Sails").ToString() + " / 10";
        CBDamageLevel.text = PlayerPrefs.GetInt("Damage").ToString() + " / 10";
        HullLevel.text = PlayerPrefs.GetInt("Hull").ToString() + " / 10";
        StorageLevel.text = PlayerPrefs.GetInt("Storage").ToString() + " / 10";
        QualityLevel.text = PlayerPrefs.GetInt("Quality").ToString() + " / 10";

        SailProgress.fillAmount = (float)PlayerPrefs.GetInt("Sails") / 10f;
        CBDamageProgress.fillAmount = (float)PlayerPrefs.GetInt("Damage") / 10f;
        HullProgress.fillAmount = (float)PlayerPrefs.GetInt("Hull") / 10f;
        StorageProgress.fillAmount = (float)PlayerPrefs.GetInt("Storage") / 10f;
        QualityProgress.fillAmount = (float)PlayerPrefs.GetInt("Quality") / 10f;

        //Display cost of upgrade for each upgrade category
        SailCost.text = PlayerPrefs.GetInt("Sails") < 10 ? (PlayerPrefs.GetInt("Sails") * 5).ToString() : "Max";
        CBDamageCost.text = PlayerPrefs.GetInt("Damage") < 10 ? (PlayerPrefs.GetInt("Damage") * 6).ToString() : "Max";
        HullCost.text = PlayerPrefs.GetInt("Hull") < 10 ? (PlayerPrefs.GetInt("Hull") * 7).ToString() : "Max";
        StorageCost.text = PlayerPrefs.GetInt("Storage") < 10 ? (PlayerPrefs.GetInt("Storage") * 8).ToString() : "Max";
        QualityCost.text = PlayerPrefs.GetInt("Quality") < 10 ? (PlayerPrefs.GetInt("Quality") * 9).ToString() : "Max";
    }
    public void ResetPoints()
    {
        // TODO
    }
    public void UpgradeSails()
    {
        Click.Play();
        if ((PlayerPrefs.GetInt("Gold") >= (PlayerPrefs.GetInt("Sails") * 5)) && (PlayerPrefs.GetInt("Sails") < 10))
        {
            UpgradeStuff.Play();
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - (PlayerPrefs.GetInt("Sails") * 5));
            PlayerPrefs.SetInt("Sails", PlayerPrefs.GetInt("Sails") + 1);
        }
        if (PlayerPrefs.GetInt("Sails") == 10)
            SailProgress.sprite = GoldCircle;
        else
            SailProgress.sprite = GreenCircle;
        PlayerPrefs.SetFloat("Turn", ((PlayerPrefs.GetInt("Sails") / 10) + 1));
        SailLevel.text = PlayerPrefs.GetInt("Sails").ToString() + " / 10";
        SailProgress.fillAmount = (float)PlayerPrefs.GetInt("Sails") / 10f;
        SailCost.text = PlayerPrefs.GetInt("Sails") < 10 ? (PlayerPrefs.GetInt("Sails") * 5).ToString() : "Max";
    }
    public void UpgradeDamage()
    {
        Click.Play();
        if ((PlayerPrefs.GetInt("Gold") >= (PlayerPrefs.GetInt("Damage") * 6)) && (PlayerPrefs.GetInt("Damage") < 10))
        {
            UpgradeStuff.Play();
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - (PlayerPrefs.GetInt("Damage") * 6));
            PlayerPrefs.SetInt("Damage", PlayerPrefs.GetInt("Damage") + 1);
        }
        if (PlayerPrefs.GetInt("Damage") == 10)
            CBDamageProgress.sprite = GoldCircle;
        else
            CBDamageProgress.sprite = GreenCircle;
        PlayerPrefs.SetFloat("fDamage", (PlayerPrefs.GetInt("Damage") * 5) + 15);
        CBDamageLevel.text = PlayerPrefs.GetInt("Damage").ToString() + " / 10";
        CBDamageProgress.fillAmount = (float)PlayerPrefs.GetInt("Damage") / 10f;
        CBDamageCost.text = PlayerPrefs.GetInt("Damage") < 10 ? (PlayerPrefs.GetInt("Damage") * 6).ToString() : "Max";
    }
    public void UpgradeHull()
    {
        Click.Play();
        if ((PlayerPrefs.GetInt("Gold") >= (PlayerPrefs.GetInt("Hull") * 7)) && (PlayerPrefs.GetInt("Hull") < 10))
        {
            UpgradeStuff.Play();
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - (PlayerPrefs.GetInt("Hull") * 7));
            PlayerPrefs.SetInt("Hull", PlayerPrefs.GetInt("Hull") + 1);
        }
        if (PlayerPrefs.GetInt("Hull") == 10)
            HullProgress.sprite = GoldCircle;
        else
            HullProgress.sprite = GreenCircle;
        PlayerPrefs.SetFloat("MHHP", ((PlayerPrefs.GetInt("Hull") - 1) * 10) + (PlayerPrefs.GetInt("Hull") <= 1 ? 50 : 60));
        PlayerPrefs.SetFloat("HHP", PlayerPrefs.GetFloat("MHHP"));
        HullLevel.text = PlayerPrefs.GetInt("Hull").ToString() + " / 10";
        HullProgress.fillAmount = (float)PlayerPrefs.GetInt("Hull") / 10f;
        HullCost.text = PlayerPrefs.GetInt("Hull") < 10 ? (PlayerPrefs.GetInt("Hull") * 7).ToString() : "Max";
    }
    public void UpgradeStorage()
    {
        Click.Play();
        if ((PlayerPrefs.GetInt("Gold") >= (PlayerPrefs.GetInt("Storage") * 8)) && (PlayerPrefs.GetInt("Storage") < 10))
        {
            UpgradeStuff.Play();
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - (PlayerPrefs.GetInt("Storage") * 8));
            PlayerPrefs.SetInt("Storage", PlayerPrefs.GetInt("Storage") + 1);
        }
        if (PlayerPrefs.GetInt("Storage") == 10)
            StorageProgress.sprite = GoldCircle;
        else
            StorageProgress.sprite = GreenCircle;
        PlayerPrefs.SetInt("Cargo", (PlayerPrefs.GetInt("Storage") * 5) + 5);
        StorageLevel.text = PlayerPrefs.GetInt("Storage").ToString() + " / 10";
        StorageProgress.fillAmount = (float)PlayerPrefs.GetInt("Storage") / 10f;
        StorageCost.text = PlayerPrefs.GetInt("Storage") < 10 ? (PlayerPrefs.GetInt("Storage") * 8).ToString() : "Max";
    }
    public void UpgradeQuality()
    {
        Click.Play();
        if ((PlayerPrefs.GetInt("Gold") >= (PlayerPrefs.GetInt("Quality") * 9)) && (PlayerPrefs.GetInt("Quality") < 10))
        {
            UpgradeStuff.Play();
            PlayerPrefs.SetInt("Gold", PlayerPrefs.GetInt("Gold") - (PlayerPrefs.GetInt("Quality") * 9));
            PlayerPrefs.SetInt("Quality", PlayerPrefs.GetInt("Quality") + 1);
        }
        if (PlayerPrefs.GetInt("Quality") == 10)
            QualityProgress.sprite = GoldCircle;
        else
            QualityProgress.sprite = GreenCircle;
        PlayerPrefs.SetFloat("fQuality", 1.0f - (PlayerPrefs.GetInt("Quality") * 0.05f));
        QualityLevel.text = PlayerPrefs.GetInt("Quality").ToString() + " / 10";
        QualityProgress.fillAmount = (float)PlayerPrefs.GetInt("Quality") / 10f;
        QualityCost.text = PlayerPrefs.GetInt("Quality") < 10 ? (PlayerPrefs.GetInt("Quality") * 9).ToString() : "Max";
    }
}
