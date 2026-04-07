using System;
using UnityEngine;

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