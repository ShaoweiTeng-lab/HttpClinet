using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using System.Threading.Tasks;
using SimpleJSON;
using System.IO;
 
using System;
 

public class NetManager : MonoBehaviour
{   /*
     *  ---------------安裝:-------------------------------
     * 記得先裝Nods.js
     *  Json server : cmd ->打 npm install -g json-server
     * ----------------拿到假資料-----------------------------
     * 
     開啟cmd-> 打 cd 然後將jasonfake資料夾 放入 按enter-> 貼上 json-server --watch db.json  按enter->到  http://localhost:3000/posts/1 即可看到自己的json
     
     */
    //建立 HttpClient
    public static HttpClient client = new HttpClient();
    [SerializeField] public PlayerData m_PlayerData = new PlayerData();
     

    //使用 async 方法從網路 url 上取得回應
    // Start is called before the first frame update
    void Start()
    {

        FuncGetRequest();
        //Post();
    }

    // Update is called once per frame
    void Update()
    {

    }

    async void FuncGetRequest()
    {
        await GetRequest();
    }
    async Task GetRequest()
    {
        string result = await client.GetStringAsync("http://localhost:3000/PlayerData");//等待回傳
        Debug.Log(result);
        #region 用 JsonUtility 取值 轉class必須為 物件 {} 但回傳為陣列[  包物件{}   ]
        /*
         回傳格式為  此為List []  所以要取出裏頭第一值
                [
                    {
                      "Name": "Jason",
                      "Coint": 100,
                      "Hp": 150,
                      "id": 1
                    }
               ]
         */
        // RootObject<List<PlayerData>> RootObject_All = JsonUtility.FromJson<RootObject<List<PlayerData>>>("{\"Data\":" + result+ "}");
        //m_PlayerData = RootObject_All.Data[0];//第一筆資料
        #endregion
        #region 用 SimpleJson + JsonUtility 取值 轉class必須為 物件 {} 但回傳為陣列[  包物件{}   ]
        //JSONArray jsonGet = (JSONArray)JSON.Parse(result);//先放入 JsonArry
        //Debug.Log(jsonGet[0]);
        //m_PlayerData = JsonUtility.FromJson<PlayerData>(jsonGet[0].ToString());//取出第一個物件 轉 class 
        #endregion
        client.Dispose();//關閉連結
    }

    public static async Task Post()
    {
        StringContent content = new StringContent($"Hello World");
        await client.PostAsync("http://localhost:3000/PlayerData", content); 
    }


   
}
[System.Serializable]
public class RootObject<T>
{
    public T Data;
}
[System.Serializable]
public class PlayerData
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public int Coint;
    [SerializeField]
    public int Hp;
    [SerializeField]
    public int id;

}
