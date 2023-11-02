import { initializeApp } from "https://www.gstatic.com/firebasejs/9.10.0/firebase-app.js";
import { getMessaging, getToken, onMessage } from "https://www.gstatic.com/firebasejs/9.10.0/firebase-messaging.js";
import { getPerformance } from "https://www.gstatic.com/firebasejs/9.10.0/firebase-performance.js";
import { getNotification, insertNotification } from "/js/indexDB.js";

const firebaseConfig = {
    apiKey: "AIzaSyDt4mgBNFwdrKHVCNrTz4uuPXswJyjehgY",
    authDomain: "ggluck-6fb0a.firebaseapp.com",
    projectId: "ggluck-6fb0a",
    storageBucket: "ggluck-6fb0a.appspot.com",
    messagingSenderId: "828206639681",
    appId: "1:828206639681:web:73582b67c5d51e02ccf4c0",
    measurementId: "G-Q3MM702N30"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);

// Initialize Firebase Cloud Messaging and get a reference to the service
const messaging = getMessaging(app);

// Initialize Performace Monitoring and get a reference to the serivce
const perf = getPerformance(app);

onMessage(messaging, (payload) => {

    var noti = payload.notification;
    var data = payload.data;

    if (data.type == 'notification') {
        // Inserting IndexedDb Item
        var db_item = {
            date: new Date(),
            title: noti.title,
            body: noti.body,
            redirectUrl: data.redirectUrl,
            role: data.role,
            jsonData: data.jsonData,
            read: false
        };
        insertNotification(db_item);
        notification('info', noti.title, noti.body);
        getNotification();
    }
    else if (data.type == 'chat') {
        const chating = JSON.parse(payload.data.jsonData);
        if (chating.ConversationId == window.localStorage.getItem('current_conversation')) {
            appendMessage(chating, getUTCTimeformatAMPM(new Date(chating.SentOn)), payload.data.sender_fullName);
        }
        notification('success', noti.title, noti.body);
    }

    // Auto Play Notification Sound
    playNotiAudio();

});

Notification.requestPermission().then(function (permission) {
    if (permission == "granted") {
        if (getCookie('notification_token') == null) {
            navigator.serviceWorker.getRegistration('/firebase-messaging-sw.js').then(registrations => {
                setTimeout(getDeviceToken(), 1000);
            });
        }
    }
    else if (permission == "denied") {
        console.log('Notification Permission Denied');
    }
}).catch(function (err) {
    console.log(err);
});

function appendMessage(chating, time, sender_fullName) {

    var messageList = document.getElementById('messageList');
    if (messageList !== undefined) {
        $('#nullMessageStatus').remove();
        var li = $('<li></li>');
        var conversationMessage = $('<div class="conversation-list"></div>');
        console.log(chating);
        if (chating.ChatAttachment !== null) {
            if (chating.ChatAttachment.FilePath.split('/')[1] == "images") {
                const imageAtt = $('<div class="mb-2"><img src="' + chating.ChatAttachment.FilePath + '" class="img-thumbnail" style="width:150px;height:auto" /></div>');
                conversationMessage.append(imageAtt);
            }
            else if (chating.ChatAttachment.FilePath.split('/')[1] == "files") {
                const FileAtt =
                    $('<div class="mb-2"><a href="' + chating.ChatAttachment.FilePath + '" download="' + chating.Message + '" class="">' +
                        '<i class="bx bx-file" style="font-size:5.3em"></i>' + '</a></div>');
                conversationMessage.append(FileAtt);
            }
        }

        var wrapTextDix = $('<div class="ctext-wrap"></div>');

        const pFulleName = '<b>' + sender_fullName + '</b>';
        if (window.localStorage.getItem('current_conversation_type') !== 'P2P')
            wrapTextDix.append(pFulleName);

        var pMessage = $('<p class="mb-1">' + chating.Message + '</p>');
        wrapTextDix.append(pMessage);
        var pTime = $('<p class="chat-time mb-0"><i class="bx bx-check-double align-middle me-1 text-primary"></i>' + time + '</p>');
        wrapTextDix.append(pTime);

        conversationMessage.append(wrapTextDix);
        li.append(conversationMessage);

        var simplebarContent = messageList.getElementsByClassName("simplebar-content");
        $(simplebarContent[0]).append(li.hide().fadeIn('1000'));

        messageList = document.getElementById('messageList').SimpleBar.getScrollElement();
        messageList.scrollTop = $(messageList.getElementsByClassName('simplebar-content')[0]).height();
    }
}

function getDeviceToken() {
    getToken(messaging, { vapidKey: 'BES7hZ6f7f9slkxudLdY7PQJTg5ZmYP4ly6IniLeD6SyfufoMy-Wqu9G6pQb0fF-kNUwUA_HLG71I6Kaxy3iGxI' }).then((currentToken) => {
        if (currentToken) {
            document.cookie = "notification_token=" + currentToken + ";path=/;SaneSite=None";
            console.log(currentToken);
        } else {
            console.log('No registration token available. Request permission to generate one.');
        }
    }).catch((err) => {
        console.log('An error occurred while retrieving token. ', err);
        // ...
    });
}

function getUTCTimeformatAMPM(date) {
    var hours = date.getUTCHours();
    var minutes = date.getUTCMinutes();
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // the hour '0' should be '12'
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var strTime = hours + ':' + minutes + ' ' + ampm;
    return strTime;
}
