//create connection
var connectionUserCount = new signalR.HubConnectionBuilder().withUrl("/hubs/userCount").build();

//connect to method that hub invokes aka receive notification from hub
connectionUserCount.on("updateTotalViews", (value) => {    
    var newCountSpan = document.getElementById("totalViewsCounter");
    newCountSpan.innerText = value.ToString();    
});

//invoke hub method aka send notification to hub
function newWindowLoadedonClinet() {
    connectionUserCount.send("NewWindowLoaded");
}

//start connection
function fulfilled() {
    //do something on start]
    console.log("Connection to user Hub successful");
    newWindowLoadedonClinet();
}

function rejected() {
    //do something on start

}

connectionUserCount.start().then(fulfilled, rejected);