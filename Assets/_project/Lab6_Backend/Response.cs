using System;
using UnityEngine;
using System.Collections.Generic; // <--- BẠN ĐANG THIẾU DÒNG NÀY

[Serializable]
public class Response
{
    public bool issuccess;
    public string notification;
    public Account data;
}

[Serializable]
public class Account
{
    public int id;
    public string email;
    public string password;
    public string userName;
    public DateTime createAt;
}

[System.Serializable]
public class ResponseAccountList
{
    public bool issuccess;
    public string notification;
    public List<Account> data;
}
[Serializable]
public class Character
{
    public int id;
    public string name;
    public string email; // Email của chủ sở hữu nhân vật
    public int level;
    public int health;
}

[Serializable]
public class ResponseCharacter
{
    public bool issuccess;
    public string notification;
    public Character data;
}

[Serializable]
public class ResponseCharacterList
{
    public bool issuccess;
    public string notification;
    public List<Character> data;
}