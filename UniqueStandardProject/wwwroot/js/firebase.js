import { initializeApp } from "https://www.gstatic.com/firebasejs/10.3.0/firebase-app.js";
import { getAnalytics } from "https://www.gstatic.com/firebasejs/10.3.0/firebase-analytics.js";
import { getMessaging, getToken } from "https://www.gstatic.com/firebasejs/10.3.0/firebase-messaging.js";
import { getPerformance } from "https://www.gstatic.com/firebasejs/10.3.0/firebase-performance.js";

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
const analytics = getAnalytics(app);
// Initialize Firebase Cloud Messaging and get a reference to the service
const messaging = getMessaging(app);

// Initialize Performace Monitoring and get a reference to the serivce
const perf = getPerformance(app);

Notification.requestPermission().then(function (permission) {
    if (permission == "granted") {
        console.log('Notification permission granted.');
    }
    else if (permission == "denied") {
        console.log('Notification Permission Denied');
    }
}).catch(function (err) {
    console.log(err);
});

getToken(messaging, { vapidKey: 'BES7hZ6f7f9slkxudLdY7PQJTg5ZmYP4ly6IniLeD6SyfufoMy-Wqu9G6pQb0fF-kNUwUA_HLG71I6Kaxy3iGxI' }).then((currentToken) => {
    if (currentToken) {
        // Send the token to your server and update the UI if necessary
        // ...
        console.log(currentToken);
    } else {
        // Show permission request UI
        console.log('No registration token available. Request permission to generate one.');
        // ...
    }
}).catch((err) => {
    console.log('An error occurred while retrieving token. ', err);
    // ...
});