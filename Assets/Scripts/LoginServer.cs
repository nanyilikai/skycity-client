using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KBEngine;

public class LoginServer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        KBEngine.Event.registerOut("onLoginSuccessfully", this, "onLoginSuccessfully");
        KBEngine.Event.registerOut("onLoginFailed", this, "onLoginFailed");
        KBEngine.Event.registerOut("onConnectionState", this, "onConnectionState");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Login()
    {
        KBEngine.Event.fireIn("login", "test1", "123456", System.Text.Encoding.UTF8.GetBytes("chess"));
        //KBEngine.Event.fireIn("createAccount", "test1", "123456", System.Text.Encoding.UTF8.GetBytes("chess"));
    }

    public void onLoginSuccessfully(UInt64 rndUUID, Int32 eid, Account accountEntity)
    {
        Debug.Log("login is successfully!(登陆成功!)");
    }

    public void onLoginFailed(UInt16 failedcode)
    {
        if (failedcode == 20)
        {
            Debug.Log("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode) + ", " + System.Text.Encoding.ASCII.GetString(KBEngineApp.app.serverdatas()));
        }
        else
        {
            Debug.Log("login is failed(登陆失败), err=" + KBEngineApp.app.serverErr(failedcode));
        }
    }

    public void onConnectionState(bool success)
    {
        if (!success)
            Debug.Log("connect(" + KBEngineApp.app.getInitArgs().ip + ":" + KBEngineApp.app.getInitArgs().port + ") is error! (连接错误)");
        else
            Debug.Log("connect successfully, please wait...(连接成功，请等候...)");
    }
}
