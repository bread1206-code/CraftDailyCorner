document.addEventListener("DOMContentLoaded", function () {
    const audio = document.getElementById("globalAudio");
    const toggleBtn = document.getElementById("musicToggleBtn");
    const toggleIcon = document.getElementById("musicToggleIcon");
    const volumeSlider = document.getElementById("musicVolumeSlider");
    const prevBtn = document.getElementById("musicPrevBtn");
    const nextBtn = document.getElementById("musicNextBtn");

    if (!audio) return;

    const playlist = [
        "/music/bgm1.mp3",
        "/music/bgm2.mp3",
        "/music/bgm3.mp3"
    ];

    const STORAGE_KEYS = {
        isPlaying: "cdc_music_isPlaying",
        currentTime: "cdc_music_currentTime",
        volume: "cdc_music_volume",
        trackIndex: "cdc_music_trackIndex",
        userInteracted: "cdc_music_userInteracted"
    };

    let currentTrackIndex = 0;
    let pendingRestoreTime = 0;

    function setIcon(isPlaying) {
        if (!toggleIcon) return;
        toggleIcon.classList.remove("bi-play-fill", "bi-pause-fill");
        toggleIcon.classList.add(isPlaying ? "bi-pause-fill" : "bi-play-fill");
    }

    function saveState() {
        try {
            localStorage.setItem(STORAGE_KEYS.isPlaying, String(!audio.paused));
            localStorage.setItem(STORAGE_KEYS.currentTime, String(audio.currentTime || 0));
            localStorage.setItem(STORAGE_KEYS.volume, String(audio.volume));
            localStorage.setItem(STORAGE_KEYS.trackIndex, String(currentTrackIndex));
        } catch (e) {
            console.warn("saveState failed:", e);
        }
    }

    function saveUserInteracted() {
        try {
            localStorage.setItem(STORAGE_KEYS.userInteracted, "true");
        } catch (e) {
            console.warn("saveUserInteracted failed:", e);
        }
    }

    function restoreVolume() {
        try {
            const savedVolume = localStorage.getItem(STORAGE_KEYS.volume);
            let volume = 0.5;

            if (savedVolume !== null) {
                const parsed = parseFloat(savedVolume);
                if (!isNaN(parsed)) {
                    volume = parsed;
                }
            }

            audio.volume = volume;

            if (volumeSlider) {
                volumeSlider.value = String(volume);
            }
        } catch (e) {
            console.warn("restoreVolume failed:", e);
        }
    }

    function restoreTrackIndex() {
        try {
            const savedIndex = localStorage.getItem(STORAGE_KEYS.trackIndex);
            let index = 0;

            if (savedIndex !== null) {
                const parsed = parseInt(savedIndex, 10);
                if (!isNaN(parsed) && parsed >= 0 && parsed < playlist.length) {
                    index = parsed;
                }
            }

            currentTrackIndex = index;
        } catch (e) {
            console.warn("restoreTrackIndex failed:", e);
            currentTrackIndex = 0;
        }
    }

    function setTrackSource(index) {
        if (!playlist.length) return;

        if (index < 0) {
            currentTrackIndex = playlist.length - 1;
        } else if (index >= playlist.length) {
            currentTrackIndex = 0;
        } else {
            currentTrackIndex = index;
        }

        audio.src = playlist[currentTrackIndex];
        audio.load();
    }

    function tryRestorePlayback() {
        try {
            const savedTime = parseFloat(localStorage.getItem(STORAGE_KEYS.currentTime) || "0");
            const shouldPlay = localStorage.getItem(STORAGE_KEYS.isPlaying) === "true";
            const userInteracted = localStorage.getItem(STORAGE_KEYS.userInteracted) === "true";

            pendingRestoreTime = !isNaN(savedTime) ? savedTime : 0;

            const onReady = () => {
                if (pendingRestoreTime > 0 && isFinite(pendingRestoreTime)) {
                    try {
                        audio.currentTime = pendingRestoreTime;
                    } catch (e) {
                        console.warn("restore currentTime failed:", e);
                    }
                }

                if (shouldPlay && userInteracted) {
                    audio.play()
                        .then(() => {
                            setIcon(true);
                            saveState();
                        })
                        .catch((err) => {
                            console.warn("auto resume blocked:", err);
                            setIcon(false);
                        });
                } else {
                    setIcon(false);
                }
            };

            if (audio.readyState >= 1) {
                onReady();
            } else {
                audio.addEventListener("loadedmetadata", onReady, { once: true });
            }
        } catch (e) {
            console.warn("tryRestorePlayback failed:", e);
            setIcon(false);
        }
    }

    function loadTrack(index, shouldPlayAfterLoad) {
        const targetTime = 0;

        setTrackSource(index);

        const onReady = () => {
            try {
                audio.currentTime = targetTime;
            } catch (e) {
                console.warn("set currentTime on track change failed:", e);
            }

            if (shouldPlayAfterLoad) {
                audio.play()
                    .then(() => {
                        setIcon(true);
                        saveState();
                    })
                    .catch((err) => {
                        console.warn("play after track change failed:", err);
                        setIcon(false);
                    });
            } else {
                setIcon(false);
                saveState();
            }
        };

        if (audio.readyState >= 1) {
            onReady();
        } else {
            audio.addEventListener("loadedmetadata", onReady, { once: true });
        }
    }

    restoreTrackIndex();
    restoreVolume();
    setTrackSource(currentTrackIndex);
    tryRestorePlayback();

    if (toggleBtn) {
        toggleBtn.addEventListener("click", function () {
            saveUserInteracted();

            if (audio.paused) {
                audio.play()
                    .then(() => {
                        setIcon(true);
                        saveState();
                    })
                    .catch((err) => {
                        console.warn("play failed:", err);
                    });
            } else {
                audio.pause();
                setIcon(false);
                saveState();
            }
        });
    }

    if (volumeSlider) {
        volumeSlider.addEventListener("input", function () {
            const value = parseFloat(this.value);
            if (!isNaN(value)) {
                audio.volume = value;
                saveState();
            }
        });
    }

    if (prevBtn) {
        prevBtn.addEventListener("click", function () {
            saveUserInteracted();
            const shouldPlayAfterLoad = !audio.paused;
            loadTrack(currentTrackIndex - 1, shouldPlayAfterLoad);
        });
    }

    if (nextBtn) {
        nextBtn.addEventListener("click", function () {
            saveUserInteracted();
            const shouldPlayAfterLoad = !audio.paused;
            loadTrack(currentTrackIndex + 1, shouldPlayAfterLoad);
        });
    }

    audio.addEventListener("play", function () {
        setIcon(true);
        saveState();
    });

    audio.addEventListener("pause", function () {
        setIcon(false);
        saveState();
    });

    audio.addEventListener("timeupdate", function () {
        saveState();
    });

    audio.addEventListener("volumechange", function () {
        if (volumeSlider) {
            volumeSlider.value = String(audio.volume);
        }
        saveState();
    });

    audio.addEventListener("ended", function () {
        loadTrack(currentTrackIndex + 1, true);
    });

    window.addEventListener("beforeunload", function () {
        saveState();
    });

    document.addEventListener("visibilitychange", function () {
        saveState();
    });
});