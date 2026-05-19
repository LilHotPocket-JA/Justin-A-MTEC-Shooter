using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public float gameTime = 180; 
    public string newScene;
    [SerializeField] private Slider slider;

    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.maxValue = gameTime;
    }

    // Update is called once per frame
    void Update()
    {
        GT();
    }

    void GT()
    {
        gameTime -= Time.deltaTime;
        slider.value = gameTime;

        if(0 > gameTime)
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
      
        SceneManager.LoadScene(newScene);
    }
}
