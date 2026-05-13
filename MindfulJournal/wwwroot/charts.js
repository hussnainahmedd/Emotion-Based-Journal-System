window.chartsReady = true;
window.renderPieChart = (canvasId, labels, data, colors) => {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;

    if (ctx._chartInstance) ctx._chartInstance.destroy();

    ctx._chartInstance = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: colors,
                borderWidth: 2,
                borderColor: '#fff'
            }]
        },
        options: {
            responsive: false,
            plugins: {
                legend: {
                    position: 'right',
                    labels: { font: { size: 11 } }
                }
            }
        }
    });
};

window.renderLineChart = (canvasId, labels, datasets) => {
    const ctx = document.getElementById(canvasId);
    if (!ctx) return;
    if (ctx._chartInstance) ctx._chartInstance.destroy();
    ctx._chartInstance = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: datasets
        },
        options: {
            responsive: false,
            plugins: { legend: { position: 'top' } },
            scales: {
                y: { beginAtZero: true, ticks: { stepSize: 1 } }
            }
        }
    });
};
window.applyTheme = function (isDark) {
    if (isDark) {
        document.body.classList.add('dark-mode');
    } else {
        document.body.classList.remove('dark-mode');
    }
    localStorage.setItem('darkMode', isDark.ToString ? isDark.ToString() : String(isDark));
};

window.getLocalStorage = function (key) {
    return localStorage.getItem(key);
};

window.setLocalStorage = function (key, value) {
    localStorage.setItem(key, value);
};

// Auto-apply theme on page load
(function () {
    const dark = localStorage.getItem('darkMode');
    if (dark === 'True') {
        document.body.classList.add('dark-mode');
    }
})();

let recognition = null;
let dotNetRef = null;

window.initSpeechRecognition = (ref) => {
    dotNetRef = ref;
    const SpeechRecognition = window.SpeechRecognition
        || window.webkitSpeechRecognition;

    if (!SpeechRecognition) {
        console.log('Speech recognition not supported');
        return;
    }

    recognition = new SpeechRecognition();
    recognition.continuous = true;
    recognition.interimResults = true;
    recognition.lang = 'en-US';

    recognition.onresult = (event) => {
        let finalText = '';
        for (let i = event.resultIndex; i < event.results.length; i++) {
            if (event.results[i].isFinal) {
                finalText += event.results[i][0].transcript + ' ';
            }
        }
        if (finalText.trim()) {
            dotNetRef.invokeMethodAsync('UpdateTranscript', finalText.trim());
        }
    };

    recognition.onerror = (event) => {
        console.error('Speech error:', event.error);
        dotNetRef.invokeMethodAsync('StopRecordingCallback');
    };

    recognition.onend = () => {
        dotNetRef.invokeMethodAsync('StopRecordingCallback');
    };
};

window.startRecording = () => {
    if (recognition) {
        try { recognition.start(); }
        catch (e) { console.log('Already started'); }
    }
};

window.stopRecording = () => {
    if (recognition) {
        try { recognition.stop(); }
        catch (e) { console.log('Already stopped'); }
    }
};

window.checkSpeechSupport = () => {
    return !!(window.SpeechRecognition || window.webkitSpeechRecognition);
};