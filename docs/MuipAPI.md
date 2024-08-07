[EN](MuipAPI.md) | [简中](MuipAPI_zh-CN.md) | [繁中](MuipAPI_zh-TW.md) | [JP](MuipAPI_ja-JP.md)
## 💡API Help
- Since version 2.3, external APIs are supported
- For example, your Dispatch is http://127.0.0.1:8080, and the request parameters and returns are in json format
- (1) Authorization interface: http://127.0.0.1:8080/muip/auth_admin (support POST)
   - -Required parameter 1: admin_key (MuipServer/AdminKey configuration in config.php)
   - -Required parameter 2: key_type (type, e.g. PEM or XML)
  - -Return example:
  ```json
  {
    "code": 0,
  //codeResponse: `code`: `0 -> Success` `1 -> Token incorrect or not enable`
    "message": "Authorized admin key successfully!",
    "data": {
        "rsaPublicKey": "***",
        "sessionId": "***",
        "expireTimeStamp": ***
    }
  }
  ```
- (2)Submit command interface: http://127.0.0.1:8080/muip/exec_cmd (support POST/GET)
  - -Required parameter 1: SessionId (obtained after authorization API request)
  - -Required parameter 2: Command (the command to be executed is encrypted by RSA[pacs#1] under rsaPublicKey)
  - -Required parameter 3: TargetUid (UID of the player executing the command)
  - -Return example:
    ```json
    {
      "code": 0,
    //codeResponse: `code`: `0 -> Success` `1 -> Session expired` `2 -> session not found` `3 -> encryption error`
      "message": "Success",
      "data": {
          "sessionId": "***",
          "message": "*** //base64
      }
    }
    ```
- (3)Interface to get server status: http://127.0.0.1:8080/muip/server_information (support POST/GET)
  - -Required parameter 1: SessionId (obtained after authorization API request)
  - -Return example:
   ```json
    {
      "code": 0,
   //codeResponse: `code`: `0 -> Success` `1 -> Session expired` `2 -> session not found` 
      "message": "Success",
      "data": {
          "onlinePlayers": [
              {
                  "uid": 10001,
                  "name": "KEVIN",
                  "headIconId": 208001
              },
              ....
          ],
          "serverTime": 1720626191,
          "maxMemory": 16002.227,
          "usedMemory": 7938.5547,
         "programUsedMemory": 323
      }
    }
    ```
- (4)Interface to get player information: http://127.0.0.1:8080/muip/player_information (support POST/GET)
  - -Required parameter 1: SessionId (obtained after authorization API request)
  - -Required parameter 2: Uid (player UID)
  - -Return example:
   ```json
    {
      "code": 0,
   //Response: `code`: `0 -> Success` `1 -> Session expired` `2 -> player not exist` `3 -> session not found` 
      "message": "Success",
      "data": {
          "uid": 10001,
          "name": "KEVIN",
          "signature": "",
          "headIconId": 208001,
          "curPlaneId": 10001,
          "curFloorId": 10001001,
          "playerStatus": "Explore",
          "stamina": 182,
          "recoveryStamina": 4,
          "assistAvatarList": Array[0],
          "displayAvatarList": Array[0],
          "finishedMainMissionIdList": Array[38],
          "finishedSubMissionIdList": Array[273],
          "acceptedMainMissionIdList": Array[67],
          "acceptedSubMissionIdList": Array[169]
      }
  }
  ```
