import React, { useRef, useState } from 'react';
import './AudioPlayer.css';
import osrsSound from './osrs.mp3';

const AudioPlayer = () => {
    const [volume, setVolume] = useState(1);
    const [isPlaying, setIsPlaying] = useState(false);
    const audioRef = useRef(null);

    const handleVolumeChange = (event) => {
        const newVolume = event.target.value;
        setVolume(newVolume);
        audioRef.current.volume = newVolume;
    };

    const toggleAudio = () => {
        if (isPlaying) {
            audioRef.current.pause();
        } else {
            audioRef.current.play();
        }
        setIsPlaying(!isPlaying);
    };

    return (
        <div className="audio-player">
            <audio ref={audioRef} loop>
                <source src={osrsSound} type="audio/mpeg" />
            </audio>
            <button onClick={toggleAudio}>
                {isPlaying ? "Stop" : "Play"}
            </button>
            <input 
                type="range" 
                min="0" 
                max="1" 
                step="0.01" 
                value={volume} 
                onChange={handleVolumeChange} 
                className="volume-slider"
            />
        </div>
    );
}

export default AudioPlayer;
