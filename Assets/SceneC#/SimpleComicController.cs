using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // 用于场景跳转

public class SimpleComicController : MonoBehaviour
{
    // 每张图的信息
    [System.Serializable]
    public class ComicImage
    {
        public RectTransform imageTransform;  // 图片的RectTransform
        public Vector2 startPosition;         // 起点位置
        public Vector2 endPosition;           // 终点位置
        public float moveDuration = 0.5f;     // 移动时间（秒）
        public bool hasMoved = false;         // 是否已经移动过
    }
    
    [Header("=== 漫画图片设置 ===")]
    public ComicImage[] comics = new ComicImage[6];  // 6张图
    
    [Header("=== 最后的大图 ===")]
    public GameObject finalImage;  // 拖入FinalImage对象
    
    [Header("=== 音效设置 ===")]
    public AudioClip clickSound;   // 点击音效
    public AudioClip finalSound;   // 显示大图的音效
    
    [Header("=== 场景设置 ===")]
    public string nextSceneName = "MainMenu";  // 下一个场景名
    
    // 私有变量
    private int currentClickCount = 0;    // 点击次数
    private AudioSource audioSource;      // 音频播放器
    private bool isMoving = false;        // 是否正在移动
    
    void Start()
    {
        // 添加音频组件
        audioSource = gameObject.AddComponent<AudioSource>();
        
        // 初始化所有图片到起点位置
        for (int i = 0; i < comics.Length; i++)
        {
            if (comics[i].imageTransform != null)
            {
                comics[i].imageTransform.anchoredPosition = comics[i].startPosition;
                comics[i].hasMoved = false;
            }
        }
        
        // 隐藏最后的大图
        if (finalImage != null)
            finalImage.SetActive(false);
    }
    
    // 播放音效
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
    
    // 点击按钮时调用
    public void OnClickScreen()
    {
        // 如果正在移动中，不响应点击
        if (isMoving) return;
        
        // 播放点击音效
        PlaySound(clickSound);
        
        currentClickCount++;
        
        // 前6次点击：移动对应的图片
        if (currentClickCount <= 6)
        {
            int comicIndex = currentClickCount - 1;  // 0-5
            StartCoroutine(MoveComic(comicIndex));
        }
        // 第7次点击：切换到最终大图
        else if (currentClickCount == 7)
        {
            ShowFinalImage();
        }
        // 第8次点击：跳转到下一个场景
        else if (currentClickCount == 8)
        {
            LoadNextScene();
        }
    }
    
    // 平滑移动图片的协程
    System.Collections.IEnumerator MoveComic(int index)
    {
        if (index >= comics.Length || comics[index].imageTransform == null)
            yield break;
        
        isMoving = true;
        
        RectTransform rt = comics[index].imageTransform;
        Vector2 startPos = rt.anchoredPosition;
        Vector2 endPos = comics[index].endPosition;
        float duration = comics[index].moveDuration;
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            // 使用平滑的插值
            rt.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            
            yield return null;  // 等待下一帧
        }
        
        // 确保最终位置准确
        rt.anchoredPosition = endPos;
        comics[index].hasMoved = true;
        
        isMoving = false;
    }
    
    // 显示最终大图
    void ShowFinalImage()
    {
        // 播放特殊音效
        PlaySound(finalSound);
        
        // 隐藏所有小图
        for (int i = 0; i < comics.Length; i++)
        {
            if (comics[i].imageTransform != null)
            {
                // 直接隐藏
                comics[i].imageTransform.gameObject.SetActive(false);
            }
        }
        
        // 显示最终大图（可以添加淡入效果）
        if (finalImage != null)
        {
            finalImage.SetActive(true);
            
            // 可选：添加淡入效果
            Image img = finalImage.GetComponent<Image>();
            if (img != null)
            {
                StartCoroutine(FadeInImage(img, 1f));
            }
        }
    }
    
    // 淡入效果（可选）
    System.Collections.IEnumerator FadeInImage(Image image, float duration)
    {
        Color color = image.color;
        color.a = 0f;
        image.color = color;
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            
            color.a = t;
            image.color = color;
            
            yield return null;
        }
        
        color.a = 1f;
        image.color = color;
    }
    
    // 加载下一个场景
    void LoadNextScene()
    {
        Debug.Log("跳转到场景: " + nextSceneName);
        
        // 方法1：直接跳转
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        // 方法2：跳转到Build Settings中的第一个场景
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}