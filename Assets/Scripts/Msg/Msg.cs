using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Msg
{
    public enum ServerMsgId : short
    {
        CCMD_ROLE_AUTH = 100,           // 用户认证, 附加数据: 认证结构
        CCMD_ROLE_REG = 101,            // 用户注册
        CCMD_ROLE_REQ_TEMPID = 102,     // 请求临时 ID
        CCMD_ROLE_REG_CHARTER = 103,    // 注册角色
        CCMD_ROLE_STATE = 104,          //修改角色信息
        CCMD_ROLE_CHAT = 105,           //聊天命令
        CCMD_OPEN_SHOP = 106,           //打开商店
        CCMD_SAVE_FORMATION = 107,      //保存阵型
        CCMD_KEEP_ALIVE = 108,          //心跳
        CCMD_BATTLE_BEGIN = 109,        // 战斗开始
        CCMD_BATTLE_END = 110,			// 战斗结束



        DCMD_AUTH_SUCCEEDED = 500,      //用户认证成功
        DCMD_REG_SUCCEEDED = 501,       //用户注册成功
        DCMD_RESP_TEMPID = 502,         // 请求临时用户 ID 的回复
        DCMD_RESP_REG_CHARTER = 503,    // 注册角色回复
        DCMD_RESP__STATE = 504,          //修改角色信息回复
        SCMD_ROLE_CHAT = 505,           // 聊天消息
        SCMD_RESP_OPEN_SHOP = 506,      // 打开商店回复
        SCMD_RESP_SAVE_FORMATION = 507, // 保存阵型回复
        SCMD_RESP_BATTLE_BEGIN = 508,   // 战斗开始回复
        SCMD_RESP_BATTLE_END = 509,		// 战斗结束回复


        ECMD_AUTH_FAILED = 900,         //通知客户端认证失败, sub_id 为失败原因代码// 0: 未知错误, 1: 连接数据服务失败, 2: 无效的用户或密码错误
        ECMD_REG_FAILED = 901,          //通知用户注册失败, sub_id 为失败原因代码
        ECMD_REQ_TEMPID_FAILED = 902,   //请求临时 ID 失败
        ECMD_REG_CHARTER = 903,         // 注册角色失败
        ECMD_REG__STATE = 904,          //修改角色信息失败
        ECMD_ROLE_CHAT = 905,           // 聊天错误
        ECMD_OPEN_SHOP = 906,           // 打开商店失败
        ECMD_SAVE_FORMATION = 907,      //保存阵型失败
        ECMD_BATTLE_BEGIN = 908,        // 战斗开始错误
        ECMD_BATTLE_END = 909,			// 战斗结束错误

        //更新装备
        C2S_UPDATEEQUIP,
        S2C_UPDATEEQUIP,
        //武将数据
        S2C_LOADHERODATA ,
        //装备数据
        S2C_LOADEQUIPDATA,

        S2C_LOADTOTEMDATA
    }
}
