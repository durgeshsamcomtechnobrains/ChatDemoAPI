//create connection
var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/userCount").build();

//connect to methods that hub invokes aka receive notfications from hub
connectionUserCount.on("updateTotalViews", (value) => {
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.toString();
});

connectionUserCount.on("updateTotalUsers", (value) => {
    var newCountSpan = document.getElementById("totalUsersCounter");
    newCountSpan.innerText = value.toString();
});

//invoke hub methods aka send notification to hub
function newWindowLoadedOnClient() {
    connectionUserCount.invoke("NewWindowLoaded", "SAMCOM").then((value) => console.log(value));
    //connectionUserCount.send("NewWindowLoaded").then((value) => console.log(value));//when we use send we do not get any response from the server
}

//start connection
function fulfilled() {
    //do something on start
    console.log("Connection to User Hub Successful");
    newWindowLoadedOnClient();
}
function rejected() {
    //rejected logs
}

connectionUserCount.start().then(fulfilled, rejected);