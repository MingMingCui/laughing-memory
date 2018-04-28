//using MessagePack;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
public class MsgHead
{
    public short cmd_id ;   // 主命令 ID
    public short sub_id ; // 子命令 ID
    public int param1; // 附加参数1
    public int param2;// 附加参数2
    public int len;   //附加数据的长度	// 这个字段置 0
}

