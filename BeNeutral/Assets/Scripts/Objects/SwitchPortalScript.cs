using System;
using System.Collections;
using TMPro;
using UI;
using UnityEngine;

public class SwitchPortalScript : MonoBehaviour
{
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    [SerializeField] private TextMeshProUGUI timerText;

    private bool canSwitch = true;
    private Renderer objectRenderer;
    private float disableDuration = 10f;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        if (!canSwitch)
        {
            disableDuration -= Time.deltaTime;
            timerText.text = Mathf.Ceil(disableDuration).ToString();
        }
        else
        {
            timerText.text = "";
        }
    }

    public void SwitchPlayer()
    {
        AudioManager.instance.PlaySwitchPlayer();
        Vector2 support = Player1.transform.position;
        Player1.transform.position = Player2.transform.position;
        Player2.transform.position = support;

        PlayerMovement pl1Movement = Player1.GetComponent<PlayerMovement>();
        pl1Movement.gravityDirection = pl1Movement.gravityDirection * -1;
        PlayerMovement pl2Movement = Player2.GetComponent<PlayerMovement>();
        pl2Movement.gravityDirection = pl2Movement.gravityDirection * -1;

        canSwitch = false;
        disableDuration = 10f; // Reset the timer when switching
        StartCoroutine(EnableSwitchingAfterDelay());
        StartCoroutine(MakeObjectTransparent(0.5f));
    }

    private IEnumerator EnableSwitchingAfterDelay()
    {
        yield return new WaitForSeconds(10f);
        canSwitch = true;
        StartCoroutine(MakeObjectTransparent(1.0f));
    }

    private IEnumerator MakeObjectTransparent(float targetAlpha)
    {
        int direction = objectRenderer.material.color.a < targetAlpha ? +1 : -1;

        while (Math.Abs(objectRenderer.material.color.a - targetAlpha) > 0.01)
        {
            Color currentColor = objectRenderer.material.color;
            currentColor.a += direction * Time.deltaTime / 2; // Adjust the speed of transparency change

            objectRenderer.material.color = currentColor;
            yield return null;
        }

        // Ensure the target alpha is set
        Color finalColor = objectRenderer.material.color;
        finalColor.a = targetAlpha;
        objectRenderer.material.color = finalColor;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (canSwitch)
        {
            SwitchPlayer();
        }
    }
}
