
var app = angular.module("message", []);
app.controller("chat", function ($scope, $http, $timeout) {

    $(document).ready(function () {
        if (localStorage.Token == null && localStorage.username == null) {
            alert("Giriş Yapılmadı!");
            return;
        }
        getList();
    })

    $scope.refresh = function () {
        if (localStorage.Token == null && localStorage.username == null) {
            alert("Giriş Yapılmadı!");
            return;
        }
        getList();
    }

    $scope.getMessages = function (otherUser) {
        if (localStorage.Token == null && localStorage.username == null) {
            alert("Giriş Yapılmadı!");
            return;
        }
        getMessages(otherUser);
    }

    $scope.sendMessage = function (otherUser, msg) {
        if (localStorage.Token == null && localStorage.username == null) {
            alert("Giriş Yapılmadı!");
            return;
        }
        sendMessage(otherUser, msg);
    }

    $scope.block = function (otherUser) {
        if (localStorage.Token == null && localStorage.username == null) {
            alert("Giriş Yapılmadı!");
            return;
        }
        blockUser(otherUser);
    }

    $scope.unblock = function (otherUser) {
        if (localStorage.Token == null && localStorage.username == null) {
            alert("Giriş Yapılmadı!");
            return;
        }
        unblockUser(otherUser);
    }

    $scope.login = function (username, password) {
        login(username, password);
    }

    $scope.register = function (username, password, confirmpassword) {
        register(username, password, confirmpassword);
    }

    $scope.logout = function () {
        if (localStorage.Token == null && localStorage.username == null) {
            alert("Giriş Yapılmadı!");
            return;
        }
        logout();
    }


    function getList() {
        var data = { "username": localStorage.username };
        var config = {
            headers: {
                'Token': localStorage.Token,
                'Content-Type': 'application/x-www-form-urlencoded;charset=utf-8;'
            }
        }

        $http({
            params: data,
            url: "/api/Message/getChats",
            type: "GET",
            headers: { 'Token': localStorage.Token },
        }).then(function (response) {
            console.log(response);
            $scope.personList = response.data.Data;
        }, function (response) {
            console.log(response);
        });

        //$http({
        //    method: "GET",
        //    url: "welcome.htm"
        //}).then(function mySuccess(response) {
        //    $scope.myWelcome = response.data;
        //}, function myError(response) {
        //    $scope.myWelcome = response.statusText;
        //});

        //$.ajax({
        //    data: data,
        //    url: "/api/Message/getChats",
        //    type: "GET",
        //    headers: { 'Token': localStorage.Token },
        //    success: function (response) {
        //        if (response.isSuccess) {
        //            $scope.personList = response.Data;

        //        }
        //    },
        //});
    }

    function getMessages(otherUser) {
        var data = { "username": localStorage.username, "otherUser": otherUser };
        var headers = { 'Token': localStorage.Token, "Content-Type": 'application/x-www-form-urlencoded;charset=utf-8;' };

        $http({
            params: data,
            url: "/api/Message/getChats",
            type: "GET",
            headers: headers,
        }).then(function (response) {

            $scope.messageList = response.data.Data[0].messages;
            console.log($scope.messageList);

        }, function (response) {
            console.log(response);
        });
    }

    function sendMessage(otherUser, message) {
        var data = { "username": localStorage.username, "otherUsername": otherUser, "msg": message };
        var headers = { 'Token': localStorage.Token, "Content-Type": 'application/x-www-form-urlencoded;charset=utf-8;' };

        $http({
            params: data,
            url: "/api/Message/sendMessage",
            method: "POST",
            headers: headers,
        }).then(function (response) {
            $(".sendMessage-response span").html(response.data.message);
            $(".chat-text input[type='text']").val("");
        }, function (response) {
            console.log(response);
        });
    }

    function blockUser(otherUser) {
        var data = { "blocker": localStorage.username, "blocked": otherUser };
        var headers = { 'Token': localStorage.Token, "Content-Type": 'application/x-www-form-urlencoded;charset=utf-8;' };

        $http({
            params: data,
            url: "/api/Message/BlockUSer",
            method: "POST",
            headers: headers,
        }).then(function (response) {
            $(".response").html(response.data.message);
            $(".inputs input[type='text']").val("");
        }, function (response) {
            console.log(response);
        });
    }

    function unblockUser(otherUser) {
        var data = { "blocker": localStorage.username, "blocked": otherUser };
        var headers = { 'Token': localStorage.Token, "Content-Type": 'application/x-www-form-urlencoded;charset=utf-8;' };

        $http({
            params: data,
            url: "/api/Message/UnBlockUSer",
            method: "POST",
            headers: headers,
        }).then(function (response) {
            $(".response").html(response.data.message);
            $(".inputs input[type='text']").val("");
        }, function (response) {
            console.log(response);
        });
    }

    function login(username, password) {
        var data = { "username": username, "password": password };

        $http({
            params: data,
            url: "/api/User/Login",
            method: "POST",
        }).then(function (response) {
            $(".login-response").html(response.data.message);
            $(".login-holder input[type='text']").val("");
            if (response.data.isSuccess) {
                localStorage.Token = response.data.Data.token;
                localStorage.username = username;
                location.reload();
            }
        }, function (response) {
            console.log(response);
        });
    }

    function register(username, password, confirmpassword) {
        var data = { "username": username, "password": password, "confirmPassword": confirmpassword };

        $http({
            params: data,
            url: "/api/User/Register",
            method: "POST",
        }).then(function (response) {
            $(".register-response").html(response.data.message);
            $(".register-holder input[type='text']").val("");
            $(".register-holder input[type='password']").val("");
            if (response.data.isSuccess) {
                localStorage.removeItem("Token");
                localStorage.removeItem("username");
                location.reload();
            }
        }, function (response) {
            console.log(response);
        });
    }

    function logout() {
        localStorage.removeItem("Token");
        localStorage.removeItem("username");
        location.reload();
    }
});
