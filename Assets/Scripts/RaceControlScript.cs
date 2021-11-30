using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class RaceControlScript : MonoBehaviour {

    //public variables
    public Camera camHuman; //Camera for the human stage
    public Camera camUndead; //Camera for the undead stage
    public Camera camOrc; //Camera for the orc stage 

    public Canvas humanCanvas; //Canvas containing human options
    public Text mHumanLabel; //Label for facial hair
    public Text fHumanLabel; //label for features
    public Canvas undeadCanvas; //Canvas containing undead options
    public Text mUndeadLabel; //Label for facial hair
    public Text fUndeadLabel; //label for features
    public Canvas orcCanvas; //Canvas containing orc options
    public Text mOrcLabel; //Label for facial hair
    public Text fOrcLabel; //label for features

    // Layout: <Race>_<Gender>_<Part><Partnumber> i.e H_F_Head1 = Human Female Head 1
    //Human customisations
    public Transform humanFemale;
    public Transform humanMale;
    public Material[] humanMaterials;

    //Undead customisations
    public Transform undeadFemale;
    public Transform undeadMale;
    public Material[] undeadMaterials;

    //Orc customisations
    public Transform orcFemale;
    public Transform orcMale;
    public Material[] orcMaterials;

    //Private Variables
    private string[] races = new string[3] { "Human", "Undead", "Orc" }; //Race array containing strings for the four races.
    private string[] genders = new string[2] { "Male", "Female" };
    private string selectedRace; //Currently selected race
    private string selectedGender; //currently selected gender
    private int selectedLowerBodyChoice; //currently selected lower body option
    private int selectedLowerTex; //currently selected lower body texture
    private int selectedUpperBodyChoice; //currently selected upper body option
    private int selectedUpperTex; //currently selected upper body texture
    private int selectedHeadChoice; //currently selected head option
    private int selectedHeadTex; //currently selected head texture
    private int selectedHairFeatureChoice; //currently selected hair or feature
    private int selectedHairFeatureTex; //currently selected hair / feature texture

    //maximum numbers for selection choices, constant variables - cannot be changed.
    private const int lowerBodyMax = 2;
    private const int upperBodyMax = 2;
    private const int headMax = 2;
    private const int hairFeatureMax = 2;

    private const int choiceMinemum = 1; //minimum value the customisation options can have.

    // Use this for initialization
    void Start()
    {
        //Causes every part of the customisation options to be randomised when the game is loaded. This ensures that something will always be loaded.
        RandomiseSelection();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncremenetLowerBody() //increments lower body selection
    {
        selectedLowerBodyChoice++;
        if (selectedLowerBodyChoice > lowerBodyMax)
        {
            selectedLowerBodyChoice = choiceMinemum;
        }
        Debug.Log("AFTER CLICK: " + selectedLowerBodyChoice);
        SwapMesh("LowerBody");
    }

    public void DecrementLowerBody() //decrements lower body selection
    {
        selectedLowerBodyChoice--;
        Debug.Log("BEFORE CLICK: " + selectedLowerBodyChoice);
        if (selectedLowerBodyChoice < choiceMinemum)
        {
            selectedLowerBodyChoice = lowerBodyMax;
        }
        SwapMesh("LowerBody");
        Debug.Log("AFTER CLICK: " + selectedLowerBodyChoice);
    }

    public void IncremenetUpperBody() //increments Upper body selection
    {
        selectedUpperBodyChoice++;
        if (selectedUpperBodyChoice > upperBodyMax)
        {
            selectedUpperBodyChoice = choiceMinemum;
        }
        SwapMesh("UpperBody");
    }

    public void DecrementUpperBody() //decrements Upper body selection
    {
        selectedUpperBodyChoice--;
        if (selectedUpperBodyChoice < choiceMinemum)
        {
            selectedUpperBodyChoice = upperBodyMax;
        }
        SwapMesh("UpperBody");
    }

    public void IncremenetHead() //increments lower body selection
    {
        selectedHeadChoice++;
        if (selectedHeadChoice > headMax)
        {
            selectedHeadChoice = choiceMinemum;
        }
        SwapMesh("Head");
    }
    public void DecrementHead() //decrements lower body selection
    {
        selectedHeadChoice--;
        if (selectedHeadChoice < choiceMinemum)
        {
            selectedHeadChoice = headMax;
        }
        SwapMesh("Head");
    }

    public void IncremenetHairFeature() //increments lower body selection
    {
        selectedHairFeatureChoice++;
        if (selectedHairFeatureChoice > hairFeatureMax)
        {
            selectedHairFeatureChoice = choiceMinemum;
        }
        SwapMesh("HairFeature");
    }
    public void DecrementHairFeature() //decrements lower body selection
    {
        selectedHairFeatureChoice--;
        if (selectedHairFeatureChoice < choiceMinemum)
        {
            selectedHairFeatureChoice = hairFeatureMax;
        }
        SwapMesh("HairFeature");
    }

    public void IncrementLowerTex()
    {
        selectedLowerTex++;
        if (selectedLowerTex > lowerBodyMax)
        {
            selectedLowerTex  = choiceMinemum;
        }
        SwapTex("LowerBody");
    }
    public void IncrementUpperTex()
    {
        selectedUpperTex++;
        if (selectedUpperTex > upperBodyMax)
        {
            selectedUpperTex = choiceMinemum;
        }
        SwapTex("UpperBody");
    }
    public void IncrementHeadTex()
    {
        selectedHeadTex++;
        if (selectedHeadTex > headMax)
        {
            selectedHeadTex = choiceMinemum;
        }
        SwapTex("Head");
    }
    public void IncrementHairFeatureTex()
    {
        selectedHairFeatureTex++;
        if (selectedHairFeatureTex > hairFeatureMax)
        {
            selectedHairFeatureTex = choiceMinemum;
        }
        SwapTex("HairFeature");
    }

    public void SetGender(string clickedGender)
    {
        selectedGender = clickedGender;
        SwapMesh("LowerBody");
        SwapMesh("UpperBody");
        SwapMesh("Head");
        SwapMesh("HairFeature");
        SwapMesh("Race");
    }

    public void SetRace(string clickedRace) //Called when race changing buttons are pressed, sets the selected race to the chosen race and calls the method that deals with panels and cameras
    {
        selectedRace = clickedRace;
        UICameraSwapper();
    }

    private void SwapTex(string section) //Method to swap the materials of the body parts depending on the currently selected texture. Changes every mesh that fits that section of the body to ensure changes happen throughout
    {
        switch (section)
        {
            case "LowerBody":
                switch (selectedLowerTex)
                {
                    case 1:
                        humanMale.GetChild(4).GetComponent<Renderer>().material = humanMaterials[24];
                        humanMale.GetChild(5).GetComponent<Renderer>().material = humanMaterials[26];
                        undeadMale.GetChild(3).GetComponent<Renderer>().material = undeadMaterials[22];
                        undeadMale.GetChild(4).GetComponent<Renderer>().material = undeadMaterials[24];
                        orcMale.GetChild(3).GetComponent<Renderer>().material = orcMaterials[22];
                        orcMale.GetChild(4).GetComponent<Renderer>().material = orcMaterials[24];
                        humanFemale.GetChild(4).GetComponent<Renderer>().material = humanMaterials[8];
                        humanFemale.GetChild(5).GetComponent<Renderer>().material = humanMaterials[10];
                        undeadFemale.GetChild(4).GetComponent<Renderer>().material = undeadMaterials[8];
                        undeadFemale.GetChild(5).GetComponent<Renderer>().material = undeadMaterials[10];
                        orcFemale.GetChild(4).GetComponent<Renderer>().material = orcMaterials[8];
                        orcFemale.GetChild(5).GetComponent<Renderer>().material = orcMaterials[10];
                        break;
                    case 2:
                        humanMale.GetChild(4).GetComponent<Renderer>().material = humanMaterials[25];
                        humanMale.GetChild(5).GetComponent<Renderer>().material = humanMaterials[27];
                        undeadMale.GetChild(3).GetComponent<Renderer>().material = undeadMaterials[23];
                        undeadMale.GetChild(4).GetComponent<Renderer>().material = undeadMaterials[24];
                        orcMale.GetChild(3).GetComponent<Renderer>().material = orcMaterials[23];
                        orcMale.GetChild(4).GetComponent<Renderer>().material = orcMaterials[25];
                        humanFemale.GetChild(4).GetComponent<Renderer>().material = humanMaterials[9];
                        humanFemale.GetChild(5).GetComponent<Renderer>().material = humanMaterials[11];
                        undeadFemale.GetChild(4).GetComponent<Renderer>().material = undeadMaterials[9];
                        undeadFemale.GetChild(5).GetComponent<Renderer>().material = undeadMaterials[11];
                        orcFemale.GetChild(4).GetComponent<Renderer>().material = orcMaterials[9];
                        orcFemale.GetChild(5).GetComponent<Renderer>().material = orcMaterials[11];
                        break;
                }
                break;
            case "UpperBody":
                switch (selectedUpperTex)
                {
                    case 1:
                        humanMale.GetChild(6).GetComponent<Renderer>().material = humanMaterials[28];
                        humanMale.GetChild(7).GetComponent<Renderer>().material = humanMaterials[30];
                        undeadMale.GetChild(5).GetComponent<Renderer>().material = undeadMaterials[26];
                        undeadMale.GetChild(6).GetComponent<Renderer>().material = undeadMaterials[28];
                        orcMale.GetChild(5).GetComponent<Renderer>().material = orcMaterials[26];
                        orcMale.GetChild(6).GetComponent<Renderer>().material = orcMaterials[28];
                        humanFemale.GetChild(6).GetComponent<Renderer>().material = humanMaterials[12];
                        humanFemale.GetChild(7).GetComponent<Renderer>().material = humanMaterials[14];
                        undeadFemale.GetChild(6).GetComponent<Renderer>().material = undeadMaterials[12];
                        undeadFemale.GetChild(7).GetComponent<Renderer>().material = undeadMaterials[14];
                        orcFemale.GetChild(6).GetComponent<Renderer>().material = orcMaterials[12];
                        orcFemale.GetChild(7).GetComponent<Renderer>().material = orcMaterials[14];
                        break;
                    case 2:
                        humanMale.GetChild(6).GetComponent<Renderer>().material = humanMaterials[29];
                        humanMale.GetChild(7).GetComponent<Renderer>().material = humanMaterials[31];
                        undeadMale.GetChild(5).GetComponent<Renderer>().material = undeadMaterials[27];
                        undeadMale.GetChild(6).GetComponent<Renderer>().material = undeadMaterials[29];
                        orcMale.GetChild(5).GetComponent<Renderer>().material = orcMaterials[27];
                        orcMale.GetChild(6).GetComponent<Renderer>().material = orcMaterials[29];
                        humanFemale.GetChild(6).GetComponent<Renderer>().material = humanMaterials[13];
                        humanFemale.GetChild(7).GetComponent<Renderer>().material = humanMaterials[15];
                        undeadFemale.GetChild(6).GetComponent<Renderer>().material = undeadMaterials[13];
                        undeadFemale.GetChild(7).GetComponent<Renderer>().material = undeadMaterials[15];
                        orcFemale.GetChild(6).GetComponent<Renderer>().material = orcMaterials[13];
                        orcFemale.GetChild(7).GetComponent<Renderer>().material = orcMaterials[15];
                        break;
                }
                break;
            case "Head":
                switch (selectedHeadTex)
                {
                    case 1:
                        humanMale.GetChild(2).GetComponent<Renderer>().material = humanMaterials[20];
                        humanMale.GetChild(3).GetComponent<Renderer>().material = humanMaterials[22];
                        undeadMale.GetChild(1).GetComponent<Renderer>().material = undeadMaterials[18];
                        undeadMale.GetChild(2).GetComponent<Renderer>().material = undeadMaterials[20];
                        orcMale.GetChild(1).GetComponent<Renderer>().material = orcMaterials[18];
                        orcMale.GetChild(2).GetComponent<Renderer>().material = orcMaterials[20];
                        humanFemale.GetChild(2).GetComponent<Renderer>().material = humanMaterials[4];
                        humanFemale.GetChild(3).GetComponent<Renderer>().material = humanMaterials[6];
                        undeadFemale.GetChild(2).GetComponent<Renderer>().material = undeadMaterials[4];
                        undeadFemale.GetChild(3).GetComponent<Renderer>().material = undeadMaterials[6];
                        orcFemale.GetChild(2).GetComponent<Renderer>().material = orcMaterials[4];
                        orcFemale.GetChild(3).GetComponent<Renderer>().material = orcMaterials[6];
                        break;
                    case 2:
                        humanMale.GetChild(2).GetComponent<Renderer>().material = humanMaterials[21];
                        humanMale.GetChild(3).GetComponent<Renderer>().material = humanMaterials[23];
                        undeadMale.GetChild(1).GetComponent<Renderer>().material = undeadMaterials[19];
                        undeadMale.GetChild(2).GetComponent<Renderer>().material = undeadMaterials[21];
                        orcMale.GetChild(1).GetComponent<Renderer>().material = orcMaterials[19];
                        orcMale.GetChild(2).GetComponent<Renderer>().material = orcMaterials[21];
                        humanFemale.GetChild(2).GetComponent<Renderer>().material = humanMaterials[5];
                        humanFemale.GetChild(3).GetComponent<Renderer>().material = humanMaterials[7];
                        undeadFemale.GetChild(2).GetComponent<Renderer>().material = undeadMaterials[5];
                        undeadFemale.GetChild(3).GetComponent<Renderer>().material = undeadMaterials[7];
                        orcFemale.GetChild(2).GetComponent<Renderer>().material = orcMaterials[5];
                        orcFemale.GetChild(3).GetComponent<Renderer>().material = orcMaterials[7];
                        break;
                }
                break;
            case "HairFeature":
                switch (selectedHairFeatureTex)
                {
                    case 1:
                        humanMale.GetChild(0).GetComponent<Renderer>().material = humanMaterials[16];
                        humanMale.GetChild(1).GetComponent<Renderer>().material = humanMaterials[18];
                        undeadMale.GetChild(0).GetComponent<Renderer>().material = undeadMaterials[16];
                        orcMale.GetChild(0).GetComponent<Renderer>().material = orcMaterials[16];
                        humanFemale.GetChild(0).GetComponent<Renderer>().material = humanMaterials[0];
                        humanFemale.GetChild(1).GetComponent<Renderer>().material = humanMaterials[2];
                        undeadFemale.GetChild(0).GetComponent<Renderer>().material = undeadMaterials[0];
                        undeadFemale.GetChild(1).GetComponent<Renderer>().material = undeadMaterials[2];
                        orcFemale.GetChild(0).GetComponent<Renderer>().material = orcMaterials[0];
                        orcFemale.GetChild(1).GetComponent<Renderer>().material = orcMaterials[2];
                        break;
                    case 2:
                        humanMale.GetChild(0).GetComponent<Renderer>().material = humanMaterials[17];
                        humanMale.GetChild(1).GetComponent<Renderer>().material = humanMaterials[19];
                        undeadMale.GetChild(0).GetComponent<Renderer>().material = undeadMaterials[17];
                        orcMale.GetChild(0).GetComponent<Renderer>().material = orcMaterials[17];
                        humanFemale.GetChild(0).GetComponent<Renderer>().material = humanMaterials[1];
                        humanFemale.GetChild(1).GetComponent<Renderer>().material = humanMaterials[3];
                        undeadFemale.GetChild(0).GetComponent<Renderer>().material = undeadMaterials[1];
                        undeadFemale.GetChild(1).GetComponent<Renderer>().material = undeadMaterials[3];
                        orcFemale.GetChild(0).GetComponent<Renderer>().material = orcMaterials[1];
                        orcFemale.GetChild(1).GetComponent<Renderer>().material = orcMaterials[3];
                        break;
                }
                break;
        }
    }
    private void SwapMesh(string section) //Method used to swap out the visibility of the meshes in the scene
    {
        switch (section)
        {
            case "LowerBody":
                switch (selectedLowerBodyChoice)
                {
                    case 1:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);

                                //Enables all male lower body options of the first option
                                humanMale.GetChild(4).gameObject.SetActive(true);
                                humanMale.GetChild(5).gameObject.SetActive(false);
                                undeadMale.GetChild(3).gameObject.SetActive(true);
                                undeadMale.GetChild(4).gameObject.SetActive(false);
                                orcMale.GetChild(3).gameObject.SetActive(true);
                                orcMale.GetChild(4).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(4).gameObject.SetActive(true);
                                humanFemale.GetChild(5).gameObject.SetActive(false);
                                undeadFemale.GetChild(4).gameObject.SetActive(true);
                                undeadFemale.GetChild(5).gameObject.SetActive(false);
                                orcFemale.GetChild(4).gameObject.SetActive(true);
                                orcFemale.GetChild(5).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                    case 2:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);
                                //Enables all male lower body options of the first option
                                humanMale.GetChild(4).gameObject.SetActive(false);
                                humanMale.GetChild(5).gameObject.SetActive(true);
                                undeadMale.GetChild(3).gameObject.SetActive(false);
                                undeadMale.GetChild(4).gameObject.SetActive(true);
                                orcMale.GetChild(3).gameObject.SetActive(false);
                                orcMale.GetChild(4).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(4).gameObject.SetActive(false);
                                humanFemale.GetChild(5).gameObject.SetActive(true);
                                undeadFemale.GetChild(4).gameObject.SetActive(false);
                                undeadFemale.GetChild(5).gameObject.SetActive(true);
                                orcFemale.GetChild(4).gameObject.SetActive(false);
                                orcFemale.GetChild(5).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                }
                break;
            case "UpperBody":
                switch (selectedUpperBodyChoice)
                {
                    case 1:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);
                                //Enables all male lower body options of the first option
                                humanMale.GetChild(6).gameObject.SetActive(true);
                                humanMale.GetChild(7).gameObject.SetActive(false);
                                undeadMale.GetChild(5).gameObject.SetActive(true);
                                undeadMale.GetChild(6).gameObject.SetActive(false);
                                orcMale.GetChild(5).gameObject.SetActive(true);
                                orcMale.GetChild(6).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(6).gameObject.SetActive(true);
                                humanFemale.GetChild(7).gameObject.SetActive(false);
                                undeadFemale.GetChild(6).gameObject.SetActive(true);
                                undeadFemale.GetChild(7).gameObject.SetActive(false);
                                orcFemale.GetChild(6).gameObject.SetActive(true);
                                orcFemale.GetChild(7).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                    case 2:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);
                                //Enables all male lower body options of the first option
                                humanMale.GetChild(6).gameObject.SetActive(false);
                                humanMale.GetChild(7).gameObject.SetActive(true);
                                undeadMale.GetChild(5).gameObject.SetActive(false);
                                undeadMale.GetChild(6).gameObject.SetActive(true);
                                orcMale.GetChild(5).gameObject.SetActive(false);
                                orcMale.GetChild(6).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(6).gameObject.SetActive(false);
                                humanFemale.GetChild(7).gameObject.SetActive(true);
                                undeadFemale.GetChild(6).gameObject.SetActive(false);
                                undeadFemale.GetChild(7).gameObject.SetActive(true);
                                orcFemale.GetChild(6).gameObject.SetActive(false);
                                orcFemale.GetChild(7).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                }
                break;
            case "Head":
                switch (selectedHeadChoice)
                {
                    case 1:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);
                                //Enables all male lower body options of the first option
                                humanMale.GetChild(2).gameObject.SetActive(true);
                                humanMale.GetChild(3).gameObject.SetActive(false);
                                undeadMale.GetChild(1).gameObject.SetActive(true);
                                undeadMale.GetChild(2).gameObject.SetActive(false);
                                orcMale.GetChild(1).gameObject.SetActive(true);
                                orcMale.GetChild(2).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(2).gameObject.SetActive(true);
                                humanFemale.GetChild(3).gameObject.SetActive(false);
                                undeadFemale.GetChild(2).gameObject.SetActive(true);
                                undeadFemale.GetChild(3).gameObject.SetActive(false);
                                orcFemale.GetChild(2).gameObject.SetActive(true);
                                orcFemale.GetChild(3).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                    case 2:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);
                                //Enables all male lower body options of the first option
                                humanMale.GetChild(2).gameObject.SetActive(false);
                                humanMale.GetChild(3).gameObject.SetActive(true);
                                undeadMale.GetChild(1).gameObject.SetActive(false);
                                undeadMale.GetChild(2).gameObject.SetActive(true);
                                orcMale.GetChild(1).gameObject.SetActive(false);
                                orcMale.GetChild(2).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(2).gameObject.SetActive(false);
                                humanFemale.GetChild(3).gameObject.SetActive(true);
                                undeadFemale.GetChild(2).gameObject.SetActive(false);
                                undeadFemale.GetChild(3).gameObject.SetActive(true);
                                orcFemale.GetChild(2).gameObject.SetActive(false);
                                orcFemale.GetChild(3).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                }
                break;
            case "HairFeature":
                switch (selectedHairFeatureChoice)
                {
                    case 1:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);
                                //Enables all male lower body options of the first option
                                humanMale.GetChild(0).gameObject.SetActive(true);
                                humanMale.GetChild(1).gameObject.SetActive(false);
                                undeadMale.GetChild(0).gameObject.SetActive(true);
                                orcMale.GetChild(0).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(0).gameObject.SetActive(true);
                                humanFemale.GetChild(1).gameObject.SetActive(false);
                                undeadFemale.GetChild(0).gameObject.SetActive(true);
                                undeadFemale.GetChild(1).gameObject.SetActive(false);
                                orcFemale.GetChild(0).gameObject.SetActive(true);
                                orcFemale.GetChild(1).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                    case 2:
                        switch (selectedGender)
                        {
                            case "Male":
                                //Ensures male model is enabled
                                humanMale.gameObject.SetActive(true);
                                undeadMale.gameObject.SetActive(true);
                                orcMale.gameObject.SetActive(true);
                                //Enables all male lower body options of the first option
                                humanMale.GetChild(0).gameObject.SetActive(false);
                                humanMale.GetChild(1).gameObject.SetActive(true);
                                undeadMale.GetChild(0).gameObject.SetActive(false);
                                orcMale.GetChild(0).gameObject.SetActive(false);

                                //ensures the female models are disabled
                                humanFemale.gameObject.SetActive(false);
                                undeadFemale.gameObject.SetActive(false);
                                orcFemale.gameObject.SetActive(false);
                                break;
                            case "Female":
                                //Ensures female model is enabled
                                humanFemale.gameObject.SetActive(true);
                                undeadFemale.gameObject.SetActive(true);
                                orcFemale.gameObject.SetActive(true);
                                //Enables all female lower body options of the first option
                                humanFemale.GetChild(0).gameObject.SetActive(false);
                                humanFemale.GetChild(1).gameObject.SetActive(true);
                                undeadFemale.GetChild(0).gameObject.SetActive(false);
                                undeadFemale.GetChild(1).gameObject.SetActive(true);
                                orcFemale.GetChild(0).gameObject.SetActive(false);
                                orcFemale.GetChild(1).gameObject.SetActive(true);

                                //ensures the female models are disabled
                                humanMale.gameObject.SetActive(false);
                                undeadMale.gameObject.SetActive(false);
                                orcMale.gameObject.SetActive(false);
                                break;
                        }
                        break;
                }
                break;
            case "race":
                switch (selectedRace)
                {
                    case "Human":
                        break;
                    case "Undead":
                        break;
                    case "Orc":
                        break;
                }
                break;
        }
    }

    private void UICameraSwapper() //Deals with enabling and disabling the cameras and UI panels
    {
        if (selectedGender == "Male")
        {
            mHumanLabel.enabled = true;
            mUndeadLabel.enabled = true;
            mOrcLabel.enabled = true;

            fHumanLabel.enabled = false;
            fOrcLabel.enabled = false;
            fUndeadLabel.enabled = false;
        }
        else
        {
            mHumanLabel.enabled = false;
            mUndeadLabel.enabled = false;
            mOrcLabel.enabled = false;

            fHumanLabel.enabled = true;
            fOrcLabel.enabled = true;
            fUndeadLabel.enabled = true;
        }
        //Checks the clicked race and enables and disables cameras and canvases depending on corresponding races in race array
        if (selectedRace == races[0]) //if human
        {
            camHuman.enabled = true;
            camUndead.enabled = false;
            camOrc.enabled = false;

            humanCanvas.enabled = true;
            undeadCanvas.enabled = false;
            orcCanvas.enabled = false;
        }
        else if (selectedRace == races[1]) //if undead
        {
            camHuman.enabled = false;
            camUndead.enabled = true;
            camOrc.enabled = false;

            humanCanvas.enabled = false;
            undeadCanvas.enabled = true;
            orcCanvas.enabled = false;
        }
        else if (selectedRace == races[2]) //if orc
        {
            camHuman.enabled = false;
            camUndead.enabled = false;
            camOrc.enabled = true;

            humanCanvas.enabled = false;
            undeadCanvas.enabled = false;
            orcCanvas.enabled = true;
        }
        else if (selectedRace == races[3]) //if lignuren
        {
            camHuman.enabled = false;
            camUndead.enabled = false;
            camOrc.enabled = false;

            humanCanvas.enabled = false;
            undeadCanvas.enabled = false;
            orcCanvas.enabled = false;
        }
    }

    public void RandomiseSelection()
    {
        selectedRace = races[UnityEngine.Random.Range(0, 3)]; //causes a random race to be chosen to be shown when the application is opened
        selectedGender = genders[UnityEngine.Random.Range(0, 2)]; //Causes a random gender to be chosen when the application is opened
        selectedLowerBodyChoice = UnityEngine.Random.Range(1, 3);//Causes a random lower body to be chosen
        selectedLowerTex = UnityEngine.Random.Range(1, 3); //causes random lower texture to be chosen
        selectedUpperBodyChoice = UnityEngine.Random.Range(1, 3); //Causes a random upper body to be chosen
        selectedUpperTex = UnityEngine.Random.Range(1, 3); //causes a random upper body texture to be chosen
        selectedHeadChoice = UnityEngine.Random.Range(1, 3); //Causes a random head to be chosen
        selectedHeadTex = UnityEngine.Random.Range(1, 3); //causes a random head texture to be chosen
        selectedHairFeatureChoice = UnityEngine.Random.Range(1, 3); //causes a random facial hair / feature to be chosen
        selectedHairFeatureTex = UnityEngine.Random.Range(1, 3); //causes a random facial hair / feature texture to be chosen

        SwapMesh("LowerBody");
        SwapMesh("UpperBody");
        SwapMesh("Head");
        SwapMesh("HairFeature");
        SwapMesh("Gender");
        SwapMesh("Race");
        SwapTex("LowerBody");
        SwapTex("UpperBody");
        SwapTex("Head");
        SwapTex("HairFeature");
        SwapTex("Gender");
        SwapTex("Race");
        //Calls the camera and UI swapping method after the choices have been randomised.
        UICameraSwapper();
    }

    public void SaveCustom()//Saves the currently selected customisations
    {
        BinaryFormatter bf = new BinaryFormatter(); //creates a new binary formatter instance that allows for data to be serialised and saved into a file
        string path = EditorUtility.SaveFilePanel("Save character as DAT",
                "",
                "SavedCharacter.dat",
                "dat"); //Sets the path string to the folder in which the user selects, as well as appends it with the name of the file to be saved.
        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.OpenOrCreate); //Opens the filestream with the previously selected path with the mode set as opening or creating, allowing for the filestream to save the file.

            SaveData data = new SaveData(); //creates a new instance of the SaveData class to be serialised. Saved the attributes of the clas to that of the currently selected choices within the tool.
            data.race = selectedRace;
            data.gender = selectedGender;
            data.lowerBodyChoice = selectedLowerBodyChoice;
            data.LowerTexChoice = selectedLowerTex;
            data.UpperBodyChoice = selectedUpperBodyChoice;
            data.UpperTexChoice = selectedUpperTex;
            data.headChoice = selectedHeadChoice;
            data.HeadTexChoice = selectedHeadTex;
            data.hairFeatureChoice = selectedHairFeatureChoice;
            data.HairFeatureTexChoice = selectedHairFeatureTex;


            bf.Serialize(file, data); //Serialises the data and saves it to the file
            file.Close(); //Closes the filestream.
        }
        else
        {
            EditorUtility.DisplayDialog("Saving Custom Data", "You must select save if you want custom data saved!", "OK");
        }
    }

    public void LoadCustom()//Loads customisations from a previously saved file.
    {
        BinaryFormatter bf = new BinaryFormatter(); //creates a new binary formatter instance that allows for the saved data to be deserialised and used in a way which the application can understand.
        string path = EditorUtility.OpenFilePanel("Select custom data", "", "dat");
        if (File.Exists(path))
        {
            FileStream file = File.Open(path, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file); //Deserialising the file, turning the binary into the actual information

            //Sets the select race, gender and further customisation options to be those that were held within the save file.
            selectedRace = data.race;
            selectedGender = data.gender;
            selectedLowerBodyChoice = data.lowerBodyChoice;
            selectedLowerTex = data.LowerTexChoice;
            selectedUpperBodyChoice = data.UpperBodyChoice;
            selectedUpperTex = data.UpperTexChoice;
            selectedHeadChoice = data.headChoice;
            selectedHeadTex = data.HeadTexChoice;
            selectedHairFeatureChoice = data.hairFeatureChoice;
            selectedHairFeatureTex = data.HairFeatureTexChoice;

            SwapMesh("LowerBody");
            SwapMesh("UpperBody");
            SwapMesh("Head");
            SwapMesh("HairFeature");
            SwapMesh("Gender");
            SwapMesh("Race");
            SwapTex("LowerBody");
            SwapTex("UpperBody");
            SwapTex("Head");
            SwapTex("HairFeature");
            SwapTex("Gender");
            SwapTex("Race");

            UICameraSwapper(); //Sends the currently selected race to the camera and UI swapping method that deals with cameras and canvases

            file.Close(); //Closes the file stream
        }
        else
        {
            EditorUtility.DisplayDialog("Select Custom Data", "you must select a valid file if you choose to load data!", "OK");
        }
    }


    [Serializable]
    class SaveData
    {
        public string race;
        public string gender;
        public int lowerBodyChoice;
        public int LowerTexChoice;
        public int UpperBodyChoice;
        public int UpperTexChoice;
        public int headChoice;
        public int HeadTexChoice;
        public int hairFeatureChoice;
        public int HairFeatureTexChoice;
    }
}
