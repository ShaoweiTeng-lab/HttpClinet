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
     *  ---------------�w��:-------------------------------
     * �O�o����Nods.js
     *  Json server : cmd ->�� npm install -g json-server
     * ----------------���찲���-----------------------------
     * 
     �}��cmd-> �� cd �M��Njasonfake��Ƨ� ��J ��enter-> �K�W json-server --watch db.json  ��enter->��  http://localhost:3000/posts/1 �Y�i�ݨ�ۤv��json
     
     */
    //�إ� HttpClient
    public static HttpClient client = new HttpClient();
    [SerializeField] public PlayerData m_PlayerData = new PlayerData();
     

    //�ϥ� async ��k�q���� url �W���o�^��
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
        string result = await client.GetStringAsync("http://localhost:3000/PlayerData");//���ݦ^��
        Debug.Log(result);
        #region �� JsonUtility ���� ��class������ ���� {} ���^�Ǭ��}�C[  �]����{}   ]
        /*
         �^�Ǯ榡��  ����List []  �ҥH�n���X���Y�Ĥ@��
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
        //m_PlayerData = RootObject_All.Data[0];//�Ĥ@�����
        #endregion
        #region �� SimpleJson + JsonUtility ���� ��class������ ���� {} ���^�Ǭ��}�C[  �]����{}   ]
        //JSONArray jsonGet = (JSONArray)JSON.Parse(result);//����J JsonArry
        //Debug.Log(jsonGet[0]);
        //m_PlayerData = JsonUtility.FromJson<PlayerData>(jsonGet[0].ToString());//���X�Ĥ@�Ӫ��� �� class 
        #endregion
        client.Dispose();//�����s��
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
