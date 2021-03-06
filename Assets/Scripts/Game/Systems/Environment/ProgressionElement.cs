using System;
using System.Collections;
using System.Collections.Generic;
using Game.Systems.Environment;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProgressionElement : MonoBehaviour
{
    public enum ProgressionStage
    {
        NONE, STUMP, BURNT, BURNT_STUMP, DEFAULT, FLORISH
    }
    
    public GameObject StumpModel;
    public GameObject BurntStump;
    public GameObject BurntModel;
    public GameObject DefaultModel;
    public GameObject FlorishModel;


    public float ScaleVariance;

    private void Start()
    {
        DynamicForest.instance.elements.Add(this);
        Random.InitState(this.transform.position.GetHashCode());
        
        var scale = this.transform.localScale;
        scale = scale.normalized * (scale.magnitude + (scale.magnitude * ((Random.value * ScaleVariance) - ScaleVariance / 2)));
        var eulerAngles = this.transform.eulerAngles;
        eulerAngles.y = Random.value * 360;
        this.transform.eulerAngles = eulerAngles;
    }

    private void OnDestroy()
    {
        DynamicForest.instance.elements.Remove(this);
    }

    public void SetProgressionStage(ProgressionStage stage)
    {
        StumpModel.SetActive(false);
        BurntModel.SetActive(false);
        DefaultModel.SetActive(false);
        FlorishModel.SetActive(false);
        try
        {

            switch (stage)
            {
                case ProgressionStage.NONE:
                    break;
                case ProgressionStage.STUMP:
                    StumpModel.SetActive(true);
                    break;
                case ProgressionStage.BURNT_STUMP:
                    BurntStump.SetActive(true);
                    break;
                case ProgressionStage.BURNT:
                    BurntModel.SetActive(true);
                    break;
                case ProgressionStage.DEFAULT:
                    DefaultModel.SetActive(true);
                    break;
                case ProgressionStage.FLORISH:
                    FlorishModel.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Could not set progression stage for " + name + " error - " + e.Message);
        }
    }
    
}
