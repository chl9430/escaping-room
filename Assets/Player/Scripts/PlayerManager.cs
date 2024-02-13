using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    StarterAssetsInputs input;

    [Header("Aim")]
    [SerializeField] CinemachineVirtualCamera aimCam;
    [SerializeField] GameObject aimImage;

    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        AimCheck();
    }

    void AimCheck()
    {
        if (input.aim)
        {
            aimCam.gameObject.SetActive(true);
            aimImage.SetActive(true);
        }
        else
        {
            aimCam.gameObject.SetActive(false);
            aimImage.SetActive(false);
        }
    }
}
