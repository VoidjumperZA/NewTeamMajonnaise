﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    private bool _hidden = false;
    // Three
    /*private Image[] ThreeFills { get { return GameManager.Levelmanager._baseUI.ThreeFills; } set { GameManager.Levelmanager._baseUI.ThreeFills = value; } }
    private Image[] ThreeIcons { get { return GameManager.Levelmanager._baseUI.ThreeIcons; } set { GameManager.Levelmanager._baseUI.ThreeIcons = value; } }
    // Four
    private Image[] FourFills { get { return GameManager.Levelmanager._baseUI.FourFills; } set { GameManager.Levelmanager._baseUI.FourFills = value; } }
    private Image[] FourIcons { get { return GameManager.Levelmanager._baseUI.FourIcons; } set { GameManager.Levelmanager._baseUI.FourIcons = value; } }
    // Five
    private Image[] FiveFills { get { return GameManager.Levelmanager._baseUI.FiveFills; } set { GameManager.Levelmanager._baseUI.FiveFills = value; } }
    private Image[] FiveIcons { get { return GameManager.Levelmanager._baseUI.FiveIcons; } set { GameManager.Levelmanager._baseUI.FiveIcons = value; } }*/
    // Icon sprites
    [SerializeField] private Sprite[] _beforeCaught;
    [SerializeField] private Sprite[] _afterCaught;
    private int _comboSize = 0;
    private List<int> _typeSet = new List<int>();
    private int _current = 0;

    public void Initialize()
    {
        GameManager.Levelmanager._baseUI.ComboUI.SetActive(true);
        CreateNewCombo();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.L)) CreateNewCombo();
    }
    public void Collect(int pFishType)
    {
        if (_current < _comboSize && pFishType == _typeSet[_current])
        {
            if (_comboSize == 3)
            {
                // Fills
                GameManager.Levelmanager._baseUI.ThreeFills[_current].enabled = true;
                if (_current > 0) GameManager.Levelmanager._baseUI.ThreeFills[_current - 1].enabled = false;
                // Icons
                GameManager.Levelmanager._baseUI.ThreeIcons[_current].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _afterCaught[pFishType].rect.width);
                GameManager.Levelmanager._baseUI.ThreeIcons[_current].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _afterCaught[pFishType].rect.height);
                GameManager.Levelmanager._baseUI.ThreeIcons[_current].sprite = _afterCaught[pFishType];
            }
            if (_comboSize == 4)
            {
                // Fills
                GameManager.Levelmanager._baseUI.FourFills[_current].enabled = true;
                if (_current > 0) GameManager.Levelmanager._baseUI.FourFills[_current - 1].enabled = false;
                // Icons
                GameManager.Levelmanager._baseUI.ThreeIcons[_current].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _afterCaught[pFishType].rect.width);
                GameManager.Levelmanager._baseUI.ThreeIcons[_current].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _afterCaught[pFishType].rect.height);
                GameManager.Levelmanager._baseUI.FourIcons[_current].sprite = _afterCaught[pFishType];
            }
            if (_comboSize == 5)
            {
                // Fills
                GameManager.Levelmanager._baseUI.FiveFills[_current].enabled = true;
                if (_current > 0) GameManager.Levelmanager._baseUI.FiveFills[_current - 1].enabled = false;
                // Icons
                GameManager.Levelmanager._baseUI.ThreeIcons[_current].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _afterCaught[pFishType].rect.width);
                GameManager.Levelmanager._baseUI.ThreeIcons[_current].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _afterCaught[pFishType].rect.height);
                GameManager.Levelmanager._baseUI.FiveIcons[_current].sprite = _afterCaught[pFishType];
            }

            _current += 1;
        }
        else CreateNewCombo();
        if (_current == _comboSize)
        {
            GameManager.Scorehandler.AddComboScore();
            StartCoroutine(CreateNewComboCoroutine(1));
        }
    }
    private IEnumerator CreateNewComboCoroutine(float pTime)
    {
        yield return new WaitForSeconds(pTime);
        CreateNewCombo();
    }
    public void CreateNewCombo()
    {
        _current = 0;
        HideFillsAndIcons();
        _comboSize = Random.Range(3, 6);
        _typeSet.Clear();
        for (int i = 0; i < _comboSize; i++)
        {
            _typeSet.Add(Random.Range(0, 3));
        }

        if (_comboSize == 3)
        {
            for (int i = 0; i < _comboSize; i++)
            {
                GameManager.Levelmanager._baseUI.ThreeIcons[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _beforeCaught[_typeSet[i]].rect.width);
                GameManager.Levelmanager._baseUI.ThreeIcons[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _beforeCaught[_typeSet[i]].rect.height);
                GameManager.Levelmanager._baseUI.ThreeIcons[i].sprite = _beforeCaught[_typeSet[i]];
                GameManager.Levelmanager._baseUI.ThreeIcons[i].enabled = true;
            }
        }
        if (_comboSize == 4)
        {
            for (int i = 0; i < _comboSize; i++)
            {
                GameManager.Levelmanager._baseUI.FourIcons[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _beforeCaught[_typeSet[i]].rect.width);
                GameManager.Levelmanager._baseUI.FourIcons[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _beforeCaught[_typeSet[i]].rect.height);
                GameManager.Levelmanager._baseUI.FourIcons[i].sprite = _beforeCaught[_typeSet[i]];
                GameManager.Levelmanager._baseUI.FourIcons[i].enabled = true;
            }
        }
        if (_comboSize == 5)
        {
            for (int i = 0; i < _comboSize; i++)
            {
                GameManager.Levelmanager._baseUI.FiveIcons[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _beforeCaught[_typeSet[i]].rect.width);
                GameManager.Levelmanager._baseUI.FiveIcons[i].rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _beforeCaught[_typeSet[i]].rect.height);
                GameManager.Levelmanager._baseUI.FiveIcons[i].sprite = _beforeCaught[_typeSet[i]];
                GameManager.Levelmanager._baseUI.FiveIcons[i].enabled = true;
            }
        }
        Debug.Log(_comboSize + " Combosize " + _typeSet.Count);
    }
    
    private void HideFillsAndIcons()
    {
        // Fills
        foreach (Image img in GameManager.Levelmanager._baseUI.ThreeFills) img.enabled = false;
        foreach (Image img in GameManager.Levelmanager._baseUI.FourFills) img.enabled = false;
        foreach (Image img in GameManager.Levelmanager._baseUI.FiveFills) img.enabled = false;
        // Icons
        foreach (Image img in GameManager.Levelmanager._baseUI.ThreeIcons)
        {
            img.preserveAspect = true;
            img.enabled = false;
        }
        foreach (Image img in GameManager.Levelmanager._baseUI.FourIcons)
        {
            img.preserveAspect = true;
            img.enabled = false;
        }
        foreach (Image img in GameManager.Levelmanager._baseUI.FiveIcons)
        {
            img.preserveAspect = true;
            img.enabled = false;
        }
    }
    /*public void CheckComboProgress(fish.FishType pFishType)
    {
        //Debug.Log("Checking Combo Progress. Combo Index: " + comboIndex);
        if (animatingComboBreak == false && pFishType == combo[comboIndex])
        {
            //Debug.Log("Combo type was correct! Hit fish was " + pFishType + " and required fish was " + combo[comboIndex]);
            iconBackgroundsList[comboIndex].sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Completed];
            comboIndex++;
            if (comboIndex == (comboLength + 1))
            {
                //Debug.Log("Combo Completed! ComboIndex: " + comboIndex + " and combo length: " + comboLength);
                GameObject.Find("Manager").GetComponent<ScoreHandler>().AddComboScore();
                CreateNewCombo();
                return;
            }
            iconBackgroundsList[comboIndex].sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Next];
        }
        else
        {
            //Debug.Log("Combo type was incorrect! Hit fish was " + pFishType + " and required fish was " + combo[comboIndex]);
            animatingComboBreak = true;
            for (int i = 0; i < iconBackgroundsList.Count; i++)
            {
                iconSlideDistancesList.Add(Random.Range(-iconSlideDistance, iconSlideDistance));
                //Debug.Log("Distance: " + iconSlideDistancesList[i]);
                iconSlideCounterList.Add(0.0f);
                int iconIndex = -1;
                if (i < comboIndex)
                {
                    iconIndex = (int)IconBackgroundStates.Completed;
                }
                else if (i == comboIndex)
                {
                    iconIndex = (int)IconBackgroundStates.Next;
                }
                else if (i > comboIndex)
                {
                    iconIndex = (int)IconBackgroundStates.Standard;
                }
                iconBackgroundsList[i].sprite = comboBrokenIconSprites[iconIndex];
            }
        }
    }

    public void ClearPreviousCombo(bool pCanGenerateNewCombo)
    {
        comboCanBeGenerated = pCanGenerateNewCombo;
        comboIndex = 0;
        for (int i = 0; i < iconBackgroundsList.Count; i++)
        {
            Destroy(iconBackgroundsList[i].gameObject);
            Destroy(iconFishList[i].gameObject);
        }
        combo.Clear();
        iconBackgroundsList.Clear();
        iconFishList.Clear();
        iconSlideDistancesList.Clear();
        iconSlideCounterList.Clear();
    }

    public void CreateNewCombo()
    {
        //Debug.Log("Combo Size: " + combo.Count);
        ClearPreviousCombo(true);

        if (comboCanBeGenerated == true)
        {
            comboLength = Random.Range(minComboSize, maxComboSize);
            //Debug.Log("New Combo [" + (comboLength + 1) + "]: ");
            int numberOfFish = System.Enum.GetNames(typeof(fish.FishType)).Length;
           // Debug.Log("There are " + numberOfFish + " types.");
            for (int i = comboLength; i > -1; i--)
            {
                int fishTypeIndex = Random.Range(0, numberOfFish);
                combo.Add((fish.FishType)fishTypeIndex);
                //Debug.Log("-> (" + fishTypeIndex + ")" + combo[combo.Count - 1].ToString());
                Image newComboIconBackground = GameObject.Instantiate(comboBackgroundImageToInstantiate, canvas.transform);
                Image newComboIconFish = GameObject.Instantiate(comboFishImageToInstantiate, canvas.transform);
                newComboIconBackground.transform.localScale = new Vector3(iconScalingSize, iconScalingSize, iconScalingSize);
                newComboIconFish.transform.localScale = new Vector3(iconScalingSize, iconScalingSize, iconScalingSize);
                newComboIconFish.sprite = comboFishIconSprites[fishTypeIndex];

                if (i == comboLength)
                {
                    newComboIconBackground.sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Next];
                }
                else
                {
                    newComboIconBackground.sprite = comboBackgroundIconSprites[(int)IconBackgroundStates.Standard];
                }

                Vector3 iconPosition = iconSpawnPosition.transform.position;
                iconPosition.x -= (i * widthBetweenIcons);
                newComboIconBackground.transform.position = iconPosition;
                newComboIconFish.transform.position = iconPosition;
                iconBackgroundsList.Add(newComboIconBackground);
                iconFishList.Add(newComboIconFish);
            }
            for (int i = 0; i < comboLength + 1; i++)
            {
                //Debug.Log("- " + combo[i].ToString());
            }
        }       
    }*/
}
