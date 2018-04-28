using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Runtime.InteropServices;
using Msg;
using Msg.LoginMsg;

public enum NetStatus
{
    Disconnected = 0,
    Connected = 1,
}

public struct ServerMsgObj
{
    public int MsgId;
    public int SubId;
    public string Msg;
}

public class Client :  Singleton<Client>, IUpdate
{

    static Socket client;
    public byte[] readBuffer;
    public int msgLength; //
    public int bufferCount = 0;  //当前有多少字节
    public const int MAX_BUFFER = 1024; //数组内总字节
    public const float ConnectCD = 1.0f;
    //public int extent;
    public NetStatus status = NetStatus.Disconnected;
    //链接成功后自动登录还是自动注册
    public bool AutoLoginOrRegist = true;
    private LinkedList<byte[]> _msgSend = new LinkedList<byte[]>();
    private LinkedList<ServerMsgObj> _msgReceived = new LinkedList<ServerMsgObj>();
    private float _lastConnectTime = 0;
    private string _userName = "";
    private string _passWd = "";
    private bool _login = false;
    private byte[] keepalive = null;
    private float lastKeepAliveSend = 0;    //最后一次发心跳时间
    private const float KeepAliveInterval = 5;      //发心跳间隔
    private float lastKeepAliveReceive = 0;         //最后一次收到心跳时间
    private const float KeepAliveTimeOut = 15;      //超时掉线时间
    private bool _openDisconnect = false;

    public Client()
    {
        readBuffer = new byte[MAX_BUFFER];
        MsgHead keepaliveHead = new MsgHead
        {
            cmd_id = (int)ServerMsgId.CCMD_KEEP_ALIVE
        };
        keepalive = ProtocolByt.StructToBytes(keepaliveHead, 16);
        ProcessCtrl.Instance.AddUpdate(this);
    }

    public void SetLoginData(string username, string passwd, bool autoLoginOrRegist)
    {
        this._userName = username;
        this._passWd = passwd;
        this.AutoLoginOrRegist = autoLoginOrRegist;
    }

    public void Reset()
    {
        this._userName = "";
        this._passWd = "";
        this.AutoLoginOrRegist = true;
        this.status = NetStatus.Disconnected;
        this._login = false;
        this._msgSend.Clear();
        this._msgReceived.Clear();
    }

