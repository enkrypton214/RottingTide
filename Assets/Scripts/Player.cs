
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;
    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;
    public bool isDead=false;

    public void Start()
    {
        playerHealthUI.text = $"Health: {HP}"; 
    }

public void TakeDamage(int damageAmount)
    {
        HP-=damageAmount;
        if (HP <= 0)
        {
            print("Player Died");
            PlayerDead();
            isDead = true;

        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloodyScreenEffect());
            playerHealthUI.text = $"Health: {HP}"; 
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        
        {
            if(isDead==false)
            {
            Debug.Log("Hand Hit Player");
            TakeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
        }
    }
    private IEnumerator BloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();
 
        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;
 
        float duration = 1.5f;
        float elapsedTime = 0f;
 
        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
 
            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;
 
            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;
 
            yield return null; ; // Wait for the next frame.
        }
        
        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }
    private void PlayerDead()
    {
        
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDied);
        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.playerDeathMusic;
        SoundManager.Instance.playerChannel.PlayDelayed(2f);    

        GetComponent<MouseMovement>().enabled=false;
        GetComponent<PlayerMovement>().enabled=false;

        //Dying animation
        GetComponentInChildren<Animator>().enabled = true;
        foreach (Transform child in transform)
{
    if (child.CompareTag("WeaponSpawn"))
    {
        child.gameObject.SetActive(false);
    }
}
        HUDManager.Instance.weaponHUD.SetActive(false);
        HUDManager.Instance.middleDot.SetActive(false);
        playerHealthUI.gameObject.SetActive(false);
        GetComponent<ScreenBlackout>().StartFade();

        StartCoroutine(ShowGameOverUI());

        
    }
    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
        int waveSurvived = GlobalRefrences.Instance.WaveNumber;
        if(waveSurvived>SaveLoadManager.Instance.LoadHighScore())
        {
        SaveLoadManager.Instance.SaveHighScore(waveSurvived-1);
        }
        StartCoroutine(ReturnToMainMenu());
    }
    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
}

