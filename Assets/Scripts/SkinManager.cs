using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SkinManager : MonoBehaviour
{
    public SpriteRenderer sr;
    public SpriteRenderer sm;
    public List<Sprite> skins = new List<Sprite>();
    public List<Sprite> skinname = new List<Sprite>();
    private int selectedSkin = 0;
    private int selectedSkinName = 0;
    public GameObject playerskin;
    public GameObject playerskinName;

    public void NextOption()
    {
	selectedSkin = selectedSkin+1;
        selectedSkinName = selectedSkinName+1;
	if (selectedSkin == skins.Count)
	{
		selectedSkin = 0;
		selectedSkinName = 0;
	}
	sr.sprite = skins[selectedSkin];
	sm.sprite = skinname[selectedSkinName];
    }
    public void BackOption()
    {
	selectedSkin = selectedSkin-1;
	selectedSkinName = selectedSkinName-1;
	if (selectedSkin < 0)
	{
		selectedSkin = skins.Count -1;
		selectedSkinName = skins.Count -1;
	}
	sr.sprite = skins[selectedSkin];
	sm.sprite = skinname[selectedSkinName];
    }
    public void saveAndLoad() {
	PrefabUtility.SaveAsPrefabAsset(playerskin, "Assets/selectedskin.prefab");
    }
}
