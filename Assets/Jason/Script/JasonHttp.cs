using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net.Http;
using System.Net.Http.Headers;
using UnityEngine.UI;
using System.Threading.Tasks;
using System.IO;
using System;

public class JasonHttp : MonoBehaviour
{/*
    Request 是指使用者透過瀏覽器或程式發送的 HTTP 請求 ，一般來說分成 GET 和 POST 兩種方法（差別在於帶資料的方式）。GET 的資料是包含在網址當中，POST 的資料是包含在封包之內的。
    Response 是網頁伺服器收到 Request 後，回傳給使用者的 HTTP Response，通常會有兩種形式。一種是僅有特定資料格式所組成的字串，稱為是 API；另一種是包含 HTML 的原始碼，稱為 HTML Response。 
    https://ithelp.ithome.com.tw/articles/10212102
    https://ithelp.ithome.com.tw/articles/10195016
    HTTP Header (傳送內容以外的資訊)
    當發送 GET/POST 請求後，除了資料內容之外，還需附上表頭來紀錄各種資料/設定
    一定放在內容前面
    內容格式 (圖檔、文字)
    內容大小 可用來推估下載要耗多久時間

    -------------------
    HTTP Body (資料內容)
    存放內容: HTML,JS,圖檔...等
    內容不一定是HTML，但HTML一定是內容之一
    --------------------------------------------
    Status Code
    狀態碼：是網頁伺服器用一個數字來表示瀏覽器提出的請求最後有沒有完成，如果沒有，為什麼沒有。其實提款機也有類似的東西，叫做訊息代碼。
    資訊回應 (Informational responses, 100–199),
    成功回應 (Successful responses, 200–299),
    重定向 (Redirects, 300–399),
    用戶端錯誤 (Client errors, 400–499),
    伺服器端錯誤 (Server errors, 500–599).
  
  */
    // Start is called before the first frame update
    public RawImage image;
     void Start()
    {
        //GetRequest("http://localhost:3000/PlayerData");
        // PostRequest("http://localhost:3000/PlayerData");
        //TestHeader();
        //DownLoadFloader(@"D:\texttest\HttpDownLoad\test_250m.zip", "http://http.speed.hinet.net/test_250m.zip");
        //Action<string> GetJsonAction=(Data)=>{ Debug.Log($"Json Get : {Data}"); };
        //GetJsonAsync("http://localhost:3000/PlayerData", GetJsonAction);// 可在start 前 加上 async 這樣的話往下的程式碼將等待此執行完才繼續
       // DownLoadPng(@"D:\texttest\HttpDownLoad\flower.jpg", "https://photos.mandarinoriental.com/is/image/MandarinOriental/hong-kong-flower-shop-lifestyle?wid=2880&hei=1200&fmt=jpeg&qlt=75,0&op_sharpen=0&resMode=sharp2&op_usm=0,0,0,0&iccEmbed=0&printRes=72&fit=wrap", image);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    async static void GetRequest(string url) {
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);//response提供了查看 headers  reasonpharse  statuscode(狀態碼是用以表示網頁伺服器超文字傳輸協定回應狀態的3位數字代碼,狀態碼的第一個數字代表了回應的五種狀態之一。)
        Debug.Log((int)response.StatusCode);//回傳 ok ,Not Found(enum ),轉型後回傳狀態碼
        HttpContent content = response.Content;//放入conect (內容)
        
        HttpContentHeaders headers =content.Headers;
        //Debug.Log(headers);
        

        if (response.IsSuccessStatusCode)
        {   //取得指示 HTTP 回應是否成功的值。
            //string GetJson = await content.ReadAsStringAsync();
            //string mycontent = await response.Content.ReadAsStringAsync();
            Debug.Log(await response.Content.ReadAsStringAsync());//只能拿到最新的資料但拿到的是json 物件  
            //Debug.Log(mycontent);
           
        }
        ///Debug.Log(content);
        client.Dispose();//每次用完釋放資源
        content.Dispose();
        response.Dispose();

    }

    async static void PostRequest(string url)
    {
        Dictionary<string, string> m_value = new Dictionary<string, string>();
        m_value.Add("Jason", "Hello World");
        HttpContent post = new FormUrlEncodedContent(m_value);//多部分內容

        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.PostAsync(url, post);//接收結果
        HttpContent contentGet = response.Content;
        if (response.IsSuccessStatusCode)
        {   //取得指示 HTTP 回應是否成功的值。
         //string mycontent = await response.Content.ReadAsStringAsync();
            Debug.Log(await response.Content.ReadAsStringAsync());//只能拿到最新的資料但拿到的是json 物件  
            //Debug.Log(mycontent);

        }
        client.Dispose();//每次用完關閉
        post.Dispose();
        response.Dispose();

    }

    async static void TestHeader() {
        HttpClient client_ = new HttpClient();
        client_.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/png"));//寫
        client_.BaseAddress = new System.Uri("http://localhost:3000/PlayerData");
        HttpRequestHeaders headers = client_.DefaultRequestHeaders;
        HttpResponseMessage response = await client_.GetAsync(client_.BaseAddress);
        //HttpContent content = response.Content;
        
         
        Debug.Log(headers.Accept);
         
        //Debug.Log(await client_.GetStringAsync(client_.BaseAddress));
        client_.Dispose();//每次用完關閉 
        response.Dispose();
    }

    #region 載入圖片
    async void LoadPng(string uri, RawImage image) {
        using (HttpClient client = new HttpClient()) {
            HttpResponseMessage response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                byte[] GetByte = await response.Content.ReadAsByteArrayAsync();//把檔案轉成 byte[]
                Debug.Log(GetByte.Length);
                Texture2D texture = new Texture2D(2, 2);
                texture.LoadImage(GetByte);
                image.texture = texture;
                response.Dispose();
            }
        } 
    }
    #endregion
    /// <summary>
    /// 下載文件至 本機端(path , url)
    /// </summary>
    /// <param name="file_name"></param>
    /// <param name="url"></param>
    async void DownLoadFloader(string file_name, string url)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                byte[] GetByte = await response.Content.ReadAsByteArrayAsync();//把檔案轉成 byte[]
                FileStream fs = new FileStream(file_name, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                await bw.BaseStream.WriteAsync(GetByte, 0, GetByte.Length);//等待寫完 
                //bw.Write(GetByte);//不等待寫完 
                response.Dispose();
            }
        }
    }

    /// <summary>
    /// 用call back 的方式接json
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callBack"></param>
    /// <returns></returns>
    public static async Task GetJsonAsync(string url, System.Action<string> callBack) {
        using (HttpClient clinet = new HttpClient()) {
            HttpResponseMessage response = await clinet.GetAsync(url);
            string getJson= await response.Content.ReadAsStringAsync();
            callBack(getJson);
        }
    
    }


    async void DownLoadPng(string file_name, string url, RawImage image)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                byte[] GetByte = await response.Content.ReadAsByteArrayAsync();//把檔案轉成 byte[]
                FileStream fs = new FileStream(file_name, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                await bw.BaseStream.WriteAsync(GetByte,0, GetByte.Length);//等待寫完 不然讀取會讀不到
                bw.Close();
                response.Dispose();
            }
        }
         
        // show png
        string path = file_name;
        if (File.Exists(path))//判斷有無此檔
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long numBytes = new FileInfo(path).Length; //取得長度
            byte[] GetByte = br.ReadBytes((int)numBytes);
            Debug.Log(GetByte.Length);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(GetByte);
            image.texture = texture;
        }
    }
}
