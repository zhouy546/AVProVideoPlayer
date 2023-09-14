using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ImageBGctr : MonoBehaviour
{

    public Image m_image;

    public Dictionary<string, Sprite> udp_sprite = new Dictionary<string, Sprite>();
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventDefine.ini, SetImage);

        EventCenter.AddListener<string>(EventDefine.ShowImage, showImage);

        EventCenter.AddListener<string>(EventDefine.SwitchVideo, HideImage);

    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void showImage(string s)
    {
        m_image.sprite = udp_sprite[s];
        m_image.enabled = true;
    }

    public void HideImage(string s)
    {
        m_image.enabled = false;
    }


    private async void SetImage() {

        foreach (var item in ValueSheet.udpImageInfo_kp)
        {
           Sprite sprite =   await getTexture(item.Value.url);

            udp_sprite.Add(item.Value.udp, sprite);
        }
    }

    public async Task<Sprite> getTexture(string name)
    {
        string imageurl = Application.streamingAssetsPath + "/" + name;
        Texture2D _texture = await GetRemoteTexture(imageurl);
        Sprite sprite = Sprite.Create(_texture, new Rect(0, 0, _texture.width, _texture.height), new Vector2(0.5f, 0.5f));

        return sprite;
    }

    public static async Task<Texture2D> GetRemoteTexture(string url)
    {
        using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(url))
        {
            // begin request:
            var asyncOp = www.SendWebRequest();

            // await until it's done: 
            while (asyncOp.isDone == false)
                await Task.Delay(1000 / 30);//30 hertz

            // read results:
            if (www.isNetworkError || www.isHttpError)
            {
                // log error:
#if DEBUG
                Debug.Log($"{www.error}, URL:{www.url}");
#endif

                // nothing to return on error:
                return null;
            }
            else
            {
                // return valid results:
                return DownloadHandlerTexture.GetContent(www);
            }
        }
    }
}
