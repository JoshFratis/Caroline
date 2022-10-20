using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using StarterAssets;

public class Player : MonoBehaviour
{
    public Text interactableObjectText;
    public Text damageText;
    public Image fadeObject;
    public UIManager uiManager;
    public Image damageIndicator;
    private bool hitInteractable;
    private bool isDead;
    private float damageTextOpacity;
    Color damageTextColor;
    FirstPersonController controller;
    [SerializeField] PlayerMovement playerMovementScript;
    [SerializeField] Damageable damageableScript;
    [SerializeField] GameObject torch;
    [SerializeField] Torch torchScript;
    [SerializeField] float healRate;
   
    void Start()
    {
        isDead = false;
        damageTextColor = new Color(1,0,0,1);
        controller = GetComponent<FirstPersonController>();

    }

    void Update()
    {
        if (!isDead)
        {
            LookCheck();

            // heal
            damageableScript.Damage(-healRate);
        }

        if (Input.GetKeyDown("r"))
        {
            StartCoroutine(uiManager.Fade(true, true));
        }
    }

    private void LookCheck()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f)); 
        
        hitInteractable = false;

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            Interactable interactable = objectHit.gameObject.GetComponent<Interactable>();

            if (interactable != null) 
            {
                interactableObjectText.text = interactable.interactionMessage;
                
                if (hit.distance <= interactable.interactionDistance)
                {
                    name = hit.collider.gameObject.name;	
                    hitInteractable = true;

                    if (Input.GetMouseButtonDown(0))
                    {
                        interactable.OnMouseClick();
                    }
                }
            }
        }

        if (hitInteractable == false)
        {
            interactableObjectText.text = "";
        }
    }

    public void KillCharacter()
    {
        if (!isDead)
        {
            Debug.Log("player killed");
            isDead = true;
            damageText.text = "YOU DIED\nPress 'R' to Restart";
            damageTextColor.a = 1;
            damageText.color = damageTextColor;
            playerMovementScript.moveSpeed = 0f;
            StartCoroutine(uiManager.Fade(true, false));
        }
    }

    public void Damaged(float healthNew)
    {
        float damageIndicatorAlpha = (1 - healthNew) / 2;
        Color damageIndicatorColor = damageIndicator.color;
        damageIndicatorColor.a = damageIndicatorAlpha;
        damageIndicator.color = damageIndicatorColor;
    }
}
