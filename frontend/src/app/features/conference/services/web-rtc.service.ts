import { Injectable, OnDestroy } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';

@Injectable()
export class WebRtcService implements OnDestroy {
    private readonly configuration: RTCConfiguration = {
        iceServers: [
            { urls: 'stun:stun.l.google.com:19302' },
            { urls: 'stun:stun1.l.google.com:19302' },
            { urls: 'stun:stun2.l.google.com:19302' },
            { urls: 'stun:stun3.l.google.com:19302' },
            { urls: 'stun:stun4.l.google.com:19302' },
        ],
        iceCandidatePoolSize: 10,
    };

    private peerConnection: RTCPeerConnection;
    constructor() {}

    ngOnDestroy(): void {
        this.peerConnection.close();
    }

    private iceCandidateInitiated = new Subject<RTCIceCandidate>();
    public iceCandidateInitiated$ = this.iceCandidateInitiated.asObservable();

    private connected = new BehaviorSubject<boolean>(false);
    public connected$ = this.connected.asObservable();

    public setupMediaSources(
        localStream: MediaStream,
        remoteStream: MediaStream,
    ) {
        this.peerConnection = new RTCPeerConnection(this.configuration);

        localStream.getTracks().forEach((track) => {
            this.peerConnection.addTrack(track, localStream);
        });

        this.peerConnection.ontrack = (event) => {
            console.log('Remote stream received');
            event.streams[0].getTracks().forEach((track) => {
                remoteStream.addTrack(track);
            });

            this.connected.next(true);
        };

        console.log('Media sources set up');

        this.peerConnection.onicecandidate = (event) => {
            console.log('Ice candidate initiated');
            event.candidate && this.iceCandidateInitiated.next(event.candidate);
        };
    }

    public addIceCandidate(candidate: RTCIceCandidateInit) {
        if (this.peerConnection.remoteDescription) {
            return this.peerConnection.addIceCandidate(candidate);
        }
        return Promise.resolve();
    }

    public async createOffer() {
        const offerDescription = await this.peerConnection.createOffer();
        await this.peerConnection.setLocalDescription(offerDescription);
        return offerDescription;
    }

    public setRemoteDescription(description: RTCSessionDescriptionInit) {
        if (!this.peerConnection.currentRemoteDescription) {
            return this.peerConnection.setRemoteDescription(description);
        }

        return Promise.resolve();
    }

    public close() {
        this.peerConnection.close();

        this.connected.next(false);
    }

    public async createAnswer() {
        const answerDescription = await this.peerConnection.createAnswer();
        await this.peerConnection.setLocalDescription(answerDescription);
        return answerDescription;
    }
}
