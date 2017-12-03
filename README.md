# OfflineMessage

DOCS

LOGIN
/api/User/Login
Method: POST
Params: (string username, string password)
returnType:
{
    "isSuccess": bool,
    "message": string,
    "Data": {
        "ID": int,
        "userID":int,
        "token": string,
        "date": string,
        "expireDate": string
    }
}
REGISTER
/api/User/Register
Method: POST
Params: (string username, string password, string confirmPassword)
returnType:
{
    "isSuccess": bool,
    "message": string
}


SEND MESSAGE
/api/User/sendMessage
Method: POST
Params: (string username, string otherUsername, string msg)
returnType:
{
    "isSuccess": true,
    "message": string
}
GET CHATS
/api/Message/getChats
Method: POST
Params: (string username, string otherUsername(Optional for specific person) )
returnType:
{
    "isSuccess": bool,
    "message": string,
    "Data": [
        {
            "firstMessageDate": string,
            "lastMessageDate": string,
            "otherUser": string,
            "messages": [
                {
                    "sender": string,
                    "recipient": string,
                    "date": string,
                    "text": string,
                    "isRead": bool,
                    "readDate":string
                } 
            ]
        } 
    ]
}

BLOCK
/api/Message/BlockUSer
Method: POST
Params: (string blocker, string blocked )
returnType:
{
    "isSuccess": bool,
    "message": string
}

UNBLOCK
/api/Message/UnBlockUSer
Method: POST
Params: (string blocker, string blocked )
returnType:
{
    "isSuccess": bool,
    "message": string
}


