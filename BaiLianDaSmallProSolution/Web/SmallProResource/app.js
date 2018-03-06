//app.js
App({
  onLaunch: function () {
    

  },
  globalData: {
    userInfo: null,
    openId: null,
    userInfoId:null
  },
  webUrl: "https://smallpro.1194792182.com",
  // webUrl: "http://localhost:51193",
  updateUserInfo: (myApp, userInfo,afterSuccessFun) => {
    wx.request({
      url: myApp.webUrl + "/api/Base",
      data: {
        NickName: userInfo.nickName,
        AvatarUrl: userInfo.avatarUrl,
        OpenId: myApp.globalData.openId
      },
      header: {
        'content-type': 'application/json' // 默认值
      },
      success: function (res) {
        myApp.globalData.userInfoId = res.data.UserInfoId
        afterSuccessFun()
        // console.log(res)
      }
    })
  }
})