    public void Connect()
    {
        if (status == NetStatus.Connected)
        {
            EDebug.Log("服务器已连接！");
            OnConnectOver();
            return;
        }
        string gateHost = "47.104.82.214";
        int gatePort = 3101;
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            client.Connect(gateHost, gatePort);
            status = NetStatus.Connected;
            client.BeginReceive(readBuffer, 0, MAX_BUFFER - bufferCount, SocketFlags.None, OnReceiveCb, readBuffer);
            OnConnectOver();
            EDebug.Log("连接上服务器了");
        }
        catch (System.Exception e)
        {
            EDebug.Log(string.Format("服务器连接失败：\n",e.ToString()));
            OnDisconnect(true);
            throw;
        }
    }

    public void OnConnectOver()
    {
        if (!string.IsNullOrEmpty(_userName) && !string.IsNullOrEmpty(_passWd))
        {
            Login();
        }
    }

    public void OnDisconnect(bool openDisconnect = false)
    {
        //弹出重新连接UI
        //CanvasView.Instance.OpenConnect(true);
        status = NetStatus.Disconnected;
        _login = false;
        lastKeepAliveReceive = 0;
        lastKeepAliveSend = 0;
        bufferCount = 0;

        //重连
        //Connect();
        Debug.Log("OnDisconnect");
        _openDisconnect = openDisconnect;
        if (!openDisconnect)
            Connect();
    }

    /// <summary>
    /// 注册和登录都走这里，根据AutoLoginOrRegister区分
    /// </summary>
    public void Login()
    {
        if (string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_passWd))
        {
            return;
        }
        EDebug.LogFormat("Login {0} {1} Login Or Regist {2}", _userName, _passWd, AutoLoginOrRegist);
        MsgHead head = new MsgHead
        {
            cmd_id = AutoLoginOrRegist ? (short)ServerMsgId.CCMD_ROLE_AUTH : (short)ServerMsgId.CCMD_ROLE_REG
        };
        LoginMsg loginMsg = new LoginMsg
        {
            type = 0,
            name = _userName,
            passwd = _passWd
        };
        string loginInfo = JsonUtility.ToJson(loginMsg);
        byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(loginInfo);
        head.len = bMsg.Length;
        byte[] bHead = ProtocolByt.StructToBytes(head, 16);
        byte[] buffer = new byte[bHead.Length + bMsg.Length];
        System.Array.Copy(bHead, buffer, bHead.Length);
        System.Array.Copy(bMsg, 0, buffer, bHead.Length, bMsg.Length);
        try
        {
            client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, null, null);
        }
        catch (Exception e)
        {
            EDebug.LogErrorFormat("Client.Login Socket Exception {0}", e.ToString());
            OnDisconnect();
        }
    }

    public void OnLoginResult(bool success)
    {
        if (success)
        {
            EDebug.Log("LoginSuccess");
            _login = true;
            lastKeepAliveReceive = Time.time;
            //CanvasView.Instance.OpenConnect(false);
        }
        else
            Reset();
    }

    public void OnReceiveCb(IAsyncResult ar)
    {
        int count = client.EndReceive(ar);
        bufferCount += count;
        Analysis(this);
        client.BeginReceive(readBuffer, bufferCount, MAX_BUFFER - bufferCount, SocketFlags.None, OnReceiveCb, readBuffer);
    }

    public void OnSendCallback(IAsyncResult result)
    {
        try
        {
            Socket client = (Socket)result.AsyncState;
            client.EndSend(result);
        }
        catch (Exception e)
        {
            Debug.LogError("send msg exception:" + e.StackTrace);
            OnDisconnect();
        }
    }

    public void Analysis(Client pClient)
    {
        int Count = pClient.bufferCount;
        try
        {
            pClient.msgLength = pClient.readBuffer.Length;
      
            object cmdmsgs = ProtocolByt.ByteToStruct(pClient.readBuffer, typeof(MsgHead));
            MsgHead head = (MsgHead)cmdmsgs;
            pClient.bufferCount -= Marshal.SizeOf(head);
            string msg = "";
            if (pClient.bufferCount > 0)
            {
                msg = System.Text.Encoding.UTF8.GetString(pClient.readBuffer, Count - pClient.bufferCount, head.len);
                bufferCount = 0;
            }

            EDebug.LogFormat("Analysis {0} {1}", head.cmd_id, msg);

            ServerMsgObj serverMsgPair = new ServerMsgObj
            {
                MsgId = (int)head.cmd_id,
                SubId = (int)head.sub_id,
                Msg = msg
            };
            _msgReceived.AddLast(serverMsgPair);
        }
        catch (Exception e)
        {
            EDebug.Log(e.ToString());
            OnDisconnect();
            //throw;
        }
    }

    public void Send(ServerMsgId pMsgId, object o, short sub_id = 0, uint arg1 = 0, uint arg2 = 0)
    {
        EDebug.Log("Send " + pMsgId.ToString());
        //if (NetStatus.Disconnected == status)
        //    Connect();
        MsgHead head = new MsgHead
        {
            cmd_id = (short)pMsgId,
            sub_id = sub_id,
            param1 = (int)arg1,
            param2 = (int)arg2
        };
        string msg = JsonUtility.ToJson(o);
        byte[] bMsg = System.Text.Encoding.UTF8.GetBytes(msg);
        head.len = bMsg.Length;
        byte[] bHead = ProtocolByt.StructToBytes(head, 16);
        byte[] buffer = new byte[bHead.Length + bMsg.Length];
        System.Array.Copy(bHead, buffer, bHead.Length);
        System.Array.Copy(bMsg, 0, buffer, bHead.Length, bMsg.Length);
        _msgSend.AddLast(buffer);
    }

    public void Update()
    {
        if (_openDisconnect)
        {
            CanvasView.Instance.OpenConnect(true);
            _openDisconnect = false;
        }
        if(Application.internetReachability == NetworkReachability.NotReachable)
        {

            return;
        }
        if (client.Connected == true)
        {
            if (_login)
            {
                if (lastKeepAliveReceive > 0 && Time.time - lastKeepAliveReceive > KeepAliveTimeOut)
                {
                    OnDisconnect();
                    return;
                }
                if (Time.time - lastKeepAliveSend >= KeepAliveInterval)
                {
                    try
                    {
                        client.BeginSend(keepalive, 0, 16, SocketFlags.None, OnSendCallback, client);
                        lastKeepAliveSend = Time.time;
                    }
                    catch (SocketException se)
                    {
                        EDebug.LogErrorFormat("Client KeepAlive Socket Exception {0}", se.Message);
                        OnDisconnect();
                        return;
                    }
                }
                if (_msgSend.Count > 0)
                {
                    byte[] buffer = _msgSend.First.Value;
                    try
                    {
                        client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, OnSendCallback, client);
                        Debug.Log("Send Data Over");
                        _msgSend.RemoveFirst();
                    }
                    catch (Exception e)
                    {
                        EDebug.LogErrorFormat("Client Socket Exception {0}", e.ToString());
                        OnDisconnect();
                    }
                }
            }
            if (_msgReceived.Count > 0)
            {
                ServerMsgObj serverMsgPair = _msgReceived.First.Value;
                //如果是登录消息，截获一下
                if (serverMsgPair.MsgId == (int)ServerMsgId.DCMD_AUTH_SUCCEEDED)
                    OnLoginResult(true);
                else if (serverMsgPair.MsgId == (int)ServerMsgId.ECMD_AUTH_FAILED)
                    OnLoginResult(false);
                else if (serverMsgPair.MsgId == (int)ServerMsgId.CCMD_KEEP_ALIVE)
                {
                    //收到心跳
                    lastKeepAliveReceive = Time.time;
                }

                try
                {
                    MsgFactory.Instance.OnRecvMsg(serverMsgPair);
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("Client->MsgFactory.Instance.OnRecvMsg error, {0}", e.ToString()));
                }
                finally
                {
                    if (_msgReceived != null && _msgReceived.Count > 0)
                        _msgReceived.RemoveFirst();
                }
            }
        }
       
    }

    ~Client()
    {
        if (client != null)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }

}